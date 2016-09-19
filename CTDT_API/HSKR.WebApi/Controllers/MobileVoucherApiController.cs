using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using CTDT.WebApi.Models;
using CTDT.Model.ViewModels;
using CTDT.Model;
using CTDT.Service;
using CTDT.WebApi.Utilities;
using System.Net;

namespace CTDT.WebApi.Controllers
{
    public class MobileVoucherApiController : BaseApiController
    {
        private readonly ChungTuService _chungtuService;
        private readonly KetQuaKiemTraService _ketQuaService;
        public MobileVoucherApiController(ChungTuService chungtuService, KetQuaKiemTraService ketQuaService)
        {
            _chungtuService = chungtuService;
            _ketQuaService = ketQuaService;
            this.ActionContext.Response = new HttpResponseMessage();
        }

        [HttpGet]
        public ResponseResult CheckVoucher(string Code, string IsBarcode)
        {
            try
            {
                string domain;
                var folderPath = "";
                var fileName = "";
                var myuri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                var pathQuery = myuri.PathAndQuery;
                var appUrl = HttpRuntime.AppDomainAppVirtualPath;
                domain = myuri.ToString().Replace(pathQuery, "") + appUrl.Trim();
                logger.Trace("domain: ", domain.ToLower());

                if (IsBarcode.Equals("1"))
                {
                    var chungTu = _chungtuService.Find(c => (c.MaVach.ToUpper().Trim() == Code.ToUpper().Trim() && c.TrangThai == 4)).FirstOrDefault();
                    if (chungTu == null)
                    {
                          return new ResponseResult(new Response<MobileChungTuModel>
                                {
                                    Message = "Không tìm thấy chứng từ",
                                    Status = true
                                }
                            ,ActionContext
                            
                            
                            );
                    }
                    var trangThaiChungTu = "";
                    if (chungTu.TrangThai == 4) trangThaiChungTu = "Đã xác thực";
                    var data = new Response<MobileChungTuModel>
                    {
                        Message = "Success!",
                        Status = true,
                        Data = new MobileChungTuModel
                        {
                            Barcode = chungTu.MaVach,
                            DonViBanHanh = chungTu.DonViBanHanh,
                            LoaiChungTu = chungTu.MaLoaiChungTu.ToString(),
                            MaChungTu = chungTu.MaChungTu,
                            NgayPheDuyet = chungTu.NgayBanHanh,
                            TrangThai = trangThaiChungTu,
                            Files = chungTu.FileDinhKem != null ? domain + chungTu.FileDinhKem.Replace("~/", "") : ""
                        }
                    };
                    return new ResponseResult(data, ActionContext);
                }
                var ct = _chungtuService.Find(c => (c.MaChungTu.ToUpper().Trim() == Code.ToUpper().Trim() && c.TrangThai == 4)).FirstOrDefault();
                if (ct == null)
                {
                    return new ResponseResult(new Response<MobileChungTuModel>
                    {
                        Message = "Không tìm thấy chứng từ",
                        Status = true
                    }
                           , ActionContext


                           );
                }
                var status = "";
                if (ct.TrangThai == 4) status = "Đã xác thực";
                var dataRes = new Response<MobileChungTuModel>
                {
                    Message = "Success!",
                    Status = true,
                    Data = new MobileChungTuModel
                    {
                        Barcode = ct.MaVach,
                        DonViBanHanh = ct.DonViBanHanh,
                        LoaiChungTu = ct.MaLoaiChungTu.ToString(),
                        MaChungTu = ct.MaChungTu,
                        NgayPheDuyet = ct.NgayBanHanh,
                        TrangThai = status
                    }
                };
                return new ResponseResult(dataRes, ActionContext);
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

        [HttpGet]
        public ResponseResult GetAll()
        {
            try
            {
                var chungTus = _chungtuService.GetAllAsQueryable();
                var data = new Response<List<MobileChungTuModel>>
                {
                    Message = "Success!",
                    Status = true,
                    Data = chungTus.Select(s => new MobileChungTuModel
                    {
                        Barcode =  s.MaVach,
                        MaChungTu = s.MaChungTu
                        
                    }).ToList()

                };
                return new ResponseResult(data, ActionContext);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ActionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                var result = new Response<MobileChungTuModel>
                {
                    Message = HttpMessage.ERROR_GET_DATA,
                    Status = false
                };
                return new ResponseResult(result, ActionContext);
            }
        }

        [HttpPost]
        public ResponseResult ConfirmCheckVoucher(KetQuaKiemTraModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var result = new Response<KetQuaKiemTraModel>
                    {
                        Message = HttpMessage.INVALID_MODEL,
                        Status = false,
                    };
                    ActionContext.Response.StatusCode = HttpStatusCode.BadRequest;
                    return new ResponseResult(result, ActionContext);
                }
                
                var ketQua = new KetQuaKiemTra
                {
                    MaChungTu = model.MaChungTu,
                    CreateDate = DateTime.Now,
                    DiaDiemKT = model.DiaDiem,
                    DonViKT = model.DonViKiemTra, 
                    NgayKT = DateTime.Now,
                    KetQuaKT = model.KetQua,
                    NoiDungKT = model.NoiDungKiemTra, 
                   NguoiKiemTra = model.NguoiKiemTra
                    
                };
                _ketQuaService.Create(ketQua);
                var data = new Response<KetQuaKiemTraModel>
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
                var data = new Response<KetQuaKiemTraModel>
                {
                    Message = HttpMessage.ERROR_CREATE,
                    Status = false
                };
                ActionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                return new ResponseResult(data, ActionContext);
            }
        }

    }
}