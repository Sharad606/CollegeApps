using Android.App;
using Android.OS;
using Android.Content;

namespace CollegeApp
{
    [Activity(Label = "CollegeApp", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var prefs = GetSharedPreferences("CollegeAppPrefs", FileCreationMode.Private);
            bool firstLaunch = prefs.GetBoolean("FirstLaunch", true);

            if (firstLaunch)
            {
                StartActivity(typeof(YesDetailsActivity));
                Finish();
                return;
            }

            StartActivity(typeof(SelectionMenuActivity));
            Finish();
        }
    }
}
