using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using GIS_Upload_Page.Extensions;
using GIS_Upload_Page.Classes.Constants;
using GIS_Upload_Page.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace GIS_Upload_Page.Data
{
  
    public class UploadDataService : BaseDataService, IDataService<UploadViewModel>
    {
        
        public UploadDataService(Upload_PageContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
    
        public IEnumerable<UploadViewModel> GetAll(bool showDeleted = false)
        {
            var uploadPageDbContext = _dbContext as Upload_PageContext;
            var user = FindUser();

            var query = uploadPageDbContext.File.AsQueryable();

            var qresult = query
                .Include(a => a.FileProperty)
                .Include(a => a.CreateUser)
                .Include(a => a.ModifyUser)
                .Select(model =>
                    new UploadViewModel
                    {
                        ID = model.Id,
                        FileName = model.FileName,
                        FileSize = model.FileSize,
                        RowCount = model.FileProperty.FirstOrDefault().NullIfRowCount(),
                        Comment = model.FileProperty.FirstOrDefault().NullIfComment(),
                        Email = model.CreateUser.Email,

                        // System
                        CreateUserId = model.CreateUserId,
                        CreateUserName = model.CreateUser.NullIfUserName(),
                        CreateFullName = model.CreateUser.NullIfFullName(),
                        ModifyUserName = model.ModifyUser.NullIfUserName(),
                        CreatedDateTime = model.CreatedDateTime.ToDisplayDateTime(),
                        ModifiedDateTime = model.ModifiedDateTime.ToDisplayDateTime(),
                        Deleted = model.Deleted,
                    }
          ).Where(o => o.Email == user.Email); ;
          
            if (!showDeleted)
                qresult = qresult.Where(o => o.Deleted == null || o.Deleted == false);

            return qresult;
        }

        public IEnumerable<UploadViewModel> Read(bool showDeleted = false)
        {
            return GetAll(showDeleted: showDeleted);
        }

        public void Create(UploadViewModel model)
        {
            var uploadPageDbContext = _dbContext as Upload_PageContext;
            var user = FindUser();
            var dateTimeUtcNow = DateTime.UtcNow;

            var entity = new File();

            entity.FileName = model.FileName;
            entity.FileSize = model.FileSize;
            // Calculated
            entity.CreateUserId = user.Id;
            entity.CreatedDateTime = entity.ModifiedDateTime = dateTimeUtcNow;
            // One-to-one
            {
                var property = new FileProperty()
                {
                    File = entity,
                    RowCount = model.RowCount,
                    Comment = model.Comment
                };
                uploadPageDbContext.FileProperty.Add(property);
                uploadPageDbContext.Entry(property).State = EntityState.Added;
            }
            // One-to-many
            {
                var currentIndex = 0d;
                while(currentIndex < model.FileContent.Length)
                {
                    var endIndex = currentIndex + Global.MaxContentByteLength - 1;
                    endIndex = endIndex > model.FileContent.Length - 1 ? model.FileContent.Length - 1 : endIndex;
                    var content = new FileContent()
                    {
                        File = entity,
                        Content = model.FileContent.SubArray(currentIndex, endIndex) 
                    };
                    uploadPageDbContext.FileContent.Add(content);
                    uploadPageDbContext.Entry(content).State = EntityState.Added;
                    currentIndex += Global.MaxContentByteLength;
                }
            }

            uploadPageDbContext.File.Add(entity);
            uploadPageDbContext.Entry(entity).State = EntityState.Added;

            uploadPageDbContext.SaveChanges();

            
            //if (templateName == "GetList")
            //{
            //    MocListViewModel model = _emailDataService.GetMocList() as MocListViewModel;

            //    msg.Subject = "MOC List";
            //    var toAddress = model.ToAddress;
            //    msg.To.Add(toAddress);
            //    string view = "~/EmailTemplates/" + templateName;
            //    var htmlBody = await RenderPartialViewToString($"{view}.cshtml", model);

            //    msg.Body = htmlBody;
            //    msg.IsBodyHtml = true;

            //    using (var smtp = new SmtpClient(email_smtp))
            //    {
            //        smtp.UseDefaultCredentials = true;
            //        smtp.EnableSsl = true;
            //        await smtp.SendMailAsync(msg);
            //    }
            //}
            model.ID = (int)entity.Id;
        }

        public void Update(UploadViewModel model)
        {
            var uploadPageDbContext = _dbContext as Upload_PageContext;
            var user = FindUser();
            var dateTimeUtcNow = DateTime.UtcNow;

            var entity = uploadPageDbContext.File
                .FirstOrDefault(o => o.Id == model.ID);
            if (entity == null) return;

            uploadPageDbContext.File.Attach(entity);

            if (entity.FileName != model.FileName) { entity.FileName = model.FileName; }
            if (entity.FileSize != model.FileSize) { entity.FileSize = model.FileSize; }

            // System
            entity.ModifyUserId = user.Id;
            entity.ModifiedDateTime = dateTimeUtcNow;

            // One-to-one
            {
                var property = uploadPageDbContext.FileProperty.FirstOrDefault(o => o.FileId == model.ID);
                if (property != null)
                {
                    if (property.Comment != model.Comment) { property.Comment = model.Comment; }
                }
            }

            uploadPageDbContext.SaveChanges();
        }

        public void Destroy(UploadViewModel model)
        {
            var uploadPageDbContext = _dbContext as Upload_PageContext;

            var entity = uploadPageDbContext.File
                .FirstOrDefault(o => o.Id == model.ID);
            if (entity == null) return;

            var user = FindUser();
            var dateTimeUtcNow = DateTime.UtcNow;
            entity.Deleted = true;
            entity.ModifyUserId = user.Id;
            entity.ModifiedDateTime = dateTimeUtcNow;

            uploadPageDbContext.SaveChanges();
        }

        public void DestroyHard(UploadViewModel model)
        {
            var uploadPageDbContext = _dbContext as Upload_PageContext;

            var entity = uploadPageDbContext.File
                .Include(o => o.FileProperty)
                .Include(o => o.FileContent)
                .FirstOrDefault(o => o.Id == model.ID);
            if (entity == null) return;

            // One-to-one
            {
                if (entity.FileProperty.FirstOrDefault() != null)
                {
                    var fileProperty = entity.FileProperty.First();
                    entity.FileProperty.Remove(fileProperty);
                    uploadPageDbContext.Entry(fileProperty).State = EntityState.Deleted;
                }
            }

            // One-to-many
            {
                entity.FileContent.ToList().ForEach(o =>
                {
                    entity.FileContent.Remove(o);
                    uploadPageDbContext.Entry(o).State = EntityState.Deleted;
                });
            }

            uploadPageDbContext.File.Remove(entity);
            uploadPageDbContext.Entry(entity).State = EntityState.Deleted;

            uploadPageDbContext.SaveChanges();
        }
    }
}
