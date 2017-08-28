using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.Views;

using Xamarin.Forms;
using FxITransit.Services.NextBus;

namespace FxITransit.ViewModels
{
    public class RoutesViewModel : BaseViewModel
    {
        
        public Command LoadRoutessCommand { get; set; }

        public Agency Agency { get; set; }

        
        public RoutesViewModel( Agency agency)
        {
            Agency = agency;
            Title = "Routes - " + Agency.Title;

            LoadRoutessCommand = new Command(async () => await ExecuteLoadRoutesCommand());
            

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var _item = item as Item;
            //    Items.Add(_item);
            //    await DataStore.AddItemAsync(_item);
            //});
        }

        async Task ExecuteLoadRoutesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var routes = await DataStore.GetRouteList(Agency.Tag);

                Agency.Routes.ReplaceRange(routes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load routes.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}