using FxITransit.Models;
using FxITransit.ViewModels;
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
    public partial class PredictionsPage : ContentPage
    {
        PredictionsViewModel viewModel;
        public PredictionsPage()
        {
            InitializeComponent();
        }

        public PredictionsPage(Stop stop)
        {
            InitializeComponent();
            BindingContext = this.viewModel = new PredictionsViewModel(stop);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Stop != null)
            {
                viewModel.LoadPredictionsCommand.Execute(null);
                viewModel.AutoRefreshPredictionsCommand.Execute(null);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (viewModel.Stop != null)
            {
                viewModel.AutoRefresh = false;
            }

        }
    }
}