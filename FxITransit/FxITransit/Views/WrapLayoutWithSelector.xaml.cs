using Sample.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WrapLayoutWithSelector : ContentPage
    {
        public WrapLayoutWithSelector()
        {
            InitializeComponent();
            BindingContext = new WrapLayoutWithSelectorViewModel();
        }
    }
}
