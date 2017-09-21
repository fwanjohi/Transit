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
    public partial class LogsPage : ContentPage
    {
        public LogsPage()
        {
            InitializeComponent();
            BindingContext = TrackingHelper.Instance;
        }

        private void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}