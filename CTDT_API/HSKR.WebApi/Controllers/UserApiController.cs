using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using HSKR.Identity;
using CTDT.Model;
using CTDT.Service;
using CTDT.WebApi.Models;
using CTDT.WebApi.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json.Linq;
using ChangePasswordBindingModel = HSKR.Identity.ChangePasswordBindingModel;

namespace CTDT.WebApi.Controllers
{
    public class UserApiController : BaseApiController
    {
        private readonly IAuthenticationManager authenticationManager;
        private MyUserManager userManager;


        public MyUserManager UserManager
        {
            get
            {
                //return _userManager ?? Request.GetOwinContext().GetUserManager<MyUserManager>();
                return userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<MyUserManager>();
            }
            private set { userManager = value; }
        }

        public UserApiController(MyUserManager userManager, IAuthenticationManager authenticationManager)
        {
            this.userManager = userManager;
            this.authenticationManager = authenticationManager;
            ActionContext.Response = new HttpResponseMessage();
        }

        [HttpPost]
        public ResponseResult SignIn(UserModel userModel)
        {
            string strToken = @"pjh6iE910kVJ338fyKlAa2kIQltw9YfabBiEu4ck_8A3ciiesxEH_v0rNMtO6w5UK_JxWvAGn9BVadqNpyqzD0oiWOIFd7vsWev8XUbUeonyPYzG-s14FvPVTaoRn1ur5chMgSALCj8Q_hYtwgJ0spMgmPAlQOHjh2QZw8YMozFO04Aj4aMIrCUq4lp5jIW_1HyF2hfcFEOcRpnuHJSo0QNLU72QRklWDCRI0xPV7yYCSIjElPJ2OLvHQ20ultDDCxGZZFyFRwHI2aLYhreb1iEFHPKB6GA5G9WqX6h9i2Kg9fpjUMPatf-Ot2o4vEV5NtGFyXdVzuOCNuvhdJ6htz8ezoo";
            try
            {
                var tenTk = "0";
                
                var user = UserManager.Find(userModel.UserName, userModel.Password);

                if (user == null)
                {
                    var data = new Response<UserViewModel>
                    {
                        Message = HttpMessage.LOGIN_INCORECT,
                        Data = null,
                        Status = false
                    };
                    ActionContext.Response.StatusCode = HttpStatusCode.Unauthorized;

                    return new ResponseResult(data, ActionContext);
                }

                var result = JObject.Parse(user.ToJson());
                tenTk = result["userName"].ToString().Trim();
              
                var userResponse = new Response<UserViewModel>
                {
                    Message = "Login success",
                    Status = true,
                    Data = new UserViewModel
                    {
                        FullName ="",
                        Id = long.Parse(result["id"].ToString()),
                        UserName = tenTk,
                        PhoneNumber = result["phoneNumber"].ToString(),
                        Email = result["email"].ToString(),
                        token_type = "bearer",
                        access_token = strToken
                    }
                };

                return new ResponseResult(userResponse, ActionContext);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                ActionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                var data = new Response<UserViewModel>
                {
                    Message = HttpMessage.LOGIN_FAIL,
                    Status = false
                };
                return new ResponseResult(data, ActionContext);
            }
        }


        /* [HttpGet]
        public ResponseResult SigUp(string userName, string fullname, string phoneNumber, string password)
        {
            var user = new MyUser() { UserName = userName, Email = userName, PhoneNumber = phoneNumber };

            IdentityResult result =  UserManager.Create(user, "Beetsoft@123");

            if (!result.Succeeded)
            {
                // return GetErrorResult(result);
            }
            
            return Ok();
        }
*/


        [HttpGet]
        public async Task<IHttpActionResult> Register(string userName, string fullname, string phoneNumber,
            string password)
        {
            var user = new MyUser {UserName = userName, Email = userName, PhoneNumber = phoneNumber};

            var result = await UserManager.CreateAsync(user, "Ab@123456");

            if (!result.Succeeded)
            {
                // return GetErrorResult(result);
            }
            return Ok();
        }

        [HttpPost]
        public ResponseResult ChangePass(HSKR.Identity.ChangePasswordBindingModel model)
        {
            try
            {
                if (!model.NewPassword.Trim().Equals(model.ConfirmPassword.Trim()))
                {
                    var data = new Response<UserViewModel>
                    {
                        Message = HttpMessage.CHANGE_PASS_OLD_PASS,
                        Data = null,
                        Status = false
                    };
                    ActionContext.Response.StatusCode = HttpStatusCode.Redirect;
                    return new ResponseResult(data, ActionContext);
                }
                var user = userManager.Find(model.UserName, model.OldPassword);
                if (user == null)
                {
                    var data = new Response<UserViewModel>
                    {
                        Message = HttpMessage.CHANGE_PASS_WRONG_ACC,
                        Data = null,
                        Status = false
                    };
                    ActionContext.Response.StatusCode = HttpStatusCode.Redirect;
                    return new ResponseResult(data, ActionContext);
                }

                var result = JObject.Parse(user.ToJson());
                var userId = result["id"];
                var changePassResult = userManager.ChangePasswordAsync(long.Parse(userId.ToString()), model.OldPassword, model.NewPassword.Trim());
                if (!changePassResult.Result.Succeeded)
                {
                    var repon = new Response<UserViewModel>
                    {
                        Message = changePassResult.Result.Errors.ToJson(),
                        Status = false,
                    };
                    ActionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                    return new ResponseResult(repon, ActionContext);
                }
               
                var userResponse = new Response<UserViewModel>
                {
                    Message = "Change password success",
                    Status = true,
                };

                return new ResponseResult(userResponse, ActionContext);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                ActionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                var data = new Response<UserViewModel>
                {
                    Message = HttpMessage.ERROR_CHANGE_PASS,
                    Status = false
                };
                return new ResponseResult(data, ActionContext);
            }
        }

        // POST api/AccountApi/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(HSKR.Identity.ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(long.Parse(User.Identity.GetUserId()), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return NotFound();
            }

            return Ok();
        }


        /// <summary>
        /// Logout 
        /// </summary>
        /// <returns></returns>
         [HttpPost]
        public ResponseResult Logout(int userId, string userName, int employeeId, string employeeName )
        {
            try
            {
              

               // authenticationManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);

                //var employeeMoney = _viewEmployeeMoneyService.AlertMoneyByEmployee(employeeId);
                //if(employeeMoney.TotalMoney==-1)
                //{
                //    var dataNull = new Response<UserViewModel>
                //    {
                //        Message = "Sign out success",
                //        Status = true
                //    };

                //    return new ResponseResult(dataNull, ActionContext);
                //}

                var data = new Response<object>
                {
                    Message = "Sign out success",
                    Status = true
                };
                return new ResponseResult(data, ActionContext);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                var data = new Response<UserViewModel>
                {
                    Message = HttpMessage.ERROR_SIGN_OUT,
                    Status = false,
                };
                return new ResponseResult(data, ActionContext);
            }
        }
    }
}