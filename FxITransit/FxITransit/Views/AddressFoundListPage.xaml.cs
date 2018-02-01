using FxITransit.Models;
using FxITransit.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FxITransit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddressFoundListPage : ContentPage
    {
        private AddressSearchViewModel _model;
        private bool _isDestination;
        public List<GoogleAddress> Addresses { get; set; }

        public AddressFoundListPage(AddressSearchViewModel model, bool isDestination)
        {
            var title = isDestination ? model.ToAddress : model.FromAddress;
            model.SearchTitle  = $"Possible results for {title}";

            InitializeComponent();
            BindingContext = model;

            _model = model;
            _isDestination = isDestination;

        }


        private async void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as GoogleAddress;

            if (item == null) return;

            if (_isDestination)
            {
                _model.SelectedToAddress = item;
            }
            else
            {
                _model.SelectedFromAddress = item;
            }

            _model.IsCanceled = false;
            await Navigation.PopModalAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _model.UpdateProperties();
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            _model.IsCanceled = true;
            await Navigation.PopModalAsync();
        }
    }
}
