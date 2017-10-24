using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FxITransit.Helpers
{
    public class CustomImage : Image
    {
        public static BindableProperty OnClickProperty =
        BindableProperty.Create("OnClick", typeof(Command), typeof(CustomImage));

        public Command OnClick
        {
            get { return (Command)GetValue(OnClickProperty); }
            set { SetValue(OnClickProperty, value); }
        }

        public CustomImage()
        {
            GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(DisTap) });
        }

        private void DisTap(object sender)
        {
            if (OnClick != null)
            {
                OnClick.Execute(sender);
            }
        }


    }
}
