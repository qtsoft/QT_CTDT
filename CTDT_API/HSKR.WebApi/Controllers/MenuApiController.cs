using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HSKR.Model.ViewModels;
using HSKR.Service.Common;
using HSKR.WebApi.Models;
using HSKR.WebApi.Utilities;
using System.Text;
using System.Data.SqlClient;
using HSKR.Model;

namespace HSKR.WebApi.Controllers
{
    public class MenuApiController : BaseApiController
    {
        private readonly MenuService _menuService;
        private DBContext dbContext;
        public MenuApiController(MenuService menuService)
        {
            dbContext = new DBContext();
            _menuService = menuService;
        }

        // GET: Menu
        public ActionResult Index()
        {
            return null;
        }

        [HttpGet]
        public Response<List<MenuModel>> GetAll()
        {
            try
            {
                var menu = _menuService.GetAll().Where(m => m.IsDisable == false);
                return new Response<List<MenuModel>>
                {
                    Status = true,
                    Data = menu.Select(b => new MenuModel
                    {
                        Name = b.Name,
                        Id = b.Id,
                        Description = b.Description,
                        Url = b.Url,
                        IconCls = b.IconCls,
                        MenuLevel = b.MenuLevel,
                        IsActive = b.IsActive,
                        MenuOrder = b.MenuOrder.GetValueOrDefault(),
                        ParentId = b.ParentId
                    }).ToList()
                };
            }
            catch (Exception)
            {
                return new Response<List<MenuModel>>
                {
                    Status = false,
                    Message = HttpMessage.ERROR_GET_DATA
                };
            }
        }

        [HttpGet]
        public Response<List<MenuModel>> GetByFilter(int userId)
        {
            try
            {
                var menus = _menuService.GetByFilter(userId);

//                var sbQuery = new StringBuilder();

//                var query = @"select m.* from UserGroupMenu um
//                        inner join Menu m on m.Id=um.MenuId
//                        inner join UserGroup ug on ug.GroupId=um.GroupId and ug.UserId=@UserId Order by m.MenuOrder";
//                sbQuery.Append(query);
//                var lstParam = new List<SqlParameter>();
//                lstParam.Add(new SqlParameter("UserId", userId));


//                var menus = dbContext.Database.SqlQuery<MenuModel>(sbQuery.ToString().Trim(), lstParam.ToArray()).ToList();

                return new Response<List<MenuModel>>
                {
                    Status = true,
                    Data = menus.Select(b => new MenuModel
                    {
                        Name = b.Name,
                        Id = b.Id,
                        Description = b.Description,
                        Url = b.Url,
                        IconCls = b.IconCls,
                        MenuLevel = b.MenuLevel,
                        IsActive = b.IsActive,
                        MenuOrder = b.MenuOrder,
                        ParentId = b.ParentId
                    }).ToList()
                };
            }
            catch (Exception)
            {
                return new Response<List<MenuModel>>
                {
                    Status = false,
                    Message = HttpMessage.ERROR_GET_DATA
                };
            }
        }
    }
}