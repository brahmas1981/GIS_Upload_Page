using Microsoft.EntityFrameworkCore;
using GIS_Upload_Page.Models;

namespace GIS_Upload_Page.Data
{
    public interface IUploadPageContext
    {
        DbSet<File> File { get; set; }
        DbSet<FileContent> FileContent { get; set; }
        DbSet<FileProperty> FileProperty { get; set; }
        DbSet<User> User { get; set; }
    }
}
