using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using FxITransit.Models;
using FxITransit.ViewModels;

namespace Sample.ViewModels
{
    public class RepeatableStackPageViewModel
    {
        
        public ObservableCollection<Hoge> BoxList{get;}

        public ObservableCollection<Route> RouteList { get; }

        public RepeatableStackPageViewModel()
        {
            BoxList = new ObservableCollection<Hoge>(Shuffle());
            RouteList = new ObservableCollection<Route>
            {
                new Route{Title = "Fuck You"},
                new Route{Title = "You too"}
            };

        }

        List<Hoge> Shuffle()
        {
            var list = new List<Hoge>();

            var rand = new Random();
            for (var i = 0; i < 8; i++) {

                var r = rand.Next(10, 245);
                var g = rand.Next(10, 245);
                var b = rand.Next(10, 245);
                var color = Color.FromRgb(r, g, b);
                var w = rand.Next(30, 100);
                var h = rand.Next(30, 60);

                list.Add(new Hoge {
                    Name = $"#{r:X2}{g:X2}{b:X2}",
                    Color = color,
                    Width = w,
                    Height = h,
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
    }
}
