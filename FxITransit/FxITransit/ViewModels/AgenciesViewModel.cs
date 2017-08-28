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
    public class AgenciesViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Agency> Agencies { get; set; }
        public Command LoadAgenciesCommand { get; set; }

        public AgenciesViewModel() : base()
        {
            Title = "Select Agency";
            Agencies = new ObservableRangeCollection<Agency>();
            LoadAgenciesCommand = new Command(async () => await ExecuteLoadAgenciesCommand());
            

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var _item = item as Item;
            //    Items.Add(_item);
            //    await DataStore.AddItemAsync(_item);
            //});
        }

        async Task ExecuteLoadAgenciesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Agencies.Clear();
                var agencies = await DataStore.GetAgencyList();
                Agencies.ReplaceRange(agencies);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load agencies.",
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