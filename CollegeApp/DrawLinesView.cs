using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using Google.MLKit.Vision.DigitalInk;
using Google.MLKit.Vision.TextRecognition;

namespace CollegeApp
{
    public class DrawLinesView : View
    {
        Paint linePaint, inkPaint, shapePaint, textPaint;
        Path inkPath;
        List<Path> strokes;
        List<string> recognizedTexts;
        DigitalInkRecognizer recognizer;
        TextRecognizer textRecognizer;

        public DrawLinesView(Context context) : base(context)
        {
            linePaint = new Paint { Color = Color.Black, StrokeWidth = 2 };
            inkPaint = new Paint { Color = Color.Blue, StrokeWidth = 5, StrokeJoin = Paint.Join.Round, StrokeCap = Paint.Cap.Round };
            shapePaint = new Paint { Color = Color.Red, StrokeWidth = 4 };
            textPaint = new Paint { Color = Color.Black, TextSize = 40 };

            inkPath = new Path();
            strokes = new List<Path>();
            recognizedTexts = new List<string>();

            Focusable = true;
            FocusableInTouchMode = true;

            // ML Kit setup
            var modelId = DigitalInkRecognitionModelIdentifier.Stylus;
            recognizer = DigitalInkRecognition.GetClient(new DigitalInkRecognizerOptions.Builder(modelId).Build());
            textRecognizer = TextRecognition.GetClient();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            float x = e.GetX();
            float y = e.GetY();

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    inkPath.MoveTo(x, y);
                    break;
                case MotionEventActions.Move:
                    inkPath.LineTo(x, y);
                    break;
                case MotionEventActions.Up:
                    strokes.Add(new Path(inkPath));
                    DetectShape(inkPath); // detect shapes
                    RecognizeHandwritingAsync(inkPath); // recognize handwriting
                    inkPath.Reset();
                    break;
            }

            Invalidate();
            return true;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            int height = Height;
            int width = Width;
            int lineHeight = 60;

            // Draw notebook lines
            for (int y = 0; y < height; y += lineHeight)
                canvas.DrawLine(0, y, width, y, linePaint);

            // Draw handwritten strokes
            foreach (var stroke in strokes)
                canvas.DrawPath(stroke, inkPaint);
            canvas.DrawPath(inkPath, inkPaint);

            // Draw recognized text
            int yOffset = 50;
            foreach (var text in recognizedTexts)
            {
                canvas.DrawText(text, 20, yOffset, textPaint);
                yOffset += 60;
            }
        }

        async void RecognizeHandwritingAsync(Path stroke)
        {
            try
            {
                // Convert Path points to Ink object for ML Kit
                var inkBuilder = new Ink.Builder();
                var strokeBuilder = new Ink.Stroke.Builder();

                // Extract points from Path (approximate)
                var pathMeasure = new PathMeasure(stroke, false);
                float[] coords = new float[2];
                float distance = 0;
                while (distance < pathMeasure.Length)
                {
                    pathMeasure.GetPosTan(distance, coords, null);
                    strokeBuilder.AddPoint(new Ink.Point(coords[0], coords[1]));
                    distance += 5; // sample points every 5 pixels
                }

                inkBuilder.AddStroke(strokeBuilder.Build());
                var ink = inkBuilder.Build();

                var result = await recognizer.RecognizeAsync(ink);
                if (result.Candidates.Count > 0)
                {
                    recognizedTexts.Add(result.Candidates[0].Text);
                    PostInvalidate(); // redraw view with recognized text
                }
            }
            catch
            {
                // ignore ML exceptions
            }
        }

        void DetectShape(Path stroke)
        {
            RectF bounds = new RectF();
            stroke.ComputeBounds(bounds, true);

            float w = bounds.Width();
            float h = bounds.Height();
            float ratio = w / h;

            if (ratio > 0.8f && ratio < 1.2f)
            {
                // likely a circle or square
                recognizedTexts.Add("[Shape: Circle/Square]");
            }
            else if (ratio > 3 || ratio < 0.33f)
            {
                recognizedTexts.Add("[Shape: Line]");
            }
            else
            {
                recognizedTexts.Add("[Shape: Unknown]");
            }

            PostInvalidate();
        }
    }
}
