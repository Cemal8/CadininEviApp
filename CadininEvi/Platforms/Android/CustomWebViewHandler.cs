using Android.Content;
using Android.Webkit;
using Microsoft.Maui.Handlers;

using AWebView = Android.Webkit.WebView;

namespace CadininEvi
{
    public class CustomWebViewHandler : WebViewHandler
    {
        protected override AWebView CreatePlatformView()
        {
            var webView = base.CreatePlatformView();
            ConfigureWebView(webView);
            return webView;
        }

        protected override void ConnectHandler(AWebView platformView)
        {
            base.ConnectHandler(platformView);
            RequestPermissions(); // Kamera + konum izinlerini iste
        }

        private void ConfigureWebView(AWebView webView)
        {
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.DomStorageEnabled = true;
            webView.Settings.AllowFileAccess = true;
            webView.Settings.AllowContentAccess = true;
            webView.Settings.SetGeolocationEnabled(true);
            webView.Settings.MediaPlaybackRequiresUserGesture = false;
            webView.Settings.AllowUniversalAccessFromFileURLs = true;
            webView.Settings.AllowFileAccessFromFileURLs = true;
            webView.Settings.MixedContentMode = MixedContentHandling.AlwaysAllow;

            webView.SetLayerType(Android.Views.LayerType.Hardware, null);
            webView.SetWebChromeClient(new CustomWebChromeClient());
        }

        private async void RequestPermissions()
        {
            var cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
            var locationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (cameraStatus != PermissionStatus.Granted)
            {
                System.Diagnostics.Debug.WriteLine("Kamera izni reddedildi!");
            }

            if (locationStatus != PermissionStatus.Granted)
            {
                System.Diagnostics.Debug.WriteLine("Konum izni reddedildi!");
            }
        }
    }
}
