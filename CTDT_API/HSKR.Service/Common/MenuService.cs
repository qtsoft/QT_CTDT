using System.Collections.Generic;
using System.Data.Entity;
using HSKR.Model;
using HSKR.Model.ViewModels;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

namespace HSKR.Service.Common
{
    public class MenuService : EntityService<Menu>
    {
        private readonly DbSet<Menu> _dbSetMenus;
        private DBContext dbContext;
        public MenuService(IContext context)
              : base(context)
        {
            dbContext = new DBContext();
            _context = context;
            _dbSet = _context.Set<Menu>();
        }

        /// <summary>
        /// get Menu by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuModel> GetByFilter(int userId = 0)
        {
            var sbQuery = new StringBuilder();

            var query = @"select m.* from UserGroupMenu um
                        inner join Menu m on m.Id=um.MenuId
                        inner join UserGroup ug on ug.GroupId=um.GroupId and ug.UserId=@UserId where m.IsDisable='False' Order by m.MenuLevel, m.MenuOrder";
            sbQuery.Append(query);
            var lstParam = new List<SqlParameter>();
            lstParam.Add(new SqlParameter("UserId", userId));


            var menus = dbContext.Database.SqlQuery<MenuModel>(sbQuery.ToString().Trim(), lstParam.ToArray()).ToList();

            return menus;
        }
    }
}
