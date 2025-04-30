#if ANDROID

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Webkit;
using Android.App;

namespace CadininEvi
{
    public class CustomWebChromeClient : WebChromeClient
    {
        private static IValueCallback _filePathCallback;

        public override bool OnShowFileChooser(Android.Webkit.WebView webView, IValueCallback filePathCallback, FileChooserParams fileChooserParams)
        {
            _filePathCallback = filePathCallback;

            var intent = fileChooserParams.CreateIntent();
            try
            {
                var activity = Platform.CurrentActivity ?? throw new NullReferenceException("Activity is null");
                activity.StartActivityForResult(intent, 1001);
            }
            catch (Exception ex)
            {
                _filePathCallback = null;
                System.Diagnostics.Debug.WriteLine("File chooser error: " + ex.Message);
                return false;
            }

            return true;
        }

        public static void OnFileChooserResult(Intent data)
        {
            if (_filePathCallback == null) return;
            _filePathCallback.OnReceiveValue(WebChromeClient.FileChooserParams.ParseResult((int)Result.Ok, data));
            _filePathCallback = null;
        }

        public override void OnPermissionRequest(PermissionRequest request)
        {
            request.Grant(request.GetResources());
        }
    }
}

#endif
