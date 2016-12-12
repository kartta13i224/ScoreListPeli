using System.Collections.Generic;

using Android.Content;
using Android.Graphics;
using ScoreListPeli.Classes;
using System;

namespace ScoreListPeli
{
    public class ObjDrawer : Android.Views.View
    {
        private static string LIVES_TEXT = "LVS ";
        public int LIVES = 3;
        private static string HIGH_SCORE_TEXT = "SCORE ";
        private int HIGH_SCORE = 0;

        private Context mContext;

        private Bitmap heart;
        private static readonly int HEART_ICON_SPACE = 5;

        private List<ObjectBounds> object_locations;
        private int counter;
        private int counter_check;
        private int counter_diff;
        private int MAX = 15;
        private int MIN = 8;

        private List<Coordinate> slash;



        public ObjDrawer(Context context) :
            base(context)
        {
            mContext = context;
            //SetBackgroundResource(Resource.Drawable.rainbow_texture679532534);

            Initialize();
        }

        // Initializes stuff.
        private void Initialize()
        {
            // Initialize lists.
            slash = new List<Coordinate>();
            object_locations = new List<ObjectBounds>();

            // Initialize values for creating new objects.
            counter = MIN;
            counter_diff = 0;
            counter_check = ScreenUtils.randonInt(MIN, MAX);

            heart = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Hart);
            heart = ScreenUtils.ScaleBitmap(heart, 30, 30);

        }

        // Generates a falling object in a random location.
        public void generateObject()
        {
            int max_speed = HIGH_SCORE / 10;
            if (max_speed <= 3)
            {
                max_speed = 5;
            }

            ObjectBounds temp = new ObjectBounds(max_speed);
            temp.size = ScreenUtils.randonInt(50,100) * (int)ScreenUtils.SCREEN_RATIO;
            temp.score = 100 / temp.size * 2;
            temp.left = ScreenUtils.randonInt(ScreenUtils.GAME_WIDTH / 10, ScreenUtils.GAME_WIDTH - ScreenUtils.GAME_WIDTH / 10 - temp.size);
            temp.right = temp.left + temp.size;
            temp.top = 0;
            temp.bottom = temp.size;

            object_locations.Add(temp);
            counter_diff++;

            if (MIN >= 1 && counter_diff == 10)
            {
                MAX--;
                MIN--;
            }

            counter_check = ScreenUtils.randonInt(MIN, MAX);
        }

        // Makes falling objects to fall and calls new objects.
        public void fallObject()
        {
            counter++;
            foreach (ObjectBounds bound in object_locations)
            {
                bound.top = bound.top + bound.speed;
                bound.bottom = bound.bottom + bound.speed;
                bound.increaseSpeed();
            }

            if (counter == counter_check)
            {
                counter = 0;
                generateObject();
            }
        }

        // Adds given coordinate to slash list.
        public void addSlashPoint(Coordinate point)
        {
            slash.Add(point);
            Console.Out.Write(" - Pointer location: X:");
            Console.Out.Write(point.x);
            Console.Out.Write(" Y:");
            Console.Out.WriteLine(point.y);
            // If slash is too long, remove at the start.
            if (slash.Count > 120)
            {
                Console.Out.Write("TOO MANY, DELETED FIRST. Capacity: ");
                Console.Out.WriteLine(slash.Count);
                slash.RemoveAt(0);
            }
        }

        // Checks the user's slash area for if it hit any of the falling objects.
        public void checkSlash()
        {
            List<ObjectBounds> deleteObjects = new List<ObjectBounds>();
            foreach (ObjectBounds bound in object_locations)
            {
                Coordinate check = ScreenUtils.ConvertCoordinate(bound.getCoordinate());
                
                foreach (Coordinate slashpoint in slash)
                {
                    // Is within slash y range, uses small fluctuation.
                    if (ScreenUtils.checkPosition(check.x, slashpoint.x, bound.size) && ScreenUtils.checkPosition(check.y, slashpoint.y, bound.size))
                    {
                        HIGH_SCORE = HIGH_SCORE + bound.score; // Increase score.
                        deleteObjects.Add(bound);
                    }
                }
            }

            foreach (ObjectBounds delete in deleteObjects)
            {
                object_locations.Remove(delete);
            }

            slash.Clear(); // Clear user's slash until a new slash is made.
            Console.Out.Write("Slash cleared. Capacity: ");
            Console.Out.WriteLine(slash.Capacity);
        }

        public int getScore()
        {
            return HIGH_SCORE;
        }


        protected override void OnDraw(Canvas canvas)
        {
            int[] bounds = new int[4]; // Used to calculate object bounds.
            RectF Temp = new RectF(); // Used to draw Rectangular objects.
            Coordinate coord; // Used for defining coordinates.
            var paint = new Paint(); // Paint tool.
            var stroke_paint = new Paint(); // Stroke paint tool.
            stroke_paint.SetStyle(Paint.Style.Stroke);
            stroke_paint.StrokeWidth = 2 * ScreenUtils.SCREEN_RATIO; // Stroke size
            stroke_paint.SetARGB(200, 50, 255, 255); // RED

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
             * DRAW FALLING OBJECTS
             * 
             */

            // Draw fall object.
            paint.SetARGB(250, 255, 0, 0); // RED
            List<ObjectBounds> deleteObjects = new List<ObjectBounds>();

            foreach (var bound in object_locations)
            {
                if (bound.top >= ScreenUtils.GAME_HEIGHT)
                {
                    LIVES = LIVES - 1;
                    deleteObjects.Add(bound);
                }

                bounds = ScreenUtils.convertBounds(bound.left, bound.top, bound.right, bound.bottom);
                Temp.Set(bounds[0], bounds[1], bounds[2], bounds[3]);
                canvas.DrawOval(Temp, paint);
                canvas.DrawOval(Temp, stroke_paint);
            }

            
            foreach (ObjectBounds delete in deleteObjects)
            {
                object_locations.Remove(delete);
            }

            // Draw user slash effect.
            int RGB = 210; // SILVER8

            foreach (Coordinate slashPoint in slash)
            {
                paint.SetARGB(200, RGB, RGB, RGB);
                Temp.Set(slashPoint.x, slashPoint.y, slashPoint.x + 5 * ScreenUtils.SCREEN_W_RATIO, slashPoint.y + 5 * ScreenUtils.SCREEN_H_RATIO);
                canvas.DrawOval(Temp, paint);
                if (!(RGB <= 0))
                    RGB--;
            }

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