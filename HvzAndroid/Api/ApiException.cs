using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

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