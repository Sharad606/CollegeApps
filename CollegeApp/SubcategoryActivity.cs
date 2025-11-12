using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using System;

namespace CollegeApp
{
    [Activity(Label = "Categories")]
    public class SubcategoryActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var layout = new LinearLayout(this) { Orientation = Orientation.Vertical, Padding = 20 };
            var prefs = GetSharedPreferences("CollegeAppPrefs", FileCreationMode.Private);
            bool goingToCollege = prefs.GetBoolean("GoingToCollege", false);
            int start = prefs.GetInt("CollegeStartHour", 9);
            int end = prefs.GetInt("CollegeEndHour", 17);
            int now = DateTime.Now.Hour;

            string[] categories = { "Classwork", "Homework", "Rough Work" };
            foreach (var cat in categories)
            {
                Button b = new Button(this) { Text = cat, TextSize = 20 };
                b.Click += (s, e) =>
                {
                    bool editable = true;
                    if (cat == "Classwork" && (now >= start && now < end))
                        editable = goingToCollege;
                    else if (cat != "Classwork")
                        editable = true;

                    var intent = new Intent(this, typeof(NotesActivity));
                    intent.PutExtra("Category", cat);
                    intent.PutExtra("Editable", editable);
                    StartActivity(intent);
                };
                layout.AddView(b);
            }

            SetContentView(layout);

            // Focus mode during college hours
            if (now >= start && now < end)
                Toast.MakeText(this, "Focus mode active: cannot exit app during college hours!", ToastLength.Long).Show();
        }
    }
}
