using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Facebook.CoreKit;
using Firebase.CloudMessaging;
using Foundation;
using Newtonsoft.Json;
using UIKit;
using UserNotifications;
using Utility = Aquatrols.iOS.Helper.Utility;

namespace Aquatrols.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate,IMessagingDelegate
    {
        private Utility utility = Utility.GetInstance;
        // class-level declarations
        public override UIWindow Window
        {
            get;
            set;
        }
        /// <summary>
        /// Finisheds the launching.
        /// </summary>
        /// <returns><c>true</c>, if launching was finisheded, <c>false</c> otherwise.</returns>
        /// <param name="application">Application.</param>
        /// <param name="launchOptions">Launch options.</param>
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
             // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            FCMCheck();
          //  Check version in applestore and current install version in device
            try
            {
                bool isNetworkReachable = false;
                if (!Reachability.IsHostReachable(AppResources.NetworkUrl)) // check the internet connection 
                {
                    isNetworkReachable = false;
                }
                else
                {
                    isNetworkReachable = true;
                }
                if (isNetworkReachable)
                {
                    string url = AppResources.itunesUrl;
                    var appStoreAppVersion = "";
                    using (var webClient = new System.Net.WebClient())
                    {
                        string jsonString = webClient.DownloadString(string.Format(url));

                        var lookup = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

                        if (lookup != null
                            && lookup.Count >= 1
                            && lookup[AppResources.resultCount] != null
                            && Convert.ToInt32(lookup[AppResources.resultCount].ToString()) > 0)
                        {

                            var results = JsonConvert.DeserializeObject<List<object>>(lookup[@"results"].ToString());

                            if (results != null && results.Count > 0)
                            {
                                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(results[0].ToString());
                                appStoreAppVersion = values.ContainsKey("version") ? values["version"].ToString() : string.Empty;
                            }
                        }
                    }
                    var currentVersion = NSBundle.MainBundle.InfoDictionary[AppResources.AppVersion].ToString(); // get the current version installed in device 
                    if (!string.IsNullOrEmpty(currentVersion))
                    {
                        NSUserDefaults.StandardUserDefaults.SetString(currentVersion, AppResources.CurrentVersion);// store the value nsuser defaults in current version
                    }
                    if (!string.IsNullOrEmpty(appStoreAppVersion))
                    {
                        NSUserDefaults.StandardUserDefaults.SetString(appStoreAppVersion, AppResources.AppStoreAppVersion); // store the value nsuser defaults in app store version
                    }
                    // if version is different store and device then show message and click the button redirect to user in store.
                    if ((!string.IsNullOrEmpty(appStoreAppVersion)) && (!string.IsNullOrEmpty(currentVersion)))
                    {
                       if (Convert.ToDouble(appStoreAppVersion) > Convert.ToDouble(currentVersion))
                       {
                          DisplayAlert(AppResources.VersionTitle, AppResources.VersionMessage); // display alert 
                       }
                   }
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.FinishedLaunching, AppResources.AppDelegate, null); // exception handling
            }
            return true;
        }
        /// <summary>
        /// method to show the alert message.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        public void DisplayAlert(string title, string message)
        {
            try
            {
                UIAlertView alert = new UIAlertView();
                alert.Title = title;
                alert.AddButton(AppResources.Update);
                alert.Message = message;
                alert.Clicked += (object s, UIButtonEventArgs ev) => {
                    // handle click event here
                    //In your app you can just open it:
                    var nsurl = new NSUrl(AppResources.StoreUrl);
                    UIApplication.SharedApplication.OpenUrl(nsurl);
                };
                alert.Show();
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.DisplayAlert, AppResources.AppDelegate, null);
            }
        }
        /// <summary>
        /// Ons the resign activation.
        /// </summary>
        /// <param name="application">Application.</param>
        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }
        /// <summary>
        /// Wills the enter foreground.
        /// </summary>
        /// <param name="application">Application.</param>
        public override void WillEnterForeground(UIApplication application)
        {
            // if version is different store and device then show message and click the button redirect to user in store.
            try
            {
                var currentVer = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CurrentVersion);
                var StoreVersion = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.AppStoreAppVersion);
                if ((!string.IsNullOrEmpty(StoreVersion)) && (!string.IsNullOrEmpty(currentVer)))
                {
                    if (Convert.ToDouble(StoreVersion) > Convert.ToDouble(currentVer))
                    {
                        DisplayAlert(AppResources.VersionTitle, AppResources.VersionMessage);
                    }
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.WillEnterForeground, AppResources.AppDelegate, null);
            }
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }
        /// <summary>
        /// Ons the activated.
        /// </summary>
        /// <param name="application">Application.</param>
        public override void OnActivated(UIApplication application)
        {
            //base.OnActivated(application);
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
            // base.OnActivated(application);
            ConnectFCM();// Connect the FCM
           
            AppEvents.ActivateApp();
            // Restart any tasks that were paused (or not yet started) while the application was inactive.
            // If the application was previously in the background, optionally refresh the user interface.
        }
       
        /// <summary>
        /// Continue the user activity.
        /// if user hit the weburl in mail then redirect to this page in the application mention in weburl url.
        /// </summary>
        /// <returns><c>true</c>, if user activity was continued, <c>false</c> otherwise.</returns>
        /// <param name="application">Application.</param>
        /// <param name="userActivity">User activity.</param>
        /// <param name="completionHandler">Completion handler.</param>
        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            // return base.ContinueUserActivity(application, userActivity, completionHandler);
            string nSUrl = userActivity.WebPageUrl.ToString();
            if(nSUrl.Contains(AppResources.EOP))
            {
                NSUserDefaults.StandardUserDefaults.SetString(AppResources.EOP, AppResources.LinkData);
                UINavigationController navigationController = (UINavigationController)this.Window.RootViewController;
                LoginController Controller = navigationController.Storyboard.InstantiateViewController(AppResources.LoginController) as LoginController;
                navigationController.PushViewController(Controller, false);
            }
            else if(nSUrl.Contains(AppResources.Redeem))
            {
                NSUserDefaults.StandardUserDefaults.SetString(AppResources.Redeem, AppResources.LinkData); 
                UINavigationController navigationController = (UINavigationController)this.Window.RootViewController;
                LoginController Controller = navigationController.Storyboard.InstantiateViewController(AppResources.LoginController) as LoginController;
                navigationController.PushViewController(Controller, false);
            }
            else if(nSUrl.Contains(AppResources.Home))
            {
                NSUserDefaults.StandardUserDefaults.SetString(AppResources.Home, AppResources.LinkData);
                UINavigationController navigationController = (UINavigationController)this.Window.RootViewController;
                LoginController Controller = navigationController.Storyboard.InstantiateViewController(AppResources.LoginController) as LoginController;
                navigationController.PushViewController(Controller, false);  
            }
            else if (nSUrl.Contains(AppResources.Profile))
            {
                NSUserDefaults.StandardUserDefaults.SetString(AppResources.Profile, AppResources.LinkData);
                UINavigationController navigationController = (UINavigationController)this.Window.RootViewController;
                LoginController Controller = navigationController.Storyboard.InstantiateViewController(AppResources.LoginController) as LoginController;
                navigationController.PushViewController(Controller, false);
            }
            else if (nSUrl.Contains(AppResources.Product))
            {
                NSUserDefaults.StandardUserDefaults.SetString(AppResources.Product, AppResources.LinkData);
                UINavigationController navigationController = (UINavigationController)this.Window.RootViewController;
                LoginController Controller = navigationController.Storyboard.InstantiateViewController(AppResources.LoginController) as LoginController;
                navigationController.PushViewController(Controller, false);
            }
            else if (nSUrl.Contains("Signup"))
            {
                NSUserDefaults.StandardUserDefaults.SetString("Signup", AppResources.LinkData);
                UINavigationController navigationController = (UINavigationController)this.Window.RootViewController;
                LoginController Controller = navigationController.Storyboard.InstantiateViewController(AppResources.LoginController) as LoginController;
                navigationController.PushViewController(Controller, false);
            }
            return true;
        }
        /// <summary>
        /// Opens the URL.
        /// </summary>
        /// <returns><c>true</c>, if URL was opened, <c>false</c> otherwise.</returns>
        /// <param name="app">App.</param>
        /// <param name="url">URL.</param>
        /// <param name="options">Options.</param>
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return base.OpenUrl(app, url, options);
        }
        /// <summary>
        /// Opens the URL.
        /// </summary>
        /// <returns><c>true</c>, if URL was opened, <c>false</c> otherwise.</returns>
        /// <param name="application">Application.</param>
        /// <param name="url">URL.</param>
        /// <param name="sourceApplication">Source application.</param>
        /// <param name="annotation">Annotation.</param>
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation);
        }
        /// <summary>
        /// Wills the terminate.
        /// </summary>
        /// <param name="application">Application.</param>
        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
        /// <summary>
        /// FCMC the check.
        /// </summary>
        public void FCMCheck()
        {
            //Firebase.Analytics.An.Configure();
            Firebase.Core.App.Configure();
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Console.WriteLine(granted);
                });
                // For iOS 10 display notification (sent via APNS)
                //  UNUserNotificationCenter.Current.Delegate = this;
            }
            else
            {
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            Messaging.SharedInstance.Delegate = this;
        }
        /// <summary>
        /// Dids the enter background.
        /// </summary>
        /// <param name="application">Application.</param>
        #pragma warning disable
        public override void DidEnterBackground(UIApplication application)
        {
            Messaging.SharedInstance.Disconnect();
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }
        /// <summary>
        /// method to pass the device token
        /// </summary>
        /// <param name="application"></param>
        /// <param name="deviceToken"></param>
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            string token = NSUserDefaults.StandardUserDefaults.StringForKey("FCMIOSToken");
            Messaging.SharedInstance.ApnsToken = deviceToken;
            // Firebase.InstanceID.InstanceId.SharedInstance.StApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Sandbox);
            string DeviceToken = deviceToken.ToString().Replace("<", string.Empty).Replace(">", string.Empty).Replace(" ", string.Empty);
            //var token = Messaging.SharedInstance.FcmToken ?? "";
            //Console.WriteLine($"FCM token: {token}");
        }
        /// <summary>
        /// Dids the receive registration token.
        /// </summary>
        /// <param name="messaging">Messaging.</param>
        /// <param name="fcmToken">Fcm token.</param>
        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            Console.WriteLine($"Firebase registration token: {fcmToken}");
            NSUserDefaults.StandardUserDefaults.SetString(fcmToken, AppResources.DeviceToken);
            // TODO: If necessary send token to application server.
            // Note: This callback is fired at each app startup and whenever a new token is generated.
        }
        // iOS 10, fire when recieve notification foreground
        /// <summary>
        /// WillPresentNotification fires when recieve notification foreground for ios 10 or above devices
        /// </summary>
        /// <param name="center"></param>
        /// <param name="notification"></param>
        /// <param name="completionHandler"></param>
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            var title = notification.Request.Content.Title;
            var body = notification.Request.Content.Body;
            debugAlert(title, body);
        }
        /// <summary>
        /// method to show the push notification alert on dialog box
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        private void debugAlert(string title, string message)
        {
            UIAlertView alert = new UIAlertView();
            alert.Title = title;
            alert.AddButton(AppResources.OK);
            alert.Message = message;
            alert.Show();
       
        }
        /// <summary>
        /// Connects the fcm.
        /// </summary>
        #pragma warning disable
        private void ConnectFCM()
        {
            try
            {
                Messaging.SharedInstance.Connect((error) =>
                {
                    if (error == null)
                    {
                        //TODO: Change Topic to what is required
                        Messaging.SharedInstance.Subscribe(AppResources.topics);
                    }
                    System.Diagnostics.Debug.WriteLine(error != null ? AppResources.errorOccured : AppResources.connectSuccess);
                });
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message.ToString());
                utility.SaveExceptionHandling(ex, AppResources.ConnectFCM, AppResources.AppDelegate, null);
            }
        }
        /// <summary>
        /// receive remote notification.
        /// show the pop up to the user 
        /// </summary>
        /// <param name="application">Application.</param>
        /// <param name="userInfo">User info.</param>
        /// <param name="completionHandler">Completion handler.</param>
        #pragma warning disable
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            try
            {
                NSDictionary aps = userInfo.ObjectForKey(new NSString(AppResources.aps)) as NSDictionary;
                string title = string.Empty;
                string alert = string.Empty;
                if (aps.ContainsKey(new NSString(AppResources.alert)))
                {
                    title = aps[new NSString(AppResources.alert)].ValueForKey(new NSString(AppResources.title)).ToString();
                    alert = aps[new NSString(AppResources.alert)].ValueForKey(new NSString(AppResources.body)).ToString();
                }
                // show alert
                if (!string.IsNullOrEmpty(alert))
                {
                    UIAlertView avAlert = new UIAlertView(title, alert, null,AppResources.OK, null);
                    avAlert.Show();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}

