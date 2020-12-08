using System;
using System.Collections.Generic;

namespace GIS_Upload_Page.Models
{
    public partial class FileProperty
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int? RowCount { get; set; }
        public string Comment { get; set; }

        public virtual File File { get; set; }
    }
}
