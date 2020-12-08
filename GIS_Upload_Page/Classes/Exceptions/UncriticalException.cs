using Microsoft.AspNetCore.Http;
using System;

namespace GIS_Upload_Page.Classes.Exceptions
{
    public class UncriticalException : Exception
    {
        public UncriticalException(string message) : base(message)
        {
        }

        public UncriticalException(int internalCode, string message) : this(message)
        {
            InternalCode = internalCode;
        }

        public int InternalCode { get; set; }

        public int GetDefStatusCode()
        {
            return InternalStatusCodes.UncriticalStatusCode;
        }
    }
}
