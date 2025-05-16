using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursorLibrary.Exceptions
{
    public class CursorApiException : Exception
    {
        public CursorApiException(string message, Exception inner) : base(message, inner) { }
    }
}
