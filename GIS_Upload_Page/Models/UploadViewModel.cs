using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GIS_Upload_Page.Models
{
    public class UploadViewModel : BaseViewModel
    {
        [Editable(false)]
        [Display(Name = "Attachment")]
        public string Attachment
        {
            get { return FileName; }
            set { FileName = value; }
        }

        [Editable(false)]
        [Display(Name = "Originator")]
        public string Email { get; set; }

        [Display(Name = "Checked By")]
        public string CheckedBy { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string FileName { get; set; }

        [UIHint("StringMultiLine")]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Editable(false)]
        [Display(Name = "Row Count")]
        public int? RowCount { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,#}")]
        [Display(Name = "File Size")]
        public long? FileSize { get; set; }

        // @@@ varbinary(max) in database => 2^31-1 bytes
        [ScaffoldColumn(false)]
        public byte[] FileContent { get; set; }

        // system overrides
        [Required]
        [Editable(false)] // @@@: warning prevents data from being bound on update
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:" + DateTimeFormat + "}")]
        [Display(Name = "Date Submitted")]
        public new DateTime CreatedDateTime { get; set; }
    }
}
