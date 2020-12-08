using System;
using System.Collections.Generic;

namespace GIS_Upload_Page.Models
{
    public partial class FileContent
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public byte[] Content { get; set; }

        public virtual File File { get; set; }
    }
}
