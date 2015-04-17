using System;
using System.Collections.Generic;
using System.Linq;
using WindowsAzure.Messaging;
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

            var settings =
                UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Sound | UIUserNotificationType.Alert | UIUserNotificationType.Badge, null);
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();

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

        public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
        {
            application.RegisterForRemoteNotifications();
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var cs =
                SBConnectionString.CreateListenAccess(new NSUrl(AppConfig.NotificationHubUri),
                    AppConfig.NotificationHubKey);

            var hub = new SBNotificationHub(cs, AppConfig.NotificationHubName);
            hub.RegisterNativeAsync(deviceToken, new NSSet("announcements"), err =>
            {
                if (err != null)
                    InvokeOnMainThread(() =>
                    {
                        new UIAlertView("Error registering push notifications", err.LocalizedDescription, null, "OK", null).Show();
                    });
                else
                {
                    Console.WriteLine("Registered for notifications");
                }
            });
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            var aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;
            if (aps != null)
            {
                var alert = aps.ObjectForKey(new NSString("alert")) as NSString;
                if (alert != null)
                    new UIAlertView("HvZ Announcement", alert.ToString(), null, "OK", null).Show();
            }
        }
    }
}