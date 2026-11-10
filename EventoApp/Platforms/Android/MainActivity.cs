using Android.App;
using Android.Content.PM;

namespace EventoApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    ConfigChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode
        | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
