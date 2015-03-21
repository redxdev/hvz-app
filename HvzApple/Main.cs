using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Xamarin;

namespace Hvz
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            Insights.Initialize(AppConfig.InsightsApiKey);
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}