using System;
using Android.App;
using Android.Runtime;
using Android.Util;
using ScanbotSDK.Xamarin.Forms;

namespace POC.Droid
{
    // It is strongly recommended to add the LargeHeap = true flag in your Application class.
    // Working with images, creating PDFs, etc. are memory intensive tasks. So to prevent OutOfMemoryError, consider adding this flag!
    // For more details see: http://developer.android.com/guide/topics/manifest/application-element.html#largeHeap
    [Application(LargeHeap = true)]
    public class MainApplication : Application
    {
        static string LOG_TAG = typeof(MainApplication).Name;

        // TODO Add your Scanbot SDK license key here.
        // You can test all Scanbot SDK features and develop your app without a license. 
        // However, if you do not specify the license key when initializing the SDK, 
        // it will work in trial mode (trial period of 1 minute). 
        // To get another trial period you have to restart your app.
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

        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        { }

        public override void OnCreate()
        {
            base.OnCreate();

            var configuration = new SBSDKConfiguration
            {
                StorageImageFormat = CameraImageFormat.Png,
                StorageImageQuality = 80,
                EnableLogging = true
            };

            Log.Debug(LOG_TAG, "Initializing Scanbot SDK...");
            SBSDKInitializer.Initialize(this, licenseKey, configuration);
        }
    }
}