using FxITransit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FxITransit.ViewModels
{
    public class StopsViewModel : BaseViewModel
    {
        public Direction Direction { get; private set; }
        public StopsViewModel(Direction direction)
        {
            Direction = direction;
            Title = "Stops for  : " + Direction.Title;
        }


    }
}
