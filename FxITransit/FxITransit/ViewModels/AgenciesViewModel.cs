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
using Acr.UserDialogs;
using System.Collections.ObjectModel;

namespace FxITransit.ViewModels
{
    public class AgenciesViewModel : BaseViewModel
    {
        public Command LoadAgenciesCommand { get; set; }

        public ObservableRangeCollection<Agency> Agencies { get; set; }

        private string _filter;
        private ObservableCollection<Agency> _filtered;
        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged("Filtered");
            }
        }
        public bool ShowFavoritesOnly
        {
            get => Settings.Preference.FavoriteAgenciesOnly;
            set
            {
                Settings.Preference.FavoriteAgenciesOnly = value;
                Db.SavePrerefence(Settings.Preference);
                OnPropertyChanged("ShowFavoritesOnly");
                OnPropertyChanged("Filtered");
            }
        }

        public ObservableCollection<Agency> Filtered
        {
            get
            {
                var items = new List<Agency>();
                if (string.IsNullOrEmpty(_filter))
                {
                    items = new List<Agency>(Agencies);//(Agency.Routes);
                }
                else
                {
                    items = new List<Agency>(Agencies.Where(x => x.Title.ToLower().Contains(_filter.ToLower())));
                }
                if (ShowFavoritesOnly)
                {
                    items = items.Where(x => x.IsFavorite == true).ToList();
                }
                _filtered = new ObservableCollection<Agency>(items);
                return _filtered;
            }



        }

        public AgenciesViewModel() : base()
        {
            Title = "Select Agency";
            Agencies = new ObservableRangeCollection<Agency>();
            _filtered = new ObservableCollection<Agency>();

            ShowFavoritesOnly = PreferencesHelper.Instance.Preference.FavoriteAgenciesOnly;

            LoadAgenciesCommand = new Command(async () =>
            {
                var agencies = await TransitService.GetAgencyList();
                Agencies.ReplaceRange(agencies);
                _filtered = new ObservableCollection<Agency>(Agencies);
                OnPropertyChanged("Filtered");
            });


        }

        //async Task ExecuteLoadAgenciesCommand()
        //{
        //    if (IsBusy)
        //        return;

        //    IsBusy = true;

        //    try
        //    {
        //        Agencies.Clear();
        //        IEnumerable<Agency> agencies = null;
        //        Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading("Loading Agencies", MaskType.Black));
        //        await Task.Run(async () =>
        //        {
        //            agencies = await TransitService.GetAgencyList();

        //            await Task.Delay(1000 * 10);

        //        }).ContinueWith(result => Device.BeginInvokeOnMainThread(() =>
        //        {

        //            Agencies.ReplaceRange(agencies);

        //            _Filtered.ReplaceRange(Agencies);
        //            OnPropertyChanged("Filtered");

        //            UserDialogs.Instance.HideLoading();

        //        }));

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //        MessagingCenter.Send(new MessagingCenterAlert
        //        {
        //            Title = "Error",
        //            Message = "Unable to load agencies.",
        //            Cancel = "OK"
        //        }, "message");
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}
    }
}