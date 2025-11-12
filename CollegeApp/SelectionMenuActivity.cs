using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using System;

namespace CollegeApp
{
    [Activity(Label = "College Menu")]
    public class SelectionMenuActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LinearLayout layout = new LinearLayout(this) { Orientation = Orientation.Vertical, Padding = 50 };
            TextView question = new TextView(this) { Text = "Are you going to college today?", TextSize = 24 };

            Button yesBtn = new Button(this) { Text = "Yes" };
            Button noBtn = new Button(this) { Text = "No" };

            yesBtn.Click += (s, e) =>
            {
                StartActivity(typeof(SubcategoryActivity));
                Finish();
                var prefs = GetSharedPreferences("CollegeAppPrefs", FileCreationMode.Private);
                prefs.Edit().PutBoolean("GoingToCollege", true).Apply();
            };

            noBtn.Click += (s, e) =>
            {
                StartActivity(typeof(SubcategoryActivity));
                Finish();
                var prefs = GetSharedPreferences("CollegeAppPrefs", FileCreationMode.Private);
                prefs.Edit().PutBoolean("GoingToCollege", false).Apply();
            };

            layout.AddView(question);
            layout.AddView(yesBtn);
            layout.AddView(noBtn);

            SetContentView(layout);
        }
    }
}
