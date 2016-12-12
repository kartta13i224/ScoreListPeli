using System.Collections.Generic;

using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
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
        private int MAX = 20;
        private int MIN = 10;

        // Variables for slash graphics.
        private List<Coordinate> slash;
        private int[] slashLine_Bounds = {0,0,0,0};
        private Coordinate slash_startPoint;
        private Coordinate slash_endPoint;
        private GradientDrawable.Orientation orientation;
        private int[] slash_color = { Color.Gray, Color.DarkGray };
        private bool slash_Done = false;
        private bool slash_drawn = true;

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
                max_speed = 5;

            ObjectBounds temp = new ObjectBounds(max_speed); // Random speed.

            object_locations.Add(temp);
            counter_diff++;

            if (MIN >= 5 && counter_diff >= 9)
            {
                MAX--;
                MIN--;
                counter_diff = 0;
            }

            counter_check = ScreenUtils.randonInt(MIN, MAX);
        }

        // Makes falling objects to fall and calls new objects.
        public void fallObject()
        {
            counter++;
            foreach (ObjectBounds bound in object_locations)
            {
                bound.bounce();
                bound.increaseSpeed();
            }

            if (counter >= counter_check)
            {
                counter = 0;
                generateObject();
            }
        }

        // Adds given coordinate to slash list.
        public void addSlashPoint(Coordinate point)
        {
            slash.Add(point);
            Console.Out.Write("Slash length: ");
            Console.Out.WriteLine(slash.Count);
        }

        // Starts slash.
        public void addStartPoint(Coordinate point)
        {
            slash_drawn = false;
            slash_startPoint = point;
            slashLine_Bounds[0] = (int)point.x;
            slashLine_Bounds[1] = (int)point.y;
            
        }

        // Changes end point.
        public void addEndPoint(Coordinate point)
        {
            slash_endPoint = point;
            slashLine_Bounds[2] = (int)point.x;
            slashLine_Bounds[3] = (int)point.y;
        }


        // Checks the user's slash area for if it hit any of the falling objects.
        public void checkSlash()
        {
            float x_start = slash_startPoint.x;
            float y_start = slash_startPoint.y;
            float x_end = slash_endPoint.x;
            float y_end = slash_endPoint.y;

            float x_diff = x_end - x_start;
            float y_diff = y_end - y_start;
            int steps = 1;

            if (Math.Abs(x_diff) > Math.Abs(y_diff))
                steps = Math.Abs((int)x_diff);
            else
                steps = Math.Abs((int)y_diff);

            float x_stepValue = x_diff / steps;
            float y_stepValue = y_diff / steps;

            // Set default values.
            slashLine_Bounds[0] = (int)x_start;
            slashLine_Bounds[1] = (int)y_start;
            slashLine_Bounds[2] = (int)x_end;
            slashLine_Bounds[3] = (int)y_end;

         

            /*
            if (x_diff > y_diff)
            {
                // From right to left
                if (x_start > x_end)
                    orientation = GradientDrawable.Orientation.RightLeft;

                // Fromt left to right
                else
                    orientation = GradientDrawable.Orientation.LeftRight;
            }
            else
            {
                // From bottom to top
                if (y_start > y_end)
                    orientation = GradientDrawable.Orientation.BottomTop;

                // Fromt top to bottom
                else
                    orientation = GradientDrawable.Orientation.TopBottom;
            }
            */

            addSlashPoint(new Coordinate(x_start, y_start)); // Start point.
            for (int i = 1; i < steps; i++)
            {
                x_start = x_start + x_stepValue;
                y_start = y_start + y_stepValue;
                addSlashPoint(new Coordinate(x_start, y_start));
            }

            List<ObjectBounds> deleteObjects = new List<ObjectBounds>();
            foreach (ObjectBounds bound in object_locations)
            {
                Coordinate check = bound.getCoordinate();
                
                foreach (Coordinate slashpoint in slash)
                {
                    // If user has earned score already, do not check again.
                    if (!bound.is_score_given())
                    {
                        // Is object within slash x range.
                        if (ScreenUtils.checkPosition(check.x, slashpoint.x, bound.size_x))
                        {
                            // Is object within slash y range.
                            if (ScreenUtils.checkPosition(check.y, slashpoint.y, bound.size_y))
                            {
                                HIGH_SCORE = HIGH_SCORE + bound.getScore(); // Increase score.
                                deleteObjects.Add(bound);
                            }  
                        }
                    }
                }
            }

            foreach (ObjectBounds delete in deleteObjects)
            {
                object_locations.Remove(delete);
            }

            slash_Done = true;
            slash.Clear(); // Clear user's slash until a new slash is made.
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
                if (bound.top >= ScreenUtils.GAME_HEIGHT * ScreenUtils.SCREEN_H_RATIO)
                {
                    LIVES = LIVES - 1;
                    deleteObjects.Add(bound);
                }

                // Bounds are already converted.
                Temp.Set(bound.left, bound.top, bound.right, bound.bottom);
                canvas.DrawOval(Temp, paint);
                canvas.DrawOval(Temp, stroke_paint);
            }

            foreach (ObjectBounds delete in deleteObjects)
            {
                object_locations.Remove(delete);
            }


            // Draw user slash effect.
            /*
            int RGB = 210; // SILVER8
            foreach (Coordinate slashPoint in slash)
            {
                paint.SetARGB(200, RGB, RGB, RGB);
                Temp.Set(slashPoint.x, slashPoint.y, slashPoint.x + 5 * ScreenUtils.SCREEN_W_RATIO, slashPoint.y + 5 * ScreenUtils.SCREEN_H_RATIO);
                canvas.DrawOval(Temp, paint);
                if (!(RGB <= 0))
                    RGB--;
            }
            */

            // Draw user slash line effect.
            if (!slash_drawn)
            {
                if (slash_Done)
                {
                    paint.SetARGB(180, 150, 150, 150);
                    slash_Done = false;
                    slash_drawn = true;
                }
                else
                {
                    paint.SetARGB(255, 210, 210, 210);
                }


                paint.SetStyle(Paint.Style.FillAndStroke); // Stroke only
                paint.StrokeWidth = 5 * ScreenUtils.SCREEN_RATIO; // Stroke size
                canvas.DrawLine(slashLine_Bounds[0], slashLine_Bounds[1], slashLine_Bounds[2], slashLine_Bounds[3], paint);
            }
           
                
                /*
                GradientDrawable slashLine = new GradientDrawable(orientation, slash_color);
                slashLine.SetShape(ShapeType.Line);
                slashLine.SetSize((int)(ScreenUtils.GAME_WIDTH * ScreenUtils.SCREEN_W_RATIO), (int)(ScreenUtils.GAME_HEIGHT * ScreenUtils.SCREEN_H_RATIO));
                slashLine.SetBounds(slashLine_Bounds[0], slashLine_Bounds[1], slashLine_Bounds[2], slashLine_Bounds[3]);
                slashLine.Draw(canvas);
                */
           


            /*
             * 
             * DRAW GAME HEADERS
             * 
             */

            // Draw header background
            paint.SetARGB(255, 0, 0, 0);
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