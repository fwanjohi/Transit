using System;
using FxITransit.Models;
using FxITransit.ViewModels;
using Xamarin.Forms;
namespace FxITransit.Views
{
    public class TestSelector:DataTemplateSelector
    {
        public DataTemplate TemplateA { get; set; }
        public DataTemplate TemplateB { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return TemplateA;
        }
    }
}
