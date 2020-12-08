using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GIS_Upload_Page.Models;

namespace GIS_Upload_Page.Models
{
    public class MasterCcViewModel : BaseViewModel
    {
        [MaxLength(255)]
        [Display(Name = "Source ID")]
        public string SourceID { get; set; }

        // @@@ check
        [MaxLength(255)]
        [Display(Name = "Source Document Name")]
        public string SourceDocumentName { get; set; }

        // @@@ check
        [MaxLength(255)]
        [Display(Name = "Source Document Number")]
        public string SourceDocumentNumber { get; set; }

        [MaxLength(8000)]
        [Display(Name = "Verbatim Statement")]
        public string VerbatimStatement { get; set; }
    }
}
