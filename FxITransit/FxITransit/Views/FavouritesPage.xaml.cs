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
    public partial class FavouritesPage : ContentPage
    {
        public FavouritesPage()
        {
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var section = FavesSection;
            section.Clear();
            foreach (var stop in OptionsHelper.Instance.Alerts.Stops)
            {

                var content = new SwitchCell();
                content.BindingContext = stop;
                content.Text = stop.Display;

                content.SetBinding(SwitchCell.TextProperty, "FullTitle", BindingMode.TwoWay);
                content.SetBinding(SwitchCell.OnProperty, "IsFavorited", BindingMode.TwoWay);

                section.Add(content);

                var Button = new Button
                {
                    Text = "Delete"
                };
                

            }
        }
    }
}