using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GIS_Upload_Page.Models
{
    public class BaseViewModel
    {
        protected const string DateTimeFormat = "MM/dd/yyyy HH:mm:ss";
        protected const string DateFormat = "MM/dd/yyyy";

        [Required]
        //[ScaffoldColumn(false)]
        //[Editable(false)]  // @@@: prevents default data from being bound
        [UIHint("IntegerReadonly")]
        [Display(Name = "ID")]
        public int ID
        {
            get;
            set;
        }

        [Editable(false)]
        [Display(Name = "Created By ID")]
        public int CreateUserId
        {
            get;
            set;
        }

        [Editable(false)]
        [Display(Name = "Created By")]
        public string CreateUserName
        {
            get;
            set;
        }

        [Editable(false)]
        [Display(Name = "Created By")]
        public string CreateFullName
        {
            get;
            set;
        }

        [Editable(false)]
        [Display(Name = "Modified By")]
        public string ModifyUserName
        {
            get;
            set;
        }

        [Required]
        [Editable(false)] // @@@: warning prevents data from being bound on update
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:"+ DateTimeFormat + "}")]
        [Display(Name = "Created DateTime")]
        public DateTime CreatedDateTime
        {
            get;
            set;
        }

        [Required]
        [Editable(false)]
        [DataType(DataType.DateTime)]
        [Display(Name = "Modified DateTime")]
        public DateTime ModifiedDateTime
        {
            get;
            set;
        }

        [Editable(false)]
        public bool? Deleted { get; set; }
    }
}
