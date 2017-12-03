
using Foundation;
using UIKit;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace FxITransit.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			LoadApplication(new App());
		    AppCenter.Start("f2224e34-b570-4eb0-8a01-7c258a86c72a",
		        typeof(Analytics), typeof(Crashes));

            return base.FinishedLaunching(app, options);
		    
        }
	}
}
