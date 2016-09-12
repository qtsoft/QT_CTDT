using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CTDT.Service;
using CTDT.Model.ViewModels;
using CTDT.WebApi.Utilities;
using CTDT.WebApi.Models;

namespace CTDT.WebApi.Controllers
{
    public class KiemTraChungTuApiController : BaseApiController
    {

        private readonly KetQuaKiemTraService _ketQuaChungtuService;

        public KiemTraChungTuApiController(KetQuaKiemTraService ketQuaChungtuService)
        {
            _ketQuaChungtuService = ketQuaChungtuService;
            this.ActionContext.Response = new HttpResponseMessage();
        }

        public ResponseResult GetByFilter(string key = "", int start = 1, int limit = 10)
        {
            try
            {
                var lst = _ketQuaChungtuService.GetByFilter(key, start, limit);
                var data = new Response<List<KetQuaKiemTraModel>>
                {
                    Status = true, 
                    Data = lst,
                    Message = "Success!"

                };
                return new ResponseResult(data, ActionContext);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ActionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                var result = new Response<object>
                {
                    Message = HttpMessage.ERROR_GET_DATA,
                    Status = false
                };
                return new ResponseResult(result, ActionContext);
            }
        }
    }
}
