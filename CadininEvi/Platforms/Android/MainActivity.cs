using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace CadininEvi.Platforms.Android
{
    [Activity(
        Theme = "@style/Maui.SplashTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string[] permissions =
            {
        Manifest.Permission.Camera,
        Manifest.Permission.AccessFineLocation,
        Manifest.Permission.AccessCoarseLocation
    };

            var missingPermissions = permissions
                .Where(p => ContextCompat.CheckSelfPermission(this, p) != Permission.Granted)
                .ToArray();

            if (missingPermissions.Any())
            {
                ActivityCompat.RequestPermissions(this, missingPermissions, 101);
            }
        }

    }
}