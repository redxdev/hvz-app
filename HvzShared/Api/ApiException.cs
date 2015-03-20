using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hvz.Api
{
    [Serializable]
    public class ApiException : Exception
    {
        public ApiException()
            : base()
        {

        }

        public ApiException(string message)
            : base(message)
        {
            
        }

        public ApiException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}