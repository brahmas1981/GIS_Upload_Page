using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upload_Page.Models
{
    public interface IViewModel
    {
        int ID { get; set; }
        string CreateUserName { get; set; }
        string ModifyUserName { get; set; }
        DateTime CreatedDateTime { get; set; }
        DateTime ModifiedDateTime { get; set; }
    }
}
