using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HSKR.Model;
using HSKR.Model.ViewModels;

namespace HSKR.Service.Master
{
    public class RequestTaskService : EntityService<RequestTask>
    {
        private readonly DbSet<RequestTask> _requestTasks;
        private readonly DbSet<Employee> _employee;
        private DBContext dbContext;

        public RequestTaskService(IContext context)
            : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _requestTasks = _context.Set<RequestTask>();
            _employee = _context.Set<Employee>();
        }

        public List<RequestTaskModel> GetByFilter(string key = "", int start = 1, int limit = 10)
        {
            if (start < 1)
            {
                start = 1;
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                var lst = _requestTasks.OrderBy(c => c.PathId).Skip(start - 1).Take(limit);
                return lst.Select(c => new RequestTaskModel
                {
                    Id = c.Id,
                    ProductName = c.ProductName,
                    Color = c.Color,
                    CreateDate = c.CreateDate,
                    Size = c.Size,
                    ProductId = c.ProductId,
                    PathId = c.PathId,
                    Description = c.Description,
                    EmployeeName = _employee.FirstOrDefault(x => x.Id == c.EmployeeId).Name,
                    EmployeeId=c.EmployeeId
                }).ToList();
            }
            var lstRequestTasks =
                _requestTasks.Where(c => c.ProductId.Equals(key)).OrderBy(c => c.ProductId).Skip(start - 1).Take(limit);
            return lstRequestTasks.Select(c => new RequestTaskModel
            {
                Id = c.Id,
                ProductName = c.ProductName,
                Color = c.Color,
                CreateDate = c.CreateDate,
                Size = c.Size,
                ProductId = c.ProductId,
                PathId = c.PathId, 
                Description = c.Description,
                EmployeeName = dbContext.Employees.SingleOrDefault(x => x.Id == c.EmployeeId).Name,
                EmployeeId = c.EmployeeId
            }).ToList();
        }

        public List<RequestTaskModel> GetByFilter(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _requestTasks.OrderBy(c => c.ProductName).Select(c => new RequestTaskModel
                {
                    Id = c.Id,
                    ProductName = c.ProductName,
                    Color = c.Color,
                    CreateDate = c.CreateDate,
                    Size = c.Size,
                    ProductId = c.ProductId,
                    PathId = c.PathId, 
                    Description = c.Description
                }).ToList();
            }
            return
                _requestTasks.Where(c => c.ProductName.Contains(key))
                    .OrderBy(c => c.ProductName)
                    .Select(c => new RequestTaskModel
                    {
                        Id = c.Id,
                        ProductName = c.ProductName,
                        Color = c.Color,
                        CreateDate = c.CreateDate,
                        Size = c.Size,
                        ProductId = c.ProductId,
                        PathId = c.PathId
                        ,
                        Description = c.Description
                    }).ToList();
        }

        public int GetByFilterCount(string productid = "")
        {
            if (string.IsNullOrWhiteSpace(productid))
            {
                return _requestTasks.ToList().Count();
            }
            var count = _requestTasks.Where(c => c.ProductId.Equals(productid)).ToList().Count();
            return count;
        }
    }
}