using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

namespace ScoreListPeli
{
    public class ObjDrawer : Android.Views.View
    {
        
        // A fixed numbers for resizing components for the device's screen.
        private static readonly int GAME_WIDTH = 400;
        private static readonly int GAME_HEIGHT = 800;
        private static readonly int GAME_RATIO = GAME_HEIGHT / GAME_WIDTH;
        private static readonly int GAME_TEXT_SIZE = GAME_WIDTH / 10;

        // Multipliers to resize the components.
        private float SCREEN_W_RATIO = 1;
        private float SCREEN_H_RATIO = 1;
        private float SCREEN_RATIO;

        private int w_PX; // Device screen width in pixels.
        private int h_PX; // Device screen height in pixels.

        private static string LIVES_TEXT = "LVS ";
        private int LIVES = 3;
        private static string HIGH_SCORE_TEXT = "SCORE ";
        private int HIGH_SCORE = 0;

        private Bitmap heart;
        private static readonly int HEART_ICON_SPACE = 5;


        public ObjDrawer(Android.Content.Context context, int wDP, int hDP) :
            base(context)
        {
            //mContext = context;
            SetBackgroundResource(Resource.Drawable.rainbow_texture679532534);
            w_PX = wDP;
            h_PX = hDP;
            Initialize();
        }

        private void Initialize()
        {
            // Initialize screen size ratios.
            SCREEN_H_RATIO = (float)h_PX / (float)GAME_HEIGHT;
            SCREEN_W_RATIO = (float)w_PX / (float)GAME_WIDTH;
            SCREEN_RATIO = SCREEN_H_RATIO / SCREEN_W_RATIO;
            Android.Content.Res.Resources res = Resources;
            heart = BitmapFactory.DecodeResource(res, Resource.Drawable.Hart);
            heart = ScaleBitmap(heart, 30, 30);
        }


        private Bitmap ScaleBitmap(Bitmap temp, int newWidth, int newHeight)
        {
            float width = temp.Width;
            float height = temp.Height;

            float scaleWidth = (newWidth * SCREEN_W_RATIO) / width;
            float scaleHeight = (newHeight * SCREEN_H_RATIO) / height;

            Matrix matrix = new Matrix();

            matrix.PostScale(scaleWidth, scaleHeight);

            Bitmap resizedBitmap = Bitmap.CreateBitmap(temp, 0, 0, (int)width, (int)height, matrix, false);

            temp.Recycle();
            return resizedBitmap;
        }

        // Converts Coordinates for display screen size.
        private Classes.Coordinate ConvertCoordinate(Classes.Coordinate input)
        {
            input.x = input.x * SCREEN_W_RATIO;
            input.y = input.y * SCREEN_H_RATIO;

            return input;
        }

        // Converts bounds for display screen size.
        private int[] convertBounds(float left, float top, float right, float bottom)
        {
            int[] bounds = new int[4];
            bounds[0] = (int)(left * SCREEN_W_RATIO); // LEFT
            bounds[1] = (int)(top * SCREEN_H_RATIO); // TOP
            bounds[2] = (int)(right * SCREEN_W_RATIO); // RIGHT
            bounds[3] = (int)(bottom * SCREEN_H_RATIO); // BOTTOM

            return bounds;
        }


        protected override void OnDraw (Canvas canvas)
        {
            int[] bounds = new int[4]; // Used to calculate object bounds.
            RectF Temp = new RectF(); // Used to draw Rectangular objects.
            Classes.Coordinate coord; // Used for defining coordinates.
            var paint = new Paint(); // Paint tool.
 
            
            /*
             * 
             * DRAW STATIC GAME OBJECTS
             * 
             */
            paint.SetStyle(Paint.Style.FillAndStroke); // Fill and Stroke
            paint.SetARGB(250, 50, 125, 255); // STRANGE_BLUE
            bounds = convertBounds(0, GAME_HEIGHT - (GAME_HEIGHT / 8), GAME_WIDTH - 5, GAME_HEIGHT);
            Temp.Set(bounds[0], bounds[1], bounds[2], bounds[3]);
            canvas.DrawRect(Temp, paint);


            // Draw bottom oval.
            paint.SetARGB(250, 0, 240, 255); // LIGHT_BLUE
            bounds = convertBounds(0, GAME_HEIGHT - (GAME_HEIGHT / 8), GAME_WIDTH-5, GAME_HEIGHT);
            Temp.Set(bounds[0], bounds[1], bounds[2], bounds[3]);
            canvas.DrawOval(Temp,paint);




            /*
             * 
             * DRAW GAME HEADERS
             * 
             */

            // Draw header background
            bounds = convertBounds(0, 0, GAME_WIDTH, GAME_HEIGHT / 12);
            Temp.Set(bounds[0], bounds[1], bounds[2], bounds[3]);
            paint.SetShader(new LinearGradient(bounds[0], bounds[1], bounds[2], bounds[3], Color.Argb(255, 0, 115, 255), Color.Argb(255, 0, 230, 255), Shader.TileMode.Repeat));
            canvas.DrawRect(Temp, paint);

            // Reset Paint for text fields.
            paint.Set(new Paint());
            paint.SetStyle(Paint.Style.FillAndStroke); // Stroke only
            paint.StrokeWidth = 2 * SCREEN_RATIO; // Stroke size
            paint.SetARGB(255, 0, 60, 140); // DARK_BLUE
            paint.TextSize = GAME_TEXT_SIZE * SCREEN_H_RATIO; // Set Text size.
            coord = ConvertCoordinate(new Classes.Coordinate(GAME_WIDTH / 100, GAME_TEXT_SIZE)); // LIVES coordinates
            canvas.DrawText(LIVES_TEXT, coord.x, coord.y, paint);
            
            // DRAW LIVES
            for (int i = 0; i < LIVES; i++)
            {
                                                                   // coord (x, y)
                coord = ConvertCoordinate(new Classes.Coordinate((LIVES_TEXT.Length * GAME_TEXT_SIZE / 2) + HEART_ICON_SPACE * i, GAME_TEXT_SIZE / 3.5f)); // LIVES coordinates
                coord.x = coord.x + (heart.Width * i);
                canvas.DrawBitmap(heart, coord.x, coord.y, paint);
            }
                

            coord = ConvertCoordinate(new Classes.Coordinate(GAME_WIDTH / 2, GAME_TEXT_SIZE)); // HIGH_SCORE coordinates
            canvas.DrawText(HIGH_SCORE_TEXT + HIGH_SCORE.ToString(), coord.x, coord.y, paint);

            /*
            bounds = convertBounds(0, 0, GAME_HEIGHT / 8, GAME_WIDTH);
            Drawable topBar = mContext.GetDrawable(Resource.Drawable.TopBar);
            setConvertedBounds(topBar, bounds[0], bounds[1], bounds[2], bounds[3]);
            topBar.Draw(canvas);
            */

        }
    }
}