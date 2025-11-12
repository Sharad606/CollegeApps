using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;

namespace CollegeApp
{
    [Activity(Label = "Setup College Details")]
    public class YesDetailsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            LinearLayout layout = new LinearLayout(this) { Orientation = Orientation.Vertical, Padding = 50 };

            EditText streamInput = new EditText(this) { Hint = "Enter Stream" };
            EditText comboInput = new EditText(this) { Hint = "Enter Combination" };
            EditText startTime = new EditText(this) { Hint = "College Start Hour (0-23)" };
            EditText endTime = new EditText(this) { Hint = "College End Hour (0-23)" };
            Button save = new Button(this) { Text = "Save" };

            layout.AddView(streamInput);
            layout.AddView(comboInput);
            layout.AddView(startTime);
            layout.AddView(endTime);
            layout.AddView(save);

            save.Click += (s, e) =>
            {
                var prefs = GetSharedPreferences("CollegeAppPrefs", FileCreationMode.Private);
                var editor = prefs.Edit();
                editor.PutString("Stream", streamInput.Text);
                editor.PutString("Combination", comboInput.Text);
                editor.PutInt("CollegeStartHour", int.Parse(startTime.Text));
                editor.PutInt("CollegeEndHour", int.Parse(endTime.Text));
                editor.PutBoolean("FirstLaunch", false);
                editor.Apply();

                StartActivity(typeof(SelectionMenuActivity));
                Finish();
            };

            SetContentView(layout);
        }
    }
}
