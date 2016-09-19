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
using System.Web;
using System.IO;
using Bytescout.BarCode;
using Newtonsoft.Json;

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
                string domain;
                var folderPath = "";
                var fileName = "";
                var myuri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                var pathQuery = myuri.PathAndQuery;
                var appUrl = HttpRuntime.AppDomainAppVirtualPath;
                domain = myuri.ToString().Replace(pathQuery, "") + appUrl.Trim();

                var lst = _chungtuService.GetByFilter(key,maChungTu, maLoaiChungTu,donViBanHanh,trangThai, start, limit);

                var chungTus = lst.Select(c => new ViewChungTuModel
                {
                    Id = c.Id,
                    DonViBanHanh = c.DonViBanHanh,
                    MaChungTu = c.MaChungTu,
                    MaLoaiChungTu = c.MaLoaiChungTu,
                    MaVach = c.MaVach,
                    TenLoaiChungTu = c.TenLoaiChungTu,
                    Ten = c.Ten,
                    TrangThai = c.TrangThai,
                    NguoiKy = c.NguoiKy,
                    MaDonVi = c.MaDonVi,
                    NgayBanHanh = c.NgayBanHanh,
                    FileDinhKem = c.FileDinhKem !=null ? domain + c.FileDinhKem.Replace("~/", ""):""
                }).ToList();
                var data = new Response<List<ViewChungTuModel>>
                {
                    Status = true,
                    Data = chungTus,
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
        public ResponseResult Create()
        {
            try
            {
                string domain;
                var folderPath = "";
                var fileName = "";
                var myuri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                var pathQuery = myuri.PathAndQuery;
                var appUrl = HttpRuntime.AppDomainAppVirtualPath;
                domain = myuri.ToString().Replace(pathQuery, "")+ appUrl.Trim();
                logger.Trace("domain: ", domain.ToLower());
               
                var httpRequest = HttpContext.Current.Request;
                var strJson = httpRequest.Form["data"].Trim();
                var model = JsonConvert.DeserializeObject<ChungTuModel>(strJson);

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

                var postedFile = httpRequest.Files["file"];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    var MaxContentLength = 1024 * 1024 * 10; //Size = 10 MB
                    IList<string> allowedFileExtensions = new List<string> {".pdf" };
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var extension = ext.ToLower();
                    if (!allowedFileExtensions.Contains(extension))
                    {
                        logger.Info("Extend  file not .pdf .jpg,.gif,.png");
                        var dataReturnError = new Response<ChungTuModel>
                        {
                            Message = HttpMessage.ERROR_IMG_TYPE,
                            Status = false
                        };
                        ActionContext.Response.StatusCode = HttpStatusCode.Redirect;
                        return new ResponseResult(dataReturnError, ActionContext);
                    }
                    if (postedFile.ContentLength > MaxContentLength)
                    {
                        logger.Info("Size file > 10mb");
                        var dataCheckSize = new Response<ChungTuModel>
                        {
                            Message = HttpMessage.ERROR_IMG_SIZE_5,
                            Status = false
                        };
                        ActionContext.Response.StatusCode = HttpStatusCode.LengthRequired;
                        return new ResponseResult(dataCheckSize, ActionContext);
                    }
                    folderPath = @"~/Uploads/" + model.MaChungTu.Replace("/","").ToString().Trim();
                    fileName = "CT_" + model.MaChungTu.Replace("/", "").ToString()+ extension;
                    var folderExists = Directory.Exists(HttpContext.Current.Request.MapPath(folderPath));
                    if (!folderExists)
                        Directory.CreateDirectory(HttpContext.Current.Request.MapPath(folderPath));

                    var pathUrl = Path.Combine(
                        HttpContext.Current.Request.MapPath(folderPath),
                        fileName
                        );
                    postedFile.SaveAs(pathUrl);
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
                    TrangThai = 1,
                    FileDinhKem= folderPath+"/"+fileName

                };
                _chungtuService.Create(chungtu);
                model.FileDinhKem = domain + folderPath.Replace("~/", "") + "/" + fileName;
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
        public ResponseResult Edit()
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
                
                var httpRequest = HttpContext.Current.Request;
                var strJson = httpRequest.Form["data"].Trim();
                var model = JsonConvert.DeserializeObject<ChungTuModel>(strJson);
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

                var postedFile = httpRequest.Files["file"];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    var MaxContentLength = 1024 * 1024 * 10; //Size = 10 MB
                    IList<string> allowedFileExtensions = new List<string> { ".pdf" };
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var extension = ext.ToLower();
                    if (!allowedFileExtensions.Contains(extension))
                    {
                        logger.Info("Extend  file not .pdf .jpg,.gif,.png");
                        var dataReturnError = new Response<ChungTuModel>
                        {
                            Message = HttpMessage.ERROR_IMG_TYPE,
                            Status = false
                        };
                        ActionContext.Response.StatusCode = HttpStatusCode.Redirect;
                        return new ResponseResult(dataReturnError, ActionContext);
                    }
                    if (postedFile.ContentLength > MaxContentLength)
                    {
                        logger.Info("Size file > 10mb");
                        var dataCheckSize = new Response<ChungTuModel>
                        {
                            Message = HttpMessage.ERROR_IMG_SIZE_5,
                            Status = false
                        };
                        ActionContext.Response.StatusCode = HttpStatusCode.LengthRequired;
                        return new ResponseResult(dataCheckSize, ActionContext);
                    }
                    folderPath = @"~/Uploads/" + model.MaChungTu.Replace("/", "").ToString().Trim();
                    fileName = "CT_" + model.MaChungTu.Replace("/", "").ToString() + extension;
                    var folderExists = Directory.Exists(HttpContext.Current.Request.MapPath(folderPath));
                    if (!folderExists)
                        Directory.CreateDirectory(HttpContext.Current.Request.MapPath(folderPath));

                    var pathUrl = Path.Combine(
                        HttpContext.Current.Request.MapPath(folderPath),
                        fileName
                        );
                    postedFile.SaveAs(pathUrl);
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
                if (!string.IsNullOrEmpty(fileName))
                {

                    ct.FileDinhKem = folderPath.Replace("~/", "") + "/" + fileName;
                    logger.Trace("Trace UrlSource: " + ct.FileDinhKem);
                }
                _chungtuService.Update(ct);
                model.FileDinhKem = domain + ct.FileDinhKem;
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
        public ResponseResult UpdateTrangThai(ChungTuModel model)
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
                ct.TrangThai = model.TrangThai;
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
        public ResponseResult Signature(ChungTuModel model)
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
                string currentDir = HttpContext.Current.Server.MapPath("~");
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
                // Create new barcode and register it.
                Bytescout.BarCode.Barcode barcode = new Bytescout.BarCode.Barcode();
                barcode.RegistrationName = "demo";
                barcode.RegistrationKey = "demo";
                
                // Set symbology
                barcode.Symbology = SymbologyType.Code93;
                // Set value
                barcode.Value = model.Id.ToString();
                // Place barcode at bottom-right corner of every document page
                folderPath = @"/Uploads/" + model.Id.ToString().Trim();
                fileName = "CTResult_" + model.Id + ".pdf";
                var inputFile = currentDir + ct.FileDinhKem.Replace("~/","");
                var outPutFile = currentDir+ folderPath + "/" + fileName;
                barcode.DrawToPDF(inputFile, -1, 150, 130, outPutFile);
                ct.TrangThai = model.TrangThai;
                ct.MaVach =  model.Id.ToString();
                ct.FileDinhKem ="~"+ folderPath +"/"+ fileName;
                _chungtuService.Update(ct);
                model.FileDinhKem = domain + ct.FileDinhKem.Replace("~/", "");
                var data = new Response<ChungTuModel>
                {
                    Message = "Sign Success",
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
