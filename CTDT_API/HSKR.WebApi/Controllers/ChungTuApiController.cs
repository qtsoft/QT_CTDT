using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CTDT.Model;
using CTDT.Model.ViewModels;
using CTDT.Service;
using CTDT.WebApi.Models;
using CTDT.WebApi.Utilities;

namespace CTDT.WebApi.Controllers
{
    public class ChungTuApiController : BaseApiController
    {
          private readonly ChungTuService _chungtuService;

          public ChungTuApiController(ChungTuService chungtuService)
        {
            _chungtuService = chungtuService;
            this.ActionContext.Response = new HttpResponseMessage();
        }

        public ResponseResult GetByFilter(string key = "",string maChungTu="", int maLoaiChungTu=0, string donViBanHanh="",int trangThai=0,   int start = 1, int limit = 10)
        {
            try
            {
                var lst = _chungtuService.GetByFilter(key,maChungTu, maLoaiChungTu,donViBanHanh,trangThai, start, limit);
                var data = new Response<List<ViewChungTuModel>>
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
                var result = new Response<ViewChungTuModel>
                {
                    Message = HttpMessage.ERROR_GET_DATA,
                    Status = false
                };
                return new ResponseResult(result, ActionContext);
            }
        }

        [HttpPost]
        public ResponseResult Create(ChungTuModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var result = new Response<ChungTuModel>
                    {
                        Message = HttpMessage.INVALID_MODEL,
                        Status = false,
                    };
                    ActionContext.Response.StatusCode = HttpStatusCode.BadRequest;
                    return new ResponseResult(result, ActionContext);
                }

                var chungtu = new ChungTu
                {
                   MaChungTu =  model.MaChungTu, 
                   DonViBanHanh = model.DonViBanHanh, 
                   MaDonVi = model.MaDonVi, 
                   MaVach = "",
                  NgayBanHanh = model.NgayBanHanh, 
                  NguoiKy =  model.NguoiKy, 
                  MaLoaiChungTu = model.MaLoaiChungTu, 
                  Ten = model.Ten, 
                  TrangThai = 1
                   
                };
                _chungtuService.Create(chungtu);
                var data = new Response<ChungTuModel>
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
                var data = new Response<ChungTuModel>
                {
                    Message = HttpMessage.ERROR_CREATE,
                    Status = false
                };
                ActionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                return new ResponseResult(data, ActionContext);
            }
        }

        [HttpPost]
        public ResponseResult Edit(ChungTuModel model)
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
                var ct = _chungtuService.GetEntityById(model.Id);
                // check exist
                if (ct == null)
                {
                    var result = new Response<ChungTuModel>
                    {
                        Message = HttpMessage.DATA_NOT_FOUND,
                        Status = false,
                    };
                    ActionContext.Response.StatusCode = HttpStatusCode.NotFound;
                    return new ResponseResult(result, ActionContext);
                }
                ct.MaChungTu = model.MaChungTu;

                ct.MaDonVi = model.MaDonVi;
                ct.MaChungTu = model.MaChungTu;
                ct.MaLoaiChungTu = model.MaLoaiChungTu;
                ct.NgayBanHanh = model.NgayBanHanh;
                ct.NguoiKy = model.NguoiKy;
                ct.Ten = model.Ten;
                ct.TrangThai = model.TrangThai;
                ct.DonViBanHanh = model.DonViBanHanh;
                _chungtuService.Update(ct);
                var data = new Response<ChungTuModel>
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
                var data = new Response<ChungTuModel>
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
               
                var loaiCT = _chungtuService.GetEntityById(id);
                // check exist
                if (loaiCT == null)
                {
                    var result = new Response<ChungTuModel>
                    {
                        Message = HttpMessage.DATA_NOT_FOUND,
                        Status = false,
                    };
                    ActionContext.Response.StatusCode = HttpStatusCode.NotFound;
                    return new ResponseResult(result, ActionContext);
                }
               

                _chungtuService.Delete(loaiCT);
                var data = new Response<ChungTuModel>
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
