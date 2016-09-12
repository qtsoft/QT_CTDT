using System.Collections.Generic;
using System.Data.Entity;
using HSKR.Model;
using HSKR.Model.ViewModels;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace HSKR.Service.Master
{
    public class ColorService : EntityService<Color>
    {
        private readonly DbSet<Color> _colors;
        private DBContext dbContext;
        public ColorService(IContext context)
            : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _colors = _context.Set<Color>();
        }

        public List<ColorModel> GetByFilter(string key = "", int start = 1, int limit = 10)
        {
            if (start < 1)
            {
                start = 1;
            }
            var sbQuery = new StringBuilder();

            var query = @"Select c.Code, c.ColorName, c.Description,CAST(CASE ";			    
            sbQuery.Append(query);
            sbQuery.Append(" WHEN m.Id in(Select MaterialId from Product) ");
            sbQuery.Append(" THEN 1 ELSE 0  ");
            sbQuery.Append(" END AS bit) as ColorUsed ");
            sbQuery.Append(" FROM Color c ");
            sbQuery.Append(" INNER JOIN MasterMaterial m ON m.ColorCode=c.Code ");


            var lstParam = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(key))
            {
                sbQuery.Append(" Where c.ColorName like @Name ");
                lstParam.Add(new SqlParameter("Name", "%" + key + "%"));
            }
            var lstColors = dbContext.Database.SqlQuery<ColorModel>(sbQuery.ToString().Trim(), lstParam.ToArray()).OrderBy(c => c.ColorName).Skip(start - 1).Take(limit).ToList();

            //if (start < 1)
            //{
            //    start = 1;
            //}
            //if (string.IsNullOrWhiteSpace(key))
            //{
            //   var lst = _colors.OrderBy(c => c.ColorName).Skip(start - 1).Take(limit);
            //    return lst.Select(c => new  ColorModel
            //    {
            //       Code = c.Code, 
            //       ColorName =  c.ColorName, 
            //       Description = c.Description
            //    }).ToList();
            //}
            //var  lstColors  = _colors.Where(c => c.ColorName.Contains(key)).OrderBy(c => c.ColorName).Skip(start - 1).Take(limit);
            return lstColors.Select(c => new ColorModel
            {
                Code = c.Code,
                ColorName = c.ColorName,
                Description = c.Description,
                ColorUsed = c.ColorUsed,
            }).ToList();

        }

        public List<ColorModel> GetByFilter(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {

                return _colors.OrderBy(c => c.ColorName).Select(c => new ColorModel
                {
                    Code = c.Code,
                    ColorName = c.ColorName,
                    Description = c.Description

                }).ToList();
            }
            return _colors.Where(c=>c.ColorName.Contains(key)).OrderBy(c => c.ColorName).Select(c => new ColorModel
            {
                Code = c.Code,
                ColorName = c.ColorName,
                Description = c.Description

            }).ToList();
        }


        public int GetByFilterCount(string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _colors.ToList().Count();
            }
            var count = _colors.Where(c => c.ColorName.Contains(key)).ToList().Count();
            return count;
        }

    }
}
