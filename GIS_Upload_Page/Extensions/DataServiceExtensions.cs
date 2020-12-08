using GIS_Upload_Page.Models;

namespace GIS_Upload_Page.Extensions
{
    public static class DataServiceExtensions
    {
        public static string NullIfUserName(this User user)
        {
            return user?.UserName;
        }

        public static string NullIfFullName(this User user)
        {
            return user?.FullName;
        }

        public static int? NullIfRowCount(this FileProperty fileProperty)
        {
            return fileProperty == null ? 0 : fileProperty.RowCount;
        }

        public static string NullIfComment(this FileProperty fileProperty)
        {
            return fileProperty == null ? null : fileProperty.Comment;
        }
    }
}
