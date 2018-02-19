using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FxITransit.Layouts
{
    public class AlternatingListView : ListView
    {
        public AlternatingListView() : base() { }
        public AlternatingListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
        }

        protected override void SetupContent(Cell content, int index)
        {
            base.SetupContent(content, index);

            var viewCell = content as ViewCell;
            viewCell.View.BackgroundColor = index % 2 == 0 ? Color.Blue : Color.Red;
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (child is ViewCell)
            {
                var cell = child as ViewCell;
                cell.View.BackgroundColor = Color.Beige;
            }
        }
    }
}
