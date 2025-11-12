using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CollegeApp
{
    public class AIButton : Button
    {
        Context context;

        public AIButton(Context ctx) : base(ctx)
        {
            context = ctx;
            Text = "AI";
            SetBackgroundColor(Color.ParseColor("#FF5722"));
            TextSize = 20;
            LayoutParameters = new FrameLayout.LayoutParams(200, 200)
            {
                Gravity = GravityFlags.Bottom | GravityFlags.Right,
                RightMargin = 50,
                BottomMargin = 50
            };

            Click += AIButton_Click;
        }

        private void AIButton_Click(object sender, System.EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle("Ask AI a question:");

            EditText input = new EditText(context);
            input.Hint = "Type your question...";
            builder.SetView(input);

            builder.SetPositiveButton("Ask", async (s, args) =>
            {
                string question = input.Text;
                string answer = await GetAIAnswerAsync(question);
                Toast.MakeText(context, answer, ToastLength.Long).Show();
            });

            builder.SetNegativeButton("Cancel", (s, args) => { });

            builder.Show();
        }

        private async Task<string> GetAIAnswerAsync(string question)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Replace with your AI API endpoint
                    string apiUrl = "https://api.openai.com/v1/chat/completions";
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer YOUR_API_KEY_HERE");

                    var payload = new
                    {
                        model = "gpt-3.5-turbo",
                        messages = new[] { new { role = "user", content = question } },
                        temperature = 0.7
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(apiUrl, content);
                    string result = await response.Content.ReadAsStringAsync();

                    dynamic json = JsonConvert.DeserializeObject(result);
                    string answer = json.choices[0].message.content;
                    return answer ?? "Sorry, AI could not respond.";
                }
            }
            catch
            {
                return "Error: Unable to reach AI server.";
            }
        }
    }
}

