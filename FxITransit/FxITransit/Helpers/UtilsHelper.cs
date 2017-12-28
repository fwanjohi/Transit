using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Plugin.TextToSpeech;
using FxITransit.Models;
using PCLStorage;
using Plugin.Notifications;
using Acr.Settings;

namespace FxITransit.Helpers
{
    public  class UtilsHelper : ObservableObject
    {
        private static readonly Lazy<UtilsHelper> _instance = new Lazy<UtilsHelper>(() => new UtilsHelper());

        public static UtilsHelper Instance { get { return _instance.Value; } }


        private UtilsHelper()
        {
            Logs = new ObservableRangeCollection<LogItem>();
        }
        public ObservableRangeCollection<LogItem> Logs { get; set; }


        public  DateTime? ConvertUnixTimeStamp(string unixTimeStamp)
        {
            var UTC = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Convert.ToDouble(unixTimeStamp));
            var date = UTC.ToLocalTime();
            return date;
        }
        public  async Task<bool> CheckPermissions(Permission permission)
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            bool request = false;
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {

                    var title = $"{permission} Permission";
                    var question = $"To use this plugin the {permission} permission is required. Please go into Settings and turn on {permission} for the app.";
                    var positive = "Settings";
                    var negative = "Maybe Later";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }

                    return false;
                }

                request = true;

            }

            if (request || permissionStatus != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                if (newStatus.ContainsKey(permission) && newStatus[permission] != PermissionStatus.Granted)
                {
                    var title = $"{permission} Permission";
                    var question = $"To use the plugin the {permission} permission is required.";
                    var positive = "Settings";
                    var negative = "Maybe Later";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }
                    return false;
                }
            }

            return true;
        }

        public  void Speak(string text)
        {
            CrossTextToSpeech.Current.Speak(text);
        }

        //public void TellThem(string message)
        //{
        //    var vehicle = pred.Vehicle ?? "";

        //    var msg = $"Your transit vehicle {vehicle} is arriving in {diff} Minutes";
        //    SendNotification(msg);

        //    if (PreferencesHelper.Instance.Prefence.Speak)
        //    {
        //        Speak(msg);
        //    }

        //    if (Settings.Preference.Vibrate)
        //    {

        //    }
        //}

        public void SendNotification(string message, string title = "Alert")
        {
            try
            {
                CrossNotifications.Current.Send(new Notification
                {
                    Title = title,
                    Message = message,
                    Vibrate = true,
                    When = TimeSpan.FromSeconds(2)
                });
            }
            catch (Exception ex)
            {
                UtilsHelper.Instance.Log("Send Notification Failed : " + ex.Message);

                try
                {
                    UtilsHelper.Instance.Speak(ex.Message);
                }
                catch (Exception ex2)
                {
                    UtilsHelper.Instance.Log(ex2.Message);
                }
            }
        }

        public void Log(string message)
        {
            var msg = new LogItem { Message = $"{DateTime.Now} : {message }" };
            Log(msg);
        }

        public void Log(LogItem item)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Logs.Insert(0,item);
            });
            
        }

       
    }
}
