using FxITransit.Helpers;
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
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext =  PreferencesHelper.Instance.Preference;
        }

        

       

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            PreferencesHelper.Instance.SaveSttingsToFile();
        }
    }
}