using System;
using System.Diagnostics;
using System.Threading.Tasks;

using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.Views;

using Xamarin.Forms;
using FxITransit.Services.NextBus;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxITransit.ViewModels
{
    public class AgenciesViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Agency> Agencies { get; set; }

        private string _filter;
        private ObservableRangeCollection<Agency> _filteredAgencies;
        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged("FilteredAgencies");
            }
        }

        public ObservableRangeCollection<Agency> FilteredAgencies
        {
            get
            {
                if (string.IsNullOrEmpty(_filter))
                {
                    _filteredAgencies.ReplaceRange(Agencies);
                }
                else
                {
                    var items = new List<Agency>(Agencies.Where(x => x.Title.ToLower().Contains(_filter.ToLower())));

                    _filteredAgencies.ReplaceRange(items);
                }
                return _filteredAgencies;
            }

        }
        public Command LoadAgenciesCommand { get; set; }

        public AgenciesViewModel() : base()
        {

            Title = "Select Agency";
            Agencies = new ObservableRangeCollection<Agency>();
            _filteredAgencies = new ObservableRangeCollection<Agency>();
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
                var agencies = await TransitService.GetAgencyList();
                Agencies.ReplaceRange(agencies);
                _filteredAgencies.ReplaceRange(Agencies);
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