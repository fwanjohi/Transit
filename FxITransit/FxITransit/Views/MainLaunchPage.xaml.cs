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
            PreferencesHelper.Instance.Preference = DbHelper.Instance.GetPreference();
        }


        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();


        }

        protected override void OnAppearing()
        {

        }
    }
}