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
using CTDT.Model;


namespace CTDT.WebApi.Controllers
{
    public class LoaiChungTuApiController : BaseApiController
    {

        private readonly LoaiChungTuService _loaiChungtuService;
       
        public LoaiChungTuApiController(LoaiChungTuService loaiChungtuService)
        {
            _loaiChungtuService = loaiChungtuService;
            this.ActionContext.Response = new HttpResponseMessage();
        }

        public ResponseResult GetByFilter(string key = "", int start = 1, int limit = 10)
        {
            try
            {
                var lst = _loaiChungtuService.GetByFilter(key, start, limit);
                var data = new Response<List<LoaiChungTuModel>>
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

        [HttpPost]
        public ResponseResult Create(LoaiChungTuModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var result = new Response<LoaiChungTuModel>
                    {
                        Message = HttpMessage.INVALID_MODEL,
                        Status = false,
                    };
                    ActionContext.Response.StatusCode = HttpStatusCode.BadRequest;
                    return new ResponseResult(result, ActionContext);
                }

                var loaiCT = new CTDT.Model.LoaiChungTu
                {
                   Name = model.Name, 
                   Description = model.Description
                };
                _loaiChungtuService.Create(loaiCT);
                var data = new Response<LoaiChungTuModel>
                {
                    Message = "Create Success",
                    Status = true,
                    Data = model
                };

                return new ResponseResult(data, ActionContext);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                var data = new Response<LoaiChungTuModel>
                {
                    Message = HttpMessage.ERROR_CREATE,
                    Status = false
                };
                ActionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                return new ResponseResult(data, ActionContext);
            }
        }

        [HttpPost]
        public ResponseResult Edit(LoaiChungTuModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var result = new Response<LoaiChungTuModel>
                    {
                        Message = HttpMessage.INVALID_MODEL,
                        Status = false,
                    };
                    ActionContext.Response.StatusCode = HttpStatusCode.BadRequest;
                    return new ResponseResult(result, ActionContext);
                }
                var loaiCT = _loaiChungtuService.GetEntityById(model.Id);
                // check exist
                if (loaiCT == null)
                {
                    var result = new Response<LoaiChungTuModel>
                    {
                        Message = HttpMessage.DATA_NOT_FOUND,
                        Status = false,
                    };
                    ActionContext.Response.StatusCode = HttpStatusCode.NotFound;
                    return new ResponseResult(result, ActionContext);
                }
                loaiCT.Name = model.Name;
                loaiCT.Description = model.Description;

                _loaiChungtuService.Update(loaiCT);
                var data = new Response<LoaiChungTuModel>
                {
                    Message = "Edit Success",
                    Status = true,
                    Data = model
                };

                return new ResponseResult(data, ActionContext);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                var data = new Response<LoaiChungTuModel>
                {
                    Message = HttpMessage.ERROR_EDIT,
                    Status = false
                };
                ActionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                return new ResponseResult(data, ActionContext);
            }
        }

        [HttpPost]
        public ResponseResult Delete(int id)
        {
            try
            {
               
                var loaiCT = _loaiChungtuService.GetEntityById(id);
                // check exist
                if (loaiCT == null)
                {
                    var result = new Response<LoaiChungTuModel>
                    {
                        Message = HttpMessage.DATA_NOT_FOUND,
                        Status = false,
                    };
                    ActionContext.Response.StatusCode = HttpStatusCode.NotFound;
                    return new ResponseResult(result, ActionContext);
                }
               

                _loaiChungtuService.Delete(loaiCT);
                var data = new Response<LoaiChungTuModel>
                {
                    Message = "delete Success",
                    Status = true,
                };

                return new ResponseResult(data, ActionContext);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                var data = new Response<LoaiChungTuModel>
                {
                    Message = HttpMessage.ERROR_DELETE,
                    Status = false
                };
                ActionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                return new ResponseResult(data, ActionContext);
            }
        }
    }
}
