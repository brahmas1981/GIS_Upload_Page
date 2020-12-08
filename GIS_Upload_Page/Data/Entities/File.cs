using System;
using System.Collections.Generic;

namespace GIS_Upload_Page.Models
{
    public partial class File
    {
        public File()
        {
            FileContent = new HashSet<FileContent>();
            FileProperty = new HashSet<FileProperty>();
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public long? FileSize { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public int? ModifyUserId { get; set; }
        public int CreateUserId { get; set; }
        public bool? Deleted { get; set; }

        public virtual User CreateUser { get; set; }
        public virtual User ModifyUser { get; set; }
        public virtual ICollection<FileContent> FileContent { get; set; }
        public virtual ICollection<FileProperty> FileProperty { get; set; }
    }
}
