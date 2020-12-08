using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ICSharpCode.SharpZipLib.Zip;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Text;
using GIS_Upload_Page.Data;
using GIS_Upload_Page.Classes.Parsers;
using GIS_Upload_Page.Models;
using FileEntity = GIS_Upload_Page.Models.File;
using GIS_Upload_Page.Classes.Exceptions;
using Upload_Page.Extensions;
using System.Net.Mail;

namespace GIS_Upload_Page.Controllers
{
     [Authorize]
    public class HomeController : Controller
    {
      //  private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDataService<UploadViewModel> _uploadDataService;
        private readonly IDataService<MasterCcViewModel> _masterCcDataService;
        private IParser<MasterCcViewModel> _masterCcParser;
        private IMemoryCache _cache;
        
        public HomeController(IConfiguration configuration, IDataService<UploadViewModel> uploadDataService, IDataService<MasterCcViewModel> masterCcDataService, IMemoryCache memoryCache, IParser<MasterCcViewModel> masterCcParser)
        {
            _configuration = configuration;
           _uploadDataService = uploadDataService;
            _masterCcParser = masterCcParser;
            _masterCcDataService = masterCcDataService;
            _cache = memoryCache;
        }

           public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult View1()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       

       
        public ActionResult Async_Save(IList<IFormFile> files, string comment)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                   // var physicalPath = Path.Combine(HostingEnvironment.WebRootPath, "App_Data", fileName);

                    // The files are not actually saved in this demo
                    //using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                    //{
                    //    await file.CopyToAsync(fileStream);
                    //}
                }


            }
            //  var sendEmail = _configuration.GetSection("Configs")?.GetSection("Features")?.GetSection("Email")?.GetValue<bool>("Enabled");
           // var sendEmail =
            var uploadedFiles = new string[] { };
            // _masterCcParser = new IParser<MasterCcViewModel>();

            // The Name of the Upload component is "files"
            if (files != null && files.Count() > 0)
            {
                for (var i = 0; i < files.Count(); i++)
                {
                    var file = files[i];
                    var fileName = Path.GetFileName(file.FileName); // GetFileName is necessary for Edge and prob IE
                    var fileSize = file.Length;

                    var lineCout = 0;
                    var fileData = new StringBuilder();
                    var masterCcViewModels = new List<MasterCcViewModel>();
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        using (var fileStream = new MemoryStream())
                        {
                            var s = reader.ReadLine();
                            while (s != null)
                            {
                                bool isHeader = false;
                               var masterCcViewModel = _masterCcParser.Parse(s, out isHeader);
                                if (!isHeader)
                                {
                                  masterCcViewModels.Add(masterCcViewModel);
                                    lineCout++;
                                }
                                fileData.Append(s);
                                fileData.Append(Environment.NewLine);
                                s = reader.ReadLine();
                            }
                        }
                    }

                    using (var stream = GenerateMemoryStreamFromString(fileData.ToString()))
                    {
                        var uploadViewModel = new UploadViewModel()
                        {
                            FileName = fileName,
                            FileSize = fileSize,
                            FileContent = stream.ToArray(),
                            Comment = comment,
                            RowCount = lineCout,
                            
                        };

                       
                        using (var transaction = _uploadDataService.DbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                        {
                            _uploadDataService.Create(uploadViewModel);
                          
                            try
                            {
                                // update master cc records
                                if (masterCcViewModels.Count() > 0)
                                    ((MasterCcDataService)_masterCcDataService).Create(masterCcViewModels);

                                transaction.Commit();

                                //if (sendEmail.HasValue && sendEmail.Value)
                                //{
                                //    try
                                //    {
                                //        // call stored proc to send email
                                //        _masterCcDataService.ExecuteSqlCommand("send_masterCc_html");
                                //    }
                                //    catch (Exception e)
                                //    {
                                //        //throw new UncriticalException("Data was saved, however there was an error sending email.");
                                //    }
                                //}

                                //email send code
                                IConfigurationSection section = _configuration.GetSection("Email");

                                string email_from = section.GetValue<string>("EMail_From");
                                string email_smtp = section.GetValue<string>("EMail_SMTP");

                                MailMessage msg = new MailMessage();
                                msg.From = new MailAddress(email_from);
                                msg.Subject = "Find attached Uploaded files uploaded by you!!";
                                var toAddress = User.Identity.Name;
                                msg.To.Add(toAddress);
                                msg.Body = "GIS Upload Page - new app - Email testing";
                                msg.IsBodyHtml = true;
                                using (var smtp = new SmtpClient(email_smtp))
                                {
                                    smtp.UseDefaultCredentials = true;
                                    smtp.EnableSsl = true;
                                    smtp.Send(msg);
                                }
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            //catch (Exception e)
                            //{
                            //    //((UploadDataService)_uploadDataService).DestroyHard(uploadViewModel);
                            //    transaction.Rollback();
                            //    throw new ShowMessageException(e.InnerException.Message);
                            //}

                            //scope.Complete();
                        }
                    }
                }

                return Ok(new { uploadedFiles });
            }
            // Return an empty string to signify success
            return Content("");
        }
        private static MemoryStream GenerateMemoryStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        public ActionResult Async_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    //var physicalPath = Path.Combine(HostingEnvironment.WebRootPath, "App_Data", fileName);

                    // TODO: Verify user permissions

                    //if (System.IO.File.Exists(physicalPath))
                    //{
                    //    // The files are not actually removed in this demo
                    //    // System.IO.File.Delete(physicalPath);
                    //}
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, bool showDeleted)
        {
            // @@@ debugging
            /*try
            {
                return Json(_dataService.Read().ToDataSourceResult(request));
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }*/

            return Json(((UploadDataService)_uploadDataService).Read(showDeleted).ToDataSourceResult(request));
        }

        [HttpGet]
        public ActionResult DownloadFile(int fileId)
        {
            try
            {
                var file = _uploadDataService.GetValues<FileEntity>().FirstOrDefault(o => o.Id == fileId);
                if (file == null)
                    throw new Exception("Invalid Upload File ID.");

                var fileContents = _uploadDataService.GetValues<FileContent>().Where(o => o.FileId == fileId);

                if (fileContents.Count() > 0)
                {
                    var fileContent = new List<byte>();

                    fileContents.ToList().ForEach(o =>
                    {
                        fileContent.AddRange(o.Content);
                    });

                    return File(fileContent.ToArray(), "application/force-download", file.FileName);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<UploadViewModel> models)
        {
            //var validator = new UploadViewModelValidator();
            //validator.Validate(models, ModelState);

            if (models != null && ModelState.IsValid)
            {
                foreach (var model in models)
                {
                    try
                    {
                        _uploadDataService.Update(model);
                    }
                    catch (ShowMessageException e)
                    {
                        ModelState.AddModelError("upload_show_error, upload_update_error, upload_cancel_changes", e.GetErrorMessageSafe());
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("upload_update_error, upload_cancel_changes", e.GetErrorMessageSafe());
                    }
                }
            }

            return Json(models.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<UploadViewModel> models, bool hardDelete)
        {
            foreach (var model in models)
            {
                try
                {
                    if (hardDelete)
                        ((UploadDataService)_uploadDataService).DestroyHard(model);
                    else
                        _uploadDataService.Destroy(model);
                }
                catch (ShowMessageException e)
                {
                    if (e.InternalCode == 102)
                        ModelState.AddModelError("upload_show_error, upload_del_error", e.GetErrorMessageSafe());
                    else
                        ModelState.AddModelError("upload_show_error, upload_del_error, upload_cancel_changes", e.GetErrorMessageSafe());
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("upload_del_error, upload_cancel_changes", e.GetErrorMessageSafe());
                }
                finally
                {
                    // in case rows without attachment are submitted for deletion, don't want model state error
                    ModelState.Remove("models[0].FileName");
                }
            }

            return Json(models.ToDataSourceResult(request, ModelState));
        }

    }
}
