using Microsoft.AspNetCore.Http;
using System;

namespace GIS_Upload_Page.Classes.Exceptions
{
    public class ShowMessageException : Exception
    {
        public ShowMessageException(string message) : base(message)
        {
        }

        public ShowMessageException(int internalCode, string message) : this(message)
        {
            InternalCode = internalCode;
        }

        public int InternalCode { get; set; }

        public int GetDefStatusCode()
        {
            return InternalStatusCodes.ShowMessageStatusCode;
        }
    }
}
