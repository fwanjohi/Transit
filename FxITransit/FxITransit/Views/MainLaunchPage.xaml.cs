using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FxITransit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainLaunchPage : TabbedPage
    {
        public MainLaunchPage()
        {
            InitializeComponent();
            
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            var cur = this.CurrentPage;

        }
    }
}