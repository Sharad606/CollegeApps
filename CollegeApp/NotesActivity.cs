using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using Android.Graphics;
using Android.Views;

namespace CollegeApp
{
    [Activity(Label = "Notes")]
    public class NotesActivity : Activity
    {
        LinearLayout layout;
        ScrollView scroll;
        Button addNote;
        string category;
        bool editable;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            category = Intent.GetStringExtra("Category") ?? "Default";
            editable = Intent.GetBooleanExtra("Editable", true);

            layout = new LinearLayout(this) { Orientation = Orientation.Vertical };
            scroll = new ScrollView(this);
            scroll.AddView(layout);

            addNote = new Button(this) { Text = "+", Enabled = editable };
            addNote.Click += (s, e) => AddNotePage();

            LinearLayout container = new LinearLayout(this) { Orientation = Orientation.Vertical };
            container.AddView(scroll);
            container.AddView(addNote);

            SetContentView(container);

            AddNotePage(); // first page

            // Floating AI circle
            AIButton ai = new AIButton(this);
            container.AddView(ai);
        }

        void AddNotePage()
        {
            var page = new DrawLinesView(this);
            layout.AddView(page);
        }
    }
}
