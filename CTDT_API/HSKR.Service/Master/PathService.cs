using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HSKR.Model;
using HSKR.Model.ViewModels;
using System.Text;
using System.Data.SqlClient;
using System;

namespace HSKR.Service.Master
{
    public class PathService : EntityService<MasterPath>
    {
         private  readonly DbSet<MasterPath> _dbSetPaths;
         private DBContext dbContext;
        public PathService(IContext context)
            : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _dbSetPaths = _context.Set<MasterPath>();
        }


        /// <summary>
        ///     get Material by Key, Start , limit. Paging Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<MasterPathFullModel> GetByFilter(string key = "", string style="", int start = 1, int limit = 10)
        {
            //if (start < 1)
            //{
            //    start = 1;
            //}
            //if (string.IsNullOrWhiteSpace(key))
            //{
            //    return _dbSetPaths.OrderBy(c => c.KanjiName).Skip(start - 1).Take(limit).ToList();
            //}
            //var lst = _dbSetPaths.Where(c => c.KanjiName.Contains(key)).OrderBy(c => c.KanjiName).Skip(start - 1).Take(limit).ToList();
            //return lst;
            var sbQuery = new StringBuilder();
            if (start < 1)
            {
                start = 1;
            }
            var query = @"Select p.Id, p.Name, p.KanjiName, p.KatakanaName, p.KanaName, p.CreateDate,               
                p.ModifyDate,
                p.Step,
                p.StyleId,
                p.TimePerPath,
                s.Name as StyleName, 
                p.UrlSource
			    From MasterPath p
			    inner join Style s on  p.StyleId = s.Code";
            sbQuery.Append(query);
            sbQuery.Append(" Where 1=1");
            var lstParam = new List<SqlParameter>();
            if (style != "" && style != "0")
            {
                sbQuery.Append(" And p.StyleId =@StyleId");
                lstParam.Add(new SqlParameter("StyleId", style));
            }                       

            if (!string.IsNullOrWhiteSpace(key))
            {
                sbQuery.Append(" And  p.Name like @Name ");
                lstParam.Add(new SqlParameter("Name", "%" + key + "%"));
            }
            sbQuery.Append(" Order by CONVERT(INT, p.Name) ");
            var paths = dbContext.Database.SqlQuery<MasterPathFullModel>(sbQuery.ToString().Trim(), lstParam.ToArray()).ToList();

            return paths;
        }

        public List<MasterPath> GetByFilter(string key, string styleId)
        {
            var lst = _dbSetPaths.Where(c => c.Name == key && c.StyleId == styleId).ToList();
            return lst;
        }

        public List<MasterPath> GetByFilter(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _dbSetPaths.OrderBy(c => c.Name).ToList();
            }
            var lst = _dbSetPaths.Where(c => c.Name.Contains(key)).OrderBy(c => c.Name).ToList();
            return lst;
        }


        /// <summary>
        /// get count Path
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetByFilterCount(string key = "", string style="")
        {
            var sbQuery = new StringBuilder();
            var query = @"Select p.Id, p.Name, p.KanjiName, p.KatakanaName, p.KanaName, p.CreateDate,               
                p.ModifyDate,
                p.Step,
                p.StyleId,
                p.TimePerPath,
                s.Name as StyleName
                
			    From MasterPath p
			    inner join Style s on  p.StyleId = s.Code";
            sbQuery.Append(query);
            sbQuery.Append(" Where 1=1");
            var lstParam = new List<SqlParameter>();
            if (style != "" && style != "0")
            {
                sbQuery.Append(" And p.StyleId =@StyleId");
                lstParam.Add(new SqlParameter("StyleId", style));
            }

            if (!string.IsNullOrWhiteSpace(key))
            {
                sbQuery.Append(" And  p.Name like @Name ");
                lstParam.Add(new SqlParameter("Name", "%" + key + "%"));
            }

            var count = dbContext.Database.SqlQuery<MasterPathFullModel>(sbQuery.ToString().Trim(), lstParam.ToArray()).OrderBy(c => c.Id).Count();
            return count;
        }

        public Int32 GetMaxPathId()
        {
            var sbQuery = new StringBuilder();

            var query = @"SELECT Isnull(Max(convert(int, Id)),0) as MaxId From MasterPath";
            sbQuery.Append(query);
           
            var paths = dbContext.Database.SqlQuery<Int32>(sbQuery.ToString().Trim()).ToList();
            if (paths.Count > 0)
            {
                return paths[0];
            }
            return 1;
        }
    }
}
