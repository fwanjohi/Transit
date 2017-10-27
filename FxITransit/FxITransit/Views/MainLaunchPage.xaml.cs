using Acr.UserDialogs;
using FxITransit.Helpers;
using FxITransit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FxITransit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainLaunchPage : TabbedPage
    {
        bool _first = true;
        public MainLaunchPage()
        {

            InitializeComponent();
        }


        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_first)
            {
                Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading("Loading preferences", MaskType.Black));
                Task.Run(async () =>
                {
                     await StopOptionsHelper.Instance.LoadSettingsFromFile();

                    
                }).ContinueWith(result => Device.BeginInvokeOnMainThread(() =>
                {
                    if (StopOptionsHelper.Instance.MySettings.AlertMinsBefore == 0)
                    {
                        StopOptionsHelper.Instance.MySettings.AlertMinsBefore = 5;
                        StopOptionsHelper.Instance.MySettings.AlertInterval = 1;
                        StopOptionsHelper.Instance.MySettings.Speak = true;
                        StopOptionsHelper.Instance.MySettings.Alert = true;


                    }
                    StopOptionsHelper.Instance.MySettings.AutoRefresh = true;

                    if (StopOptionsHelper.Instance.MySettings.FavoriteStops == null)
                    {
                        StopOptionsHelper.Instance.MySettings.FavoriteStops = new ObservableRangeCollection<Stop>();
                    }
                    UserDialogs.Instance.HideLoading();

                }));
            }
            _first = false;


        }
    }
}