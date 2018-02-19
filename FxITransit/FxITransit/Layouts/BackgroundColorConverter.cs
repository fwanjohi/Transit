using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FxITransit.Layouts
{
    public class BackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = (value  as ViewCell);

            if (index % 2 == 0)
                return Color.White;
            else
                return Color.Blue;
        }
    }
}
