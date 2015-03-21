﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Hvz.Api;
using Hvz.Api.Response;
using UIKit;
using Xamarin;

namespace Hvz
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window
        {
            get;
            set;
        }

        public HvzClient ApiClient { get; private set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            ApiClient = new HvzClient();
            ApiClient.ApiKey = NSUserDefaults.StandardUserDefaults.StringForKey("apikey") ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(ApiClient.ApiKey))
            {
                ApiClient.GetProfile(response =>
                {
                    if (response.Status == ApiResponse.ResponseStatus.Ok)
                    {
                        Insights.Identify(ApiClient.ApiKey, new Dictionary<string, string>
                        {
                            {Insights.Traits.Name, response.Profile.FullName},
                            {Insights.Traits.Email, response.Profile.Email},
                            {"Team", response.Profile.Team.ToString()}
                        });
                    }
                });
            }

            return true;
        }

        //
        // This method is invoked when the application is about to move from active to inactive state.
        //
        // OpenGL applications should use this method to pause.
        //
        public override void OnResignActivation(UIApplication application)
        {
        }

        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
        }

        // This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground(UIApplication application)
        {
        }

        // This method is called when the application is about to terminate. Save data, if needed. 
        public override void WillTerminate(UIApplication application)
        {
        }
    }
}