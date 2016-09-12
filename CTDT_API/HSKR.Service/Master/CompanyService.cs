using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HSKR.Model;
using System.Text;
using System.Data.SqlClient;
using HSKR.Model.ViewModels;

namespace HSKR.Service.Master
{
    public class CompanyService : EntityService<MasterCompany>
    {
        private readonly DbSet<MasterCompany> _dbSetCompany;
        private DBContext dbContext;
        public CompanyService(IContext context) : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _dbSetCompany = _context.Set<MasterCompany>();
        }       

        /// <summary>
        /// get Company by Key, Start , limit. Paging Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="limmit"></param>
        /// <returns></returns>
        public List<MasterCompanyFullModel> GetByFilter(string key = "", int start = 1, int limit = 10)
        {
            var sbQuery = new StringBuilder();
            if (start < 1)
            {
                start = 1;
            }            
            var query = @"Select c.Id, c.Name,c.CountryCode,
                coun.Name as CountryName
			    From MasterCompany c 
			    inner join MasterCountry coun on  c.CountryCode = coun.Code";
            sbQuery.Append(query);
            

            var lstParam = new List<SqlParameter>();
            

            if (!string.IsNullOrWhiteSpace(key))
            {
                sbQuery.Append(" Where  c.Name like @Name ");
                lstParam.Add(new SqlParameter("Name", "%" + key + "%"));
            }
            var lst = dbContext.Database.SqlQuery<MasterCompanyFullModel>(sbQuery.ToString().Trim(), lstParam.ToArray()).Skip(start - 1).Take(limit).ToList();
            return lst;
           
        }

        /// <summary>
        /// get Company by Key
        /// </summary>
        /// <param name="key"></param>        
        /// <returns></returns>
        public List<MasterCompany> GetByFilter(string key = "")
        {            
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetCompany.OrderBy(c => c.Name).ToList();
            }
            var lst = _dbSetCompany.Where(c => c.Name.Contains(key)).OrderBy(c => c.Name).ToList();
            return lst;
        }

        public int GetByFilterCount(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetCompany.ToList().Count();
            }
            var count = _dbSetCompany.Where(c => c.Name.Contains(key)).ToList().Count;
            return count;
        }
    }
}
