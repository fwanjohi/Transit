using Acr.UserDialogs;
using FxITransit.Helpers;
using FxITransit.Services.NextBus;
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
    public partial class LogsPage : ContentPage
    {
        public LogsPage()
        {
            InitializeComponent();
            BindingContext = UtilsHelper.Instance;
        }

        private void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void BtnClear_Clicked(object sender, EventArgs e)
        {
            var config = new ConfirmConfig
            {
                OkText = "Yes Remove All",
                CancelText = "No",
                Message = "Refresh All Route Data",
                OnAction = (yes) =>
                {
                    if (yes)
                    {
                        NextBusService.Instance.RefreshDatabase();
                    }

                }

            };
            UserDialogs.Instance.Confirm(config);



        }
    }
}