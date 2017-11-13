using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using FxITransit.Models;
using FxITransit.ViewModels;

namespace Sample.ViewModels
{
    public class WrapLayoutWithSelectorViewModel : BaseViewModel
    {
       
        public WrapLayoutWithSelectorViewModel()
        {
            IsSquare = false;
            UniformColumns = 0;
            Title = "WrapLayout(Variable)";

            Routes = new ObservableCollection<Route>(Shuffle());
        }

        List<Route> Shuffle()
        {
            var list = new List<Route>();

            var rand = new Random();
            for (var i = 0; i < 8; i++) {

                var r = rand.Next(10, 245);
                var g = rand.Next(10, 245);
                var b = rand.Next(10, 245);
                var color = Color.FromRgb(r, g, b);
                var w = rand.Next(30, 100);
                var h = rand.Next(30, 60);

                list.Add(new Route {
                    Title = $"#{r:X2}{g:X2}{b:X2}",
                });
            }

            return list;
        }

        Hoge GetNextItem()
        {
            var rand = new Random();
            var r = rand.Next(10, 245);
            var g = rand.Next(10, 245);
            var b = rand.Next(10, 245);
            var color = Color.FromRgb(r, g, b);
            var w = rand.Next(30, 100);
            var h = rand.Next(30, 60);

            return new Hoge {
                Name = $"#{r:X2}{g:X2}{b:X2}",
                Color = color,
                Width = w,
                Height = h,
            };
        }

        Route GetNextRoute()
        {
            var rand = new Random();
            var r = rand.Next(10, 245);
            var g = rand.Next(10, 245);
            var b = rand.Next(10, 245);
            var color = Color.FromRgb(r, g, b);
            var w = rand.Next(30, 100);
            var h = rand.Next(30, 60);

            return new Route
            {
                Title = $"#{r:X2}{g:X2}{b:X2}",
            };
        }

        private string _Title;
        public string Title {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        private int _UniformColumns;
        public int UniformColumns {
            get { return _UniformColumns; }
            set { SetProperty(ref _UniformColumns, value); }
        }

        private bool _IsSquare;
        public bool IsSquare {
            get { return _IsSquare; }
            set { SetProperty(ref _IsSquare, value); }
        }


        public ObservableCollection<Route> Routes { get; set; }

        private Command _AddCommand;
        public Command AddCommand {
            get {
                return _AddCommand = _AddCommand ?? new Command(() => {
                    Routes.Add(GetNextRoute());
                });
            }
        }

        private Command _ToggleUniCommand;
        public Command ToggleUniCommand {
            get {
                return _ToggleUniCommand = _ToggleUniCommand ?? new Command(() => {
                    if (UniformColumns == 0) {
                        UniformColumns = 3;
                        Title = "WrapLayout(Uniform)";
                    }
                    else {
                        UniformColumns = 0;
                        Title = "WrapLayout(Variable)";
                    }


                });
            }
        }

        private Command _ToggleSquareCommand;
        public Command ToggleSquareCommand {
            get {
                return _ToggleSquareCommand = _ToggleSquareCommand ?? new Command(() => {
                    IsSquare = !IsSquare;
                    if (IsSquare) {
                        Title += "(Square)";
                    }

                });
            }
        }

        private Command _CheckCommand;
        public Command CheckCommand {
            get {
                return _CheckCommand = _CheckCommand ?? new Command(() => {
                    var chk = Routes;
                    var bak = Routes.ToList();

                    Routes.Clear();

                    //Routes = new ObservableCollection<Hoge>(bak);
                    //OnPropertyChanged(() => Routes);
                    foreach (var hoge in bak) {
                        Routes.Add(hoge);
                    }


                });
            }
        }

        private Command _ShuffleCommand;
        public Command ShuffleCommand {
            get {
                return _ShuffleCommand = _ShuffleCommand ?? new Command(() => {

                    Routes.Clear();
                    var list = Shuffle();
                    Routes = new ObservableCollection<Route>(list);
                    OnPropertyChanged("Routes");
                    //foreach (var hoge in list) {
                    //    Routes.Add(hoge);
                    //}

                });
            }
        }

        private Command _ClearCommand;
        public Command ClearCommand {
            get {
                return _ClearCommand = _ClearCommand ?? new Command(() => {
                    Routes.Clear();
                });
            }
        }


        private Command _DeleteCommand;
        public Command DeleteCommand {
            get {
                return _DeleteCommand = _DeleteCommand ?? new Command(() => {
                    Routes.Remove(Routes.Last());
                });
            }
        }

        private Command _ReplaceCommand;
        public Command ReplaceCommand {
            get {
                return _ReplaceCommand = _ReplaceCommand ?? new Command(() => {
                    Routes[0] = GetNextRoute();
                });
            }
        }

        private Command<object> _TapCommand;
        public Command<object> TapCommand {
            get {
                return _TapCommand = _TapCommand ?? new Command<object>((x) => {
                    var item = x as Hoge;
                    //_pageDialog.DisplayAlertAsync("", item.Name, "OK");
                });
            }
        }



    }

}
