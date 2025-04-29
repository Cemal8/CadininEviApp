using Microsoft.Extensions.Logging;
#if ANDROID
using CadininEvi.Platforms.Android;
#endif

namespace CadininEvi
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

#if ANDROID
            // WebView Handler'ı sadece Android'de CustomWebViewHandler ile değiştiriyoruz
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<WebView, CustomWebViewHandler>();
            });
#endif

            return builder.Build();
        }
    }
}
