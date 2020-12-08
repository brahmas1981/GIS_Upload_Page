using ICSharpCode.SharpZipLib.Zip;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
//using GIS_Upload_Page.Classes.Exceptions;
//using GIS_Upload_Page.Classes.Parsers;
//using GIS_Upload_Page.Data;
//using GIS_Upload_Page.Data.Entities.UploadPage;
//using GIS_Upload_Page.Extensions;
using GIS_Upload_Page.Models;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using GIS_Upload_Page.Classes.Parsers;
using GIS_Upload_Page.Data;
//using FileEntity = Upload_Page.Data.Entities.UploadPage.File;

namespace GIS_Upload_Page.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        private readonly IDataService<UploadViewModel> _uploadDataService;
        private readonly IDataService<MasterCcViewModel> _masterCcDataService;
        private readonly IConfiguration _configuration;
        private IMemoryCache _cache;
       private IParser<MasterCcViewModel> _masterCcParser;

        public UploadController(IConfiguration configuration, IDataService<UploadViewModel> uploadDataService, IDataService<MasterCcViewModel> masterCcDataService, IMemoryCache memoryCache, IParser<MasterCcViewModel> masterCcParser)
        {
            _configuration = configuration;
            _uploadDataService = uploadDataService;
            _masterCcParser = masterCcParser;
            _masterCcDataService = masterCcDataService;
            //_cache = memoryCache;
        }
        public IActionResult AsyncUpload()
        {
            return View();
        }
        public ActionResult Async_Save(IEnumerable<IFormFile> files)
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
                    var physicalPath = Path.Combine(HostingEnvironment.WebRootPath, "App_Data", fileName);

                    // The files are not actually saved in this demo
                    //using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                    //{
                    //    await file.CopyToAsync(fileStream);
                    //}
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Async_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(HostingEnvironment.WebRootPath, "App_Data", fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        // System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        //public ActionResult Read([DataSourceRequest] DataSourceRequest request, bool showDeleted)
        //{
        //    // @@@ debugging
        //    /*try
        //    {
        //        return Json(_dataService.Read().ToDataSourceResult(request));
        //    }
        //    catch(Exception e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        //    }*/

        //    return Json(((UploadDataService)_uploadDataService).Read(showDeleted).ToDataSourceResult(request));
        //}

        [HttpPost]
        //public ActionResult Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UploadViewModel> models)
        //{
        //    //var validator = new UploadViewModelValidator();
        //    //validator.Validate(models, ModelState);

        //    if (models != null && ModelState.IsValid)
        //    {
        //        foreach (var model in models)
        //        {
        //            try
        //            {
        //                _uploadDataService.Update(model);
        //            }
        //            catch (ShowMessageException e)
        //            {
        //                ModelState.AddModelError("upload_show_error, upload_update_error, upload_cancel_changes", e.GetErrorMessageSafe());
        //            }
        //            catch (Exception e)
        //            {
        //                ModelState.AddModelError("upload_update_error, upload_cancel_changes", e.GetErrorMessageSafe());
        //            }
        //        }
        //    }

        //    return Json(models.ToDataSourceResult(request, ModelState));
        //}

        [HttpPost]
        //public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UploadViewModel> models, bool hardDelete)
        //{
        //    foreach (var model in models)
        //    {
        //        try
        //        {
        //            if (hardDelete)
        //                ((UploadDataService)_uploadDataService).DestroyHard(model);
        //            else
        //                _uploadDataService.Destroy(model);
        //        }
        //        catch (ShowMessageException e)
        //        {
        //            if (e.InternalCode == 102)
        //                ModelState.AddModelError("upload_show_error, upload_del_error", e.GetErrorMessageSafe());
        //            else
        //                ModelState.AddModelError("upload_show_error, upload_del_error, upload_cancel_changes", e.GetErrorMessageSafe());
        //        }
        //        catch (Exception e)
        //        {
        //            ModelState.AddModelError("upload_del_error, upload_cancel_changes", e.GetErrorMessageSafe());
        //        }
        //        finally
        //        {
        //            // in case rows without attachment are submitted for deletion, don't want model state error
        //            ModelState.Remove("models[0].FileName");
        //        }
        //    }

        //    return Json(models.ToDataSourceResult(request, ModelState));
        //}

        public IActionResult UploadFiles(IList<IFormFile> files, string comment)
        {
            //try
            //{
            //    // @@@ debug
            //    //throw new ShowMessageException("Test exception");

            //    var sendEmail = _configuration.GetSection("Configs")?.GetSection("Features")?.GetSection("Email")?.GetValue<bool>("Enabled");

            //    var uploadedFiles = new string[] { };

            //    // The Name of the Upload component is "files"
            //    if (files != null && files.Count() > 0)
            //    {
            //        for (var i = 0; i < files.Count(); i++)
            //        {
            //            var file = files[i];
            //            var fileName = Path.GetFileName(file.FileName); // GetFileName is necessary for Edge and prob IE
            //            var fileSize = file.Length;



            //            var lineCout = 0;
            //            var fileData = new StringBuilder();
            //            var masterCcViewModels = new List<MasterCcViewModel>();

            //            using (var reader = new StreamReader(file.OpenReadStream()))
            //            {
            //                using (var fileStream = new MemoryStream())
            //                {
            //                    var s = reader.ReadLine();
            //                    while (s != null)
            //                    {
            //                        bool isHeader = false;
            //                        var masterCcViewModel = _masterCcParser.Parse(s, out isHeader);
            //                        if (!isHeader)
            //                        {
            //                            masterCcViewModels.Add(masterCcViewModel);
            //                            lineCout++;
            //                        }
            //                        fileData.Append(s);
            //                        fileData.Append(Environment.NewLine);
            //                        s = reader.ReadLine();
            //                    }
            //                }
            //            }

            //            using (var stream = GenerateMemoryStreamFromString(fileData.ToString()))
            //            {
            //                var uploadViewModel = new UploadViewModel()
            //                {
            //                    FileName = fileName,
            //                    FileSize = fileSize,
            //                    FileContent = stream.ToArray(),
            //                    Comment = comment,
            //                    RowCount = lineCout
            //                };

            //                // @@@ distributed transaction not supported in core
            //                //using (var scope = new TransactionScope(
            //                //                   TransactionScopeOption.Required,
            //                //                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            //                using (var transaction = _uploadDataService.DbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            //                {
            //                    _uploadDataService.Create(uploadViewModel);

            //                    try
            //                    {
            //                        // update master cc records
            //                        if (masterCcViewModels.Count() > 0)
            //                            ((MasterCcDataService)_masterCcDataService).Create(masterCcViewModels);

            //                        transaction.Commit();

            //                        if (sendEmail.HasValue && sendEmail.Value)
            //                        {
            //                            try
            //                            {
            //                                // call stored proc to send email
            //                                _masterCcDataService.ExecuteSqlCommand("send_masterCc_html");
            //                            }
            //                            catch (Exception e)
            //                            {
            //                                throw new UncriticalException("Data was saved, however there was an error sending email.");
            //                            }
            //                        }
            //                    }
            //                    catch(UncriticalException e)
            //                    {
            //                        throw e;
            //                    }
            //                    catch (Exception e)
            //                    {
            //                        //((UploadDataService)_uploadDataService).DestroyHard(uploadViewModel);
            //                        transaction.Rollback();
            //                        throw new ShowMessageException(e.InnerException.Message);
            //                    }

            //                    //scope.Complete();
            //                }
            //            }
            //        }

            //        return Ok(new { uploadedFiles });
            //    }
            //}
            //catch (UncriticalException e)
            //{
            //    return StatusCode(e.GetDefStatusCode(), e.Message);
            //}
            //catch (ShowMessageException e)
            //{
            //    return StatusCode(e.GetDefStatusCode(), e.Message);
            //}
            //catch (Exception e)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            //}

            return new EmptyResult();
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

        //[HttpGet]
        //public ActionResult DownloadFile(int fileId)
        // {
        //    try
        //    {
        //        var file = _uploadDataService.GetValues<FileEntity>().FirstOrDefault(o => o.Id == fileId);
        //        if (file == null)
        //            throw new Exception("Invalid Upload File ID.");

        //        var fileContents = _uploadDataService.GetValues<FileContent>().Where(o => o.FileId == fileId);

        //        if (fileContents.Count() > 0)
        //        {
        //            var fileContent = new List<byte>();

        //            fileContents.ToList().ForEach(o =>
        //            {
        //                fileContent.AddRange(o.Content);
        //            });

        //            return File(fileContent.ToArray(), "application/force-download", file.FileName);
        //        }

        //        return StatusCode(StatusCodes.Status404NotFound);
        //    }
        //    catch
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        //[HttpGet]
        //public ActionResult DownloadFiles(int[] fileIds)
        //{
        //    try
        //    {
        //        if (fileIds.Length > 0)
        //        {
        //            var zipFilesList = new List<ZipFileEntry>();
        //            var stringBuffer = new StringBuilder();

        //            var fileContents = _uploadDataService.GetValues<FileContent>().Where(o => fileIds.Contains(o.FileId));

        //            fileContents.ToList().ForEach(fileContent => {

        //                var file = _uploadDataService.GetValues<FileEntity>().FirstOrDefault(o => o.Id == fileContent.FileId);

        //                if (file != null)
        //                {
        //                    if (fileContent != null || fileContent.Content != null || fileContent.Content.Length > 0)
        //                    {
        //                        zipFilesList.Add(new ZipFileEntry() { FileName = file.FileName, Content = Encoding.UTF8.GetBytes(fileContent.Content.ToString()) });
        //                    }
        //                }
        //            });

        //            if (zipFilesList.Count() > 0)
        //                return MakeZip(zipFilesList, fileIds);

        //            return StatusCode(StatusCodes.Status404NotFound);
        //        }

        //        return new EmptyResult();
        //    }
        //    catch
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        //public ActionResult MakeZip(List<ZipFileEntry> zipFileList, int[] ids)
        //{
        //    //    var cacheDurationInMin = _configuration.GetSection("Configs")?.GetSection("Download")?.GetValue<double>("ZipFileCacheDurInMin");

        //    //    byte[] buffer = new byte[4096];
        //    //    var ms = new MemoryStream();

        //    //    using (var zipOutputStream = new ZipOutputStream(ms))
        //    //    {
        //    //        zipOutputStream.IsStreamOwner = false;
        //    //        zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression

        //    //        foreach (ZipFileEntry zipFile in zipFileList)
        //    //        {
        //    //            using (var fs = new MemoryStream(zipFile.Content))    // or any suitable inputstream
        //    //            {
        //    //                var fileName = string.IsNullOrEmpty(zipFile.DirName) ? zipFile.FileName : zipFile.DirName + "\\" + zipFile.FileName;
        //    //                var entry = new ZipEntry(ZipEntry.CleanName(fileName));
        //    //                entry.Size = fs.Length;
        //    //                // Setting the Size provides WinXP built-in extractor compatibility,
        //    //                //  but if not available, you can set zipOutputStream.UseZip64 = UseZip64.Off instead.

        //    //                zipOutputStream.PutNextEntry(entry);

        //    //                int count = fs.Read(buffer, 0, buffer.Length);
        //    //                while (count > 0)
        //    //                {
        //    //                    zipOutputStream.Write(buffer, 0, count);
        //    //                    count = fs.Read(buffer, 0, buffer.Length);
        //    //                    //if (!Response.IsClientConnected) break;

        //    //                    ms.Flush();
        //    //                }
        //    //            }
        //    //        }

        //    //        zipOutputStream.Flush();

        //    //    }
        //    //    ms.Flush();
        //    //    ms.Seek(0, SeekOrigin.Begin);

        //    //    var guid = Guid.NewGuid().ToString();

        //    //    // Set cache options.
        //    //    var cacheEntryOptions = new MemoryCacheEntryOptions()
        //    //        // Keep in cache for this time, reset time if accessed.
        //    //        .SetSlidingExpiration(TimeSpan.FromMinutes(cacheDurationInMin.Value));

        //    //    // Save data in cache.
        //    //    _cache.Set(guid, new ZipFile() { Content = ms.ToArray(), Ids = ids }, cacheEntryOptions);

        //    //    return Ok(new { guid, ids = string.Join(",", ids) });
        //    //    //return File(ms.ToArray(), System.Net.Mime.MediaTypeNames.Application.Zip, downloadFileName);
        //    //}

        //    //[HttpGet]
        //    //public ActionResult DownloadZip(string guid)
        //    //{
        //    //    var download = _configuration.GetSection("Configs")?.GetSection("Download");
        //    //    var downloadFileName = download?.GetValue<string>("DownloadZipName");
        //    //    var downloadTimeStampFormat = download?.GetValue<string>("DownloadTimestampFormat");

        //    //    var zipFile = _cache.Get<ZipFile>(guid);

        //    //    if (zipFile != null)
        //    //    {
        //    //        try
        //    //        {
        //    //            if (downloadFileName.Contains("{0}"))
        //    //                downloadFileName = string.Format(downloadFileName, DateTime.Now.ToString(downloadTimeStampFormat));

        //    //            return File(zipFile.Content, System.Net.Mime.MediaTypeNames.Application.Zip, downloadFileName);
        //    //        }
        //    //        catch (ShowMessageException e)
        //    //        {
        //    //            return StatusCode(e.GetDefStatusCode(), "Something went wrong with the export.");
        //    //        }
        //    //        catch (Exception e)
        //    //        {
        //    //            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        //    //        }
        //    //        finally
        //    //        {
        //    //            _cache.Remove(guid);
        //    //        }
        //    //    }
        //    //    return StatusCode(StatusCodes.Status404NotFound);
        //    //}
        //}

        public class ZipFileEntry
        {
            public string DirName { get; set; }
            public string FileName { get; set; }
            public byte[] Content { get; set; }
        }

        public class ZipFile
        {
            public byte[] Content { get; set; }
            public int[] Ids { get; set; }
        }
        public IHostingEnvironment HostingEnvironment { get; set; }

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index(FileProperty model)
        {
            //if (model.AllowedExtensions == null)
            //{
            //    model = new FileProperty()
            //    {
            //        AllowedExtensions = new string[] { "jpg", "pdf", "docx", "xlsx", "zip" },
            //        IsLimited = false
            //    };
            //}

            return View(model);
        }
        [HttpPost]
        public ActionResult Save(IList<IFormFile> files)
        {

            try
            {
                // @@@ debug
                //throw new ShowMessageException("Test exception");

                var sendEmail = _configuration.GetSection("Configs")?.GetSection("Features")?.GetSection("Email")?.GetValue<bool>("Enabled");

                var uploadedFiles = new string[] { };

                // The Name of the Upload component is "files"
                if (files != null && files.Count() > 0)
                {
                    for (var i = 0; i < files.Count(); i++)
                    {
                        var file = files[i];
                        var fileName = Path.GetFileName(file.FileName); // GetFileName is necessary for Edge and prob IE
                        var fileSize = file.Length;

                        //if (fileSize > Global.MaxContentByteLength)
                        //    throw new ShowMessageException("The file exceeds max allowed length in size.");


                        //logic for reading any file type
                        /*using (var fileStream = new MemoryStream())
                        {
                            file.CopyToAsync(fileStream);
                            //byte[] test = fileStream.ToArray();
                            //var chars = System.Text.Encoding.UTF8.GetString(test).ToCharArray();

                            var model = new UploadViewModel()
                            {
                                FileName = fileName,
                                FileSize = fileSize,
                                FileContent = fileStream.ToArray(),
                                Comment = comment,
                                RowCount = 1
                            };

                            //_dataService.Create(model);
                        }*/


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
                                //Comment = comment,
                                RowCount = lineCout
                            };

                            // @@@ distributed transaction not supported in core
                            //using (var scope = new TransactionScope(
                            //                   TransactionScopeOption.Required,
                            //                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                            using (var transaction = _uploadDataService.DbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                            {
                                _uploadDataService.Create(uploadViewModel);

                                try
                                {
                                    // update master cc records
                                    //if (masterCcViewModels.Count() > 0)
                                    //    ((MasterCcDataService)_masterCcDataService).Create(masterCcViewModels);

                                    transaction.Commit();

                                    if (sendEmail.HasValue && sendEmail.Value)
                                    {
                                        try
                                        {
                                            // call stored proc to send email
                                            _masterCcDataService.ExecuteSqlCommand("send_masterCc_html");
                                        }
                                        catch (Exception e)
                                        {
                                          //  throw new UncriticalException("Data was saved, however there was an error sending email.");
                                        }
                                    }
                                }
                                //catch (UncriticalException e)
                                //{
                                //    throw e;
                                //}
                                catch (Exception e)
                                {
                                    //((UploadDataService)_uploadDataService).DestroyHard(uploadViewModel);
                                    transaction.Rollback();
                                    //throw new ShowMessageException(e.InnerException.Message);
                                }

                                //scope.Complete();
                            }
                        }
                    }

                    return Ok(new { uploadedFiles });
                }
            }
            //catch (UncriticalException e)
            //{
            //    return StatusCode(e.GetDefStatusCode(), e.Message);
            //}
            //catch (ShowMessageException e)
            //{
            //    return StatusCode(e.GetDefStatusCode(), e.Message);
            //}
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return new EmptyResult();
        }

    }
}

