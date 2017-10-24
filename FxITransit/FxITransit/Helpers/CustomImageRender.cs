using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Helpers
{
    //public class CustomImageRenderer : ImageRenderer
    //{

    //    private readonly ImageGestureListener listener;
    //    private readonly GestureDetector detector;

    //    private CustomImage customImage;

    //    public CustomImageRenderer()
    //    {
    //        listener = new ImageGestureListener(this);
    //        detector = new GestureDetector(listener);
    //    }

    //    internal void FireLongClick(MotionEvent e)
    //    {
    //        if (customImage != null)
    //            customImage.FireLongPress(new EventArgs());
    //    }

    //    internal void FireSingleClick(EventArgs e)
    //    {
    //        if (customImage != null)
    //            customImage.FireClick(new EventArgs());
    //    }

    //    internal void FireTapUpClick(EventArgs e)
    //    {
    //        if (customImage != null)
    //            customImage.FireTapUp(new EventArgs());
    //    }

    //    internal void FireScroll(EventArgs e)
    //    {
    //        if (customImage != null)
    //            customImage.FireScroll(new EventArgs());
    //    }

    //    protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
    //    {
    //        base.OnElementChanged(e);
    //        customImage = this.Element as CustomImage;

    //        if (e.NewElement == null)
    //        {
    //            this.GenericMotion -= HandleGenericMotion;
    //            this.Touch -= HandleTouch;
    //        }

    //        if (e.OldElement == null)
    //        {
    //            this.GenericMotion += HandleGenericMotion;
    //            this.Touch += HandleTouch;
    //        }
    //    }

    //    void HandleTouch(object sender, TouchEventArgs e)
    //    {
    //        detector.OnTouchEvent(e.Event);
    //    }

    //    void HandleGenericMotion(object sender, GenericMotionEventArgs e)
    //    {
    //        detector.OnTouchEvent(e.Event);
    //    }
    //}
}
