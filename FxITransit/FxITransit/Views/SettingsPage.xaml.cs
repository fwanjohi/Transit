﻿using FxITransit.Helpers;
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
            BindingContext =  StopOptionsHelper.Instance;
        }

        

        private void StopsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            StopOptionsHelper.Instance.SaveSttingsToFile();
        }
    }
}