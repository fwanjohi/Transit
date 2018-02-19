
using Xamarin.Forms;

namespace FxITransit.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            var Model = new Creator { Name = "This Guy", Email = "ThisGuy@here.com" };
            this.BindingContext = Model;

       
        }
    }

    public class Creator
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
