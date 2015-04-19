using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsAzure.Messaging;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Gcm;
using Hvz.Api;
using Hvz.Api.Response;
using Java.Util.Concurrent.Atomic;
using Xamarin;

namespace Hvz
{
    [BroadcastReceiver(Permission=Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] {Constants.INTENT_FROM_GCM_MESSAGE}, Categories = new string[]{"@PACKAGE_NAME@"})]
    [IntentFilter(new string[] {Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK}, Categories = new string[]{"@PACKAGE_NAME@"})]
    [IntentFilter(new string[] {Constants.INTENT_FROM_GCM_LIBRARY_RETRY}, Categories = new string[]{"@PACKAGE_NAME@"})]
    public class HvzGcmBroadcastReceiver : GcmBroadcastReceiverBase<HvzGcmService>
    {
        public static string[] SenderIds = {"338156487320"};
    }

    [Service]
    public class HvzGcmService : GcmServiceBase
    {
        private static NotificationHub hub;

        public static bool HasProfile { get; set; }

        public static void Initialize(Context context)
        {
            var cs =
                ConnectionString.CreateUsingSharedAccessKeyWithListenAccess(
                    new Java.Net.URI(AppConfig.NotificationHubUri), AppConfig.NotificationHubKey);
            
            hub = new NotificationHub(AppConfig.NotificationHubName, cs, context);
        }

        public static void Register(Context context)
        {
            GcmClient.Register(context, HvzGcmBroadcastReceiver.SenderIds);
        }

        public HvzGcmService()
            : base(HvzGcmBroadcastReceiver.SenderIds)
        {
            
        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            if (hub != null)
            {
                try
                {
                    hub.Register(registrationId, "announcements");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception while registering notifications: " + e.Message);
                }
            }
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            if (hub != null)
            {
                try
                {
                    hub.Unregister();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception while unregistering notifications: " + e.Message);
                }
            }
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            Console.WriteLine("Received notification");

            if (intent != null)
            {
                string message = intent.Extras.GetString("msg");
                if(message == null)
                    Console.WriteLine("Null msg");

                Intent appIntent = new Intent(this, typeof(MainActivity));
                appIntent.PutExtra("notification-message", message);
                PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, appIntent,
                    PendingIntentFlags.OneShot);

                Notification.Builder builder = new Notification.Builder(context)
                    .SetContentTitle("HvZ Announcement")
                    .SetContentText(message)
                    .SetSmallIcon(Resource.Drawable.Icon)
                    .SetDefaults(NotificationDefaults.All)
                    .SetContentIntent(pendingIntent);

                Notification notification = builder.Build();

                NotificationManager notificationManager =
                    GetSystemService(Context.NotificationService) as NotificationManager;

                notificationManager.Notify(0, notification);
            }
        }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            Console.WriteLine("GCM recoverable error: " + errorId);

            return true;
        }

        protected override void OnError(Context context, string errorId)
        {
            Console.WriteLine("GCM error: " + errorId);
        }
    }
}