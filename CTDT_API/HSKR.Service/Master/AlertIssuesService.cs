using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HSKR.Model;
using HSKR.Model.ViewModels;
using System.Text;
using System.Data.SqlClient;

namespace HSKR.Service.Master
{
    public class AlertIssuesService : EntityService<AlertIssue>
    {
        private readonly DbSet<AlertIssue> _alertIssues;
        private readonly DbSet<MasterIssue> _masterIssues;
        private DBContext dbContext;
        //private readonly DbSet<Customer> _dbSetIssue;

        public AlertIssuesService(IContext context)
            : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _alertIssues = _context.Set<AlertIssue>();
            _masterIssues = _context.Set<MasterIssue>();
        }

        public List<AlertIssueFullModel> GetByFilter(string key = "", int start = 1, int limit = 10)
        {
            var sbQuery = new StringBuilder();
            if (start < 1)
            {
                start = 1;
            }
            var query = @"Select a.Id, a.IssuesId, a.EmployeeId,a.CreateDate,a.ProductId,
               
                a.Status,
                a.UpdateTime,
                a.Description,
                i.Name as IssuesName,
                e.Name as EmployeeName,
                o.Id as ProductId,
                o.Name as ProductName,
                c.Id as CustomerId,
                c.Name as CustomerName

			    From AlertIssues a 
			    inner join MasterIssues i on  i.Id = a.IssuesId
                inner join Employee e on e.Id=a.EmployeeId
                left join Orders o on o.Id=a.ProductId
                left join Customers c on c.Id=o.IdCustomer
                Where a.Status = 'true'";
            sbQuery.Append(query);
           

            var lstParam = new List<SqlParameter>();          

            if (!string.IsNullOrWhiteSpace(key))
            {
                sbQuery.Append(" Where i.Name like @Name ");
                lstParam.Add(new SqlParameter("Name", "%" + key + "%"));
            }
            sbQuery.Append(" Order By a.CreateDate DESC");

            var issues = dbContext.Database.SqlQuery<AlertIssueFullModel>(sbQuery.ToString().Trim(), lstParam.ToArray()).Skip(start - 1).Take(limit).ToList();

            return issues;
        }
        //


        public int GetCountAlertIssue()
        {
            var sbQuery = new StringBuilder();
            var query = @"Select count(*) from AlertIssues Where CONVERT(char(10), AlertIssues.CreateDate, 126) = CONVERT(char(10), GETDATE(), 126) and AlertIssues.Status = 'true'";
            sbQuery.Append(query);

            var issues = dbContext.Database.SqlQuery<int>(sbQuery.ToString().Trim()).FirstOrDefault();
            return issues;
        }
        //public List<AlertIssueModel> GetByFilter(string key = "")
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //    {
        //        return _alertIssues.OrderBy(c => c.ProductName).Select(c => new AlertIssueModel
        //        {
        //            Id = c.Id,
        //            IssuesId = c.IssuesId,
        //            EmployeeId = c.EmployeeId,
        //            CreateDate = c.CreateDate,
        //            ProductId = c.ProductId,
        //            Status = c.Status,
        //            UpdateTime = c.UpdateTime,
        //            Description = c.Description
        //        }).ToList();
        //    }
        //    return
        //        _alertIssues.Where(c => c.ProductName.Contains(key))
        //            .OrderBy(c => c.ProductName)
        //            .Select(c => new AlertIssueModel
        //            {
        //                Id = c.Id,
        //                IssuesId = c.IssuesId,
        //                EmployeeId = c.EmployeeId,
        //                CreateDate = c.CreateDate,
        //                ProductId = c.ProductId,
        //                Status = c.Status,
        //                UpdateTime = c.UpdateTime,
        //                Description = c.Description
        //            }).ToList();
        //}

        public int GetByFilterCount()
        {
            //if (string.IsNullOrWhiteSpace(key))
            //{
            //    return _alertIssues.ToList().Count();
            //}

            var count = _alertIssues.ToList().Count();           

            return count;
        }
    }
}