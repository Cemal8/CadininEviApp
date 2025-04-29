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
            // Temel WebView Ayarları
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.DomStorageEnabled = true;
            webView.Settings.AllowFileAccess = true;
            webView.Settings.AllowContentAccess = true;
            webView.Settings.SetGeolocationEnabled(true);
            webView.Settings.MediaPlaybackRequiresUserGesture = false;
            webView.Settings.AllowUniversalAccessFromFileURLs = true;
            webView.Settings.AllowFileAccessFromFileURLs = true;
            webView.Settings.MixedContentMode = MixedContentHandling.AlwaysAllow;

            // Donanım Hızlandırma
            webView.SetLayerType(Android.Views.LayerType.Hardware, null);

            // Özel ChromeClient
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


        public class CustomWebChromeClient : WebChromeClient
        {
            public static IValueCallback? FilePathCallback;

            public override void OnPermissionRequest(PermissionRequest request)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var resources = request.GetResources();
                    foreach (var resource in resources)
                    {
                        if (resource == PermissionRequest.ResourceVideoCapture ||
                            resource == PermissionRequest.ResourceAudioCapture ||
                            resource == PermissionRequest.ResourceProtectedMediaId)
                        {
                            request.Grant(resources);
                            return;
                        }
                    }
                    base.OnPermissionRequest(request);
                });
            }

            public override void OnGeolocationPermissionsShowPrompt(string origin, GeolocationPermissions.ICallback callback)
            {
                callback.Invoke(origin, true, false);
            }

            public override bool OnShowFileChooser(
                AWebView webView,
                IValueCallback filePathCallback,
                FileChooserParams fileChooserParams)
            {
                FilePathCallback = filePathCallback;
                var intent = fileChooserParams.CreateIntent();

                try
                {
                    Platform.CurrentActivity?.StartActivityForResult(intent, 1001);
                }
                catch (ActivityNotFoundException)
                {
                    FilePathCallback = null;
                    return false;
                }
                return true;
            }
        }
    }
}