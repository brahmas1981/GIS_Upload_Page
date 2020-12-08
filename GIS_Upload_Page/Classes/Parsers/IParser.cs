using GIS_Upload_Page.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIS_Upload_Page.Classes.Parsers
{
    public interface IParser<TViewModel> 
        where TViewModel : BaseViewModel
    {
        TViewModel Parse(string data);
        TViewModel Parse(string data, out bool isHeader);
    }
}
