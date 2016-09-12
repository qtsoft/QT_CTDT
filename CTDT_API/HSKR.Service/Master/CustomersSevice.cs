using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HSKR.Model;
using System.Text;
using HSKR.Model.ViewModels;
using System.Data.SqlClient;

namespace HSKR.Service.Master
{
    public class CustomersService : EntityService<Customer>
    {
        private readonly DbSet<Customer> _dbSetCustomer;
        private readonly DBContext dbContext;
        public CustomersService(IContext context)
            : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _dbSetCustomer = _context.Set<Customer>();
        }

        
        /// <summary>
        /// Get Customer by Key, Start , limit. Paging Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="limmit"></param>
        /// <returns></returns>
        public List<CustomerFullModel> GetByFilter(string key = "", int start = 1, int limit = 10)
        {            
            var sbQuery = new StringBuilder();
            if (start < 1)
            {
                start = 1;
            }
            var query = @"Select c.Id, c.Name,c.WorkEmail,               
                c.HomeEmail,
                c.WorkPhone,
                c.HomePhone,
                c.HomeAddress,
               
                c.WorkAddress,
                c.CreateDate,                
               			                    
                c.TerminationDate,
                c.CountryCode,               
                co.Name as CountryName
			    From Customers c 
			    inner join MasterCountry co on  c.CountryCode = co.Code";
            sbQuery.Append(query);
            var lstParam = new List<SqlParameter>();          

            if (!string.IsNullOrWhiteSpace(key))
            {
                sbQuery.Append(" And  c.Name like @Name ");
                lstParam.Add(new SqlParameter("Name", "%" + key + "%"));
            }   
            var customers =
                dbContext.Database.SqlQuery<CustomerFullModel>(sbQuery.ToString().Trim(), lstParam.ToArray()).OrderBy(c => c.Name).Skip(start - 1).Take(limit).ToList();

            return customers;
        }

        /// <summary>
        /// Get Customer by Key
        /// </summary>
        /// <param name="key"></param>        
        /// <returns></returns>
        public List<Customer> GetByFilter(string key = "")
        {           
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetCustomer.OrderBy(c => c.Name).ToList();
            }
            var lst = _dbSetCustomer.Where(c => c.Name.Contains(key)).OrderBy(c => c.Name).ToList();
            return lst;
        }

        /// <summary>
        /// get count Path
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetByFilterCount(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetCustomer.ToList().Count();
            }
            var count = _dbSetCustomer.Where(c => c.Name.Contains(key)).ToList().Count();
            return count;
        }
    }
}
