using System.Collections.Generic;
using System.Data.Entity;
using HSKR.Model;
using HSKR.Model.ViewModels;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace HSKR.Service.Master
{
    public class StyleService : EntityService<Style>
    {
        private readonly DbSet<Style> _styles;
        private DBContext dbContext;
        public StyleService(IContext context)
            : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _styles = _context.Set<Style>();
        }

        public List<StyleModel> GetByFilter(string key = "", int start = 1, int limit = 10)
        {
           
            if (start < 1)
            {
                start = 1;
            }
            if (string.IsNullOrWhiteSpace(key))
            {
               var lst = _styles.OrderBy(c => c.Name).Skip(start - 1).Take(limit);
                return lst.Select(c => new  StyleModel
                {
                   Code = c.Code, 
                   Name =  c.Name, 
                   Description = c.Description
                }).ToList();
            }
            var  lstStyles  = _styles.Where(c => c.Name.Contains(key)).OrderBy(c => c.Name).Skip(start - 1).Take(limit);
            return lstStyles.Select(c => new StyleModel
            {
                Code = c.Code,
                Name = c.Name,
                Description = c.Description
            }).ToList();

        }

        public List<StyleModel> GetByFilter(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {

                return _styles.OrderBy(c => c.Name).Select(c => new StyleModel
                {
                    Code = c.Code,
                    Name = c.Name,
                    Description = c.Description

                }).ToList();
            }
            return _styles.Where(c=>c.Name.Contains(key)).OrderBy(c => c.Name).Select(c => new StyleModel
            {
                Code = c.Code,
                Name = c.Name,
                Description = c.Description

            }).ToList();
        }


        public int GetByFilterCount(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _styles.ToList().Count();
            }
            var count = _styles.Where(c => c.Name.Contains(key)).ToList().Count();
            return count;
        }

        public List<Style> GetAutoNumber()
        {
            var sbQuery = new StringBuilder();

            var query = @"SELECT top 1 * From Style Order by CAST(Code AS INT) DESC";
            sbQuery.Append(query);          

            var styles = dbContext.Database.SqlQuery<Style>(sbQuery.ToString().Trim()).ToList();

            return styles;
        }

    }
}
