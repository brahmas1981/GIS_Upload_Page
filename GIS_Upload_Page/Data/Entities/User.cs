using System;
using System.Collections.Generic;

namespace GIS_Upload_Page.Models
{
    public partial class User
    {
        public User()
        {
            FileCreateUser = new HashSet<File>();
            FileModifyUser = new HashSet<File>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        public virtual ICollection<File> FileCreateUser { get; set; }
        public virtual ICollection<File> FileModifyUser { get; set; }
    }
}
