using System.Collections.Generic;

using Android.Content;
using Android.Graphics;
using ScoreListPeli.Classes;

namespace ScoreListPeli
{
    public class ObjDrawer : Android.Views.View
    {
        private static string LIVES_TEXT = "LVS ";
        private int LIVES = 3;
        private static string HIGH_SCORE_TEXT = "SCORE ";
        private int HIGH_SCORE = 0;

        private Context mContext;


        private Bitmap heart;
        private static readonly int HEART_ICON_SPACE = 5;

        private FallObject_normal testObject;
        private List<FallObject_normal> FallingObjects;

        public ObjDrawer(Context context) :
            base(context)
        {
            mContext = context;
            //SetBackgroundResource(Resource.Drawable.rainbow_texture679532534);

            //test = new ImageView(context);

            Initialize();
        }

        private void Initialize()
        {
            Android.Content.Res.Resources res = Resources;
            heart = BitmapFactory.DecodeResource(res, Resource.Drawable.Hart);
            heart = ScreenUtils.ScaleBitmap(heart, 30, 30);

            

            FallingObjects = new List<FallObject_normal>();
            testObject = new FallObject_normal(mContext, ScreenUtils.GAME_WIDTH / 2, ScreenUtils.GAME_HEIGHT / 2);
        }

        

        protected override void OnDraw(Canvas canvas)
        {

            int[] bounds = new int[4]; // Used to calculate object bounds.
            RectF Temp = new RectF(); // Used to draw Rectangular objects.
            Coordinate coord; // Used for defining coordinates.
            var paint = new Paint(); // Paint tool.

            coord = ScreenUtils.ConvertCoordinate(new Coordinate(ScreenUtils.GAME_WIDTH / 2, ScreenUtils.GAME_HEIGHT / 2));
            /*
            bounds = ScreenUtils.convertBounds(0, 0, 79, 79);
            test.Layout(bounds[0], bounds[1], bounds[2], bounds[3]);
            test.Draw(canvas);
            */

            /* 
            bounds = ScreenUtils.convertBounds(testObject.Coordinates.x, testObject.Coordinates.y, testObject.Coordinates.x + testObject.Width, testObject.Coordinates.y + testObject.Height);
            testObject.setLayout(bounds);

            testObject.changeViewTest(this, bounds[0], bounds[1]);
            testObject.draw(canvas);
            testObject.start();
            

            bounds = ScreenUtils.convertBounds(testObject.Coordinates.x, testObject.Coordinates.y);
            testObject.changeViewTest(bounds[0], bounds[1]);
            testObject.Draw(canvas);
            */

            /*
             * 
             * DRAW STATIC GAME OBJECTS
             * 
             */
            paint.SetStyle(Paint.Style.FillAndStroke); // Fill and Stroke
            paint.SetARGB(250, 50, 125, 255); // STRANGE_BLUE
            bounds = ScreenUtils.convertBounds(0, ScreenUtils.GAME_HEIGHT - (ScreenUtils.GAME_HEIGHT / 8), ScreenUtils.GAME_WIDTH - 5, ScreenUtils.GAME_HEIGHT);
            Temp.Set(bounds[0], bounds[1], bounds[2], bounds[3]);
            canvas.DrawRect(Temp, paint);


            // Draw bottom oval.
            paint.SetARGB(250, 0, 240, 255); // LIGHT_BLUE
            bounds = ScreenUtils.convertBounds(0, ScreenUtils.GAME_HEIGHT - (ScreenUtils.GAME_HEIGHT / 8), ScreenUtils.GAME_WIDTH - 5, ScreenUtils.GAME_HEIGHT);
            Temp.Set(bounds[0], bounds[1], bounds[2], bounds[3]);
            canvas.DrawOval(Temp, paint);


            

            /*
             * 
             * DRAW GAME HEADERS
             * 
             */

            // Draw header background
            bounds = ScreenUtils.convertBounds(0, 0, ScreenUtils.GAME_WIDTH, ScreenUtils.GAME_HEIGHT / 12);
            Temp.Set(bounds[0], bounds[1], bounds[2], bounds[3]);
            paint.SetShader(new LinearGradient(bounds[0], bounds[1], bounds[2], bounds[3], Color.Argb(255, 0, 115, 255), Color.Argb(255, 0, 230, 255), Shader.TileMode.Repeat));
            canvas.DrawRect(Temp, paint);

            // Reset Paint for text fields.
            paint.Set(new Paint());
            paint.SetStyle(Paint.Style.FillAndStroke); // Stroke only
            paint.StrokeWidth = 2 * ScreenUtils.SCREEN_RATIO; // Stroke size
            paint.SetARGB(255, 0, 60, 140); // DARK_BLUE
            paint.TextSize = ScreenUtils.GAME_TEXT_SIZE * ScreenUtils.SCREEN_H_RATIO; // Set Text size.
            coord = ScreenUtils.ConvertCoordinate(new Classes.Coordinate(ScreenUtils.GAME_WIDTH / 100, ScreenUtils.GAME_TEXT_SIZE)); // LIVES coordinates
            canvas.DrawText(LIVES_TEXT, coord.x, coord.y, paint);

            // DRAW LIVES
            for (int i = 0; i < LIVES; i++)
            {
                // coord (x, y)
                coord = ScreenUtils.ConvertCoordinate(new Classes.Coordinate((LIVES_TEXT.Length * ScreenUtils.GAME_TEXT_SIZE / 2) + HEART_ICON_SPACE * i, ScreenUtils.GAME_TEXT_SIZE / 3.5f)); // LIVES coordinates
                coord.x = coord.x + (heart.Width * i);
                canvas.DrawBitmap(heart, coord.x, coord.y, paint);
            }


            coord = ScreenUtils.ConvertCoordinate(new Classes.Coordinate(ScreenUtils.GAME_WIDTH / 2, ScreenUtils.GAME_TEXT_SIZE)); // HIGH_SCORE coordinates
            canvas.DrawText(HIGH_SCORE_TEXT + HIGH_SCORE.ToString(), coord.x, coord.y, paint);

            /*
            bounds = ScreenUtils.convertBounds(0, 0, ScreenUtils.GAME_HEIGHT / 8, ScreenUtils.GAME_WIDTH);
            Drawable topBar = mContext.GetDrawable(Resource.Drawable.TopBar);
            setConvertedBounds(topBar, bounds[0], bounds[1], bounds[2], bounds[3]);
            topBar.Draw(canvas);
            */

        }
    }
}