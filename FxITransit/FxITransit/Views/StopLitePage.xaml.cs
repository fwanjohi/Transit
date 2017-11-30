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
    public partial class StopLitePage : ContentPage
    {
        private StopLiteViewModel _StopLiteViewModel;
        public StopLitePage(StopLiteViewModel viewModel )
        {
            InitializeComponent();
            _StopLiteViewModel = viewModel;
            BindingContext = viewModel;
        }

        private void BtnOK_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {

        }
    }
}