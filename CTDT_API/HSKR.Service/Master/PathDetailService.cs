using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HSKR.Model;
using System.Text;
using System.Data.SqlClient;
using HSKR.Model.ViewModels;

namespace HSKR.Service.Master
{
    public class PathDetailService : EntityService<PathDetail>
    {
        private  readonly DbSet<PathDetail> _dbSetPathDetails;
        private DBContext dbContext;
        public PathDetailService(IContext context)
             : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _dbSetPathDetails = _context.Set<PathDetail>();
        }


        /// <summary>
        /// get PathDetail by Key, Start , limit. Paging Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<PathDetaiFullModel> GetByFilter(string key = "", string pathId = "", int start = 1, int limit = 10)
        {
            var sbQuery = new StringBuilder();
            if (start < 1)
            {
                start = 1;
            }
            var query = @"Select pd.Id, pd.Name, pd.Quantity, pd.TimeWork, pd.Step, pd.YarnColor,               
                pd.PathId,
                p.Name as PathName
			    From PathDetail pd
			    inner join MasterPath p on  pd.PathId = p.Id";
            sbQuery.Append(query);
            sbQuery.Append(" Where 1=1");
            if (pathId != "" && pathId != "0")
            {
                sbQuery.Append(" And pd.PathId =@PathId");
            }            

            var lstParam = new List<SqlParameter>();
            lstParam.Add(new SqlParameter("PathId", pathId));

            if (!string.IsNullOrWhiteSpace(key))
            {
                sbQuery.Append(" And  pd.Name like @Name ");
                lstParam.Add(new SqlParameter("Name", "%" + key + "%"));
            }

            var pathDetais = dbContext.Database.SqlQuery<PathDetaiFullModel>(sbQuery.ToString().Trim(), lstParam.ToArray()).Skip(start - 1).Take(limit).ToList();

            return pathDetais;
        }


        public List<PathDetail> GetByFilter(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetPathDetails.OrderBy(c => c.Name).ToList();
            }
            var lst = _dbSetPathDetails.Where(c => c.Name.Contains(key)).OrderBy(c => c.Name).ToList();
            return lst;
        }


        /// <summary>
        /// get count PathDetail
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetByFilterCount(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetPathDetails.ToList().Count();
            }
            var count = _dbSetPathDetails.Where(c => c.Name.Contains(key)).ToList().Count();
            return count;
        }
    }
}
