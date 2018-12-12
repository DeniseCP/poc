using System;
using System.Collections.Generic;
using System.Linq;
using ScanbotSDK.Xamarin.Forms;
using Foundation;
using UIKit;

namespace POC.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        const string licenseKey =
             "N8mTvA/k08K8OVGPcunEqWQhRPIqMe" +
             "313quaDG+HkY+pHZ9zNn2G5udkLxxX" +
             "w/QABiUGm7Cd6R7QhDRkeu8uZRmBtQ" +
             "XA/hK9V/pDKlXybHR1iIogLzQCYiMK" +
             "G7a3yc64L7id/tj4Ampx+TjKG1rxge" +
             "1El55vixq/tv7kHRSHmiKYZI7UzG8Y" +
             "8T5YSEzTzUiukas0yXMsPKGgr4IZky" +
             "0cNIqwxrQVDgNjKVWNFJRdi/5wy7Vg" +
             "dW6PHb1zrUCP4NObY18E63P1FdV3v/" +
             "pLuW0PZGlCqfrL1rZEgo64fdzSPFo8" +
             "NtC1iVCdNlqtaXlPE0RvfjLlLtvs8C" +
             "q90ku6/CKNUw==\nU2NhbmJvdFNESw" +
             "pjb20uY29tcGFueW5hbWUuUE9DCjE1" +
             "NDcyNTExOTkKMzI3NjcKMw==\n";

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            SBSDKInitializer.Initialize(app, licenseKey, new SBSDKConfiguration { EnableLogging = true });
            return base.FinishedLaunching(app, options);
        }
    }
}
