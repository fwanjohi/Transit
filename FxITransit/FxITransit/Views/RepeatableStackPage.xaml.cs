using System;
using System.Collections.Generic;
using Sample.ViewModels;
using Xamarin.Forms;

namespace Sample.Views
{
    public partial class RepeatableStackPage : ContentPage
    {
        public RepeatableStackPage()
        {
            InitializeComponent();
            BindingContext = new RepeatableStackPageViewModel();
        }
    }
}
