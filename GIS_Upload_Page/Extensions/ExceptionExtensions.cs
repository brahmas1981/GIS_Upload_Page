using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upload_Page.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetErrorMessageSafe(this Exception e)
        {
            if (e.InnerException != null)
                return e.InnerException.Message;
            else
                return e.Message;
        }
    }
}
