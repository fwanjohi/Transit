﻿using FxITransit.Models;
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
    public partial class AgencyListView : ContentPage
    {
        AgenciesViewModel viewModel;

        public AgencyListView()
        {
            InitializeComponent();

            BindingContext = viewModel = new AgenciesViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var agency = args.SelectedItem as Agency;
            if (agency == null)
                return;

            await Navigation.PushAsync(new RouteListPage(agency));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new NewItemPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Agencies.Count == 0)
                viewModel.LoadAgenciesCommand.Execute(null);
        }
    }
}