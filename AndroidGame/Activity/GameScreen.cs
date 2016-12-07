using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.Timers;
using ScoreListPeli.Classes;
namespace ScoreListPeli
{
    [Activity(Label = "GameScreen", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class GameScreen : Activity
	{
        private static string LOG_TAG = "GameScreen_Activity"; // Activity log tag.

        protected ObjDrawer mObjDrawer;

        RelativeLayout layout;

        private List<Coordinate> slash;


        /*
        List<ImageView> fruits;
        List<FallObject_normal> fruits;
        List<AnimationDrawable> fruitAnimation;

        AnimationDrawable SpinningThing;

        // Create coordinates for dynamic ImageViews
        Coordinate[] coordinates = new Coordinate[] {
                new Coordinate((float)ScreenUtils.GAME_WIDTH / 8f, (float)ScreenUtils.GAME_HEIGHT / 8f), // ImageView 0
                new Coordinate((float)ScreenUtils.GAME_WIDTH - ((float)ScreenUtils.GAME_WIDTH / 8f), (float)ScreenUtils.GAME_HEIGHT / 8f), // ImageView 1
                new Coordinate((float)ScreenUtils.GAME_WIDTH / 2f, (float)ScreenUtils.GAME_HEIGHT / 8f * 2), // ImageView 2
                new Coordinate((float)ScreenUtils.GAME_WIDTH / 8f, (float)ScreenUtils.GAME_HEIGHT / 2f), // ImageView 3
                new Coordinate((float)ScreenUtils.GAME_WIDTH - ((float)ScreenUtils.GAME_WIDTH / 8f), (float)ScreenUtils.GAME_HEIGHT / 2f), // ImageView 4
                new Coordinate((float)ScreenUtils.GAME_WIDTH / 2f, (float)ScreenUtils.GAME_HEIGHT - (float)ScreenUtils.GAME_HEIGHT / 8f * 2), // ImageView 5
                new Coordinate((float)ScreenUtils.GAME_WIDTH / 8f, (float)ScreenUtils.GAME_HEIGHT - (float)ScreenUtils.GAME_HEIGHT / 8f), // ImageView 6
                new Coordinate((float)ScreenUtils.GAME_WIDTH - ((float)ScreenUtils.GAME_WIDTH / 8f), (float)ScreenUtils.GAME_HEIGHT - (float)ScreenUtils.GAME_HEIGHT / 8f) // ImageView 7
            };
            */
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Console.Out.WriteLine(LOG_TAG + " in onCreate");
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.game);

            layout = FindViewById<RelativeLayout>(Resource.Id.myDrawing);

            mObjDrawer = new ObjDrawer(this);
            layout.AddView(mObjDrawer, new ViewGroup.LayoutParams(
                RelativeLayout.LayoutParams.MatchParent,
                RelativeLayout.LayoutParams.MatchParent
            ));

            slash = new List<Coordinate>();

            // Initialize lists for imageViews.
            //fruits = new List<ImageView>();
           // fruits = new List<FallObject_normal>();
            //fruitAnimation = new List<AnimationDrawable>();

            /*
            SpinningThing = (AnimationDrawable)(Resources.GetDrawable(Resource.Drawable.SpinningThing));
            ScreenUtils.ScaleAnimation(ref SpinningThing, (int)(79 * ScreenUtils.SCREEN_W_RATIO), (int)(79 * ScreenUtils.SCREEN_H_RATIO));
            */

            /*
            for (int i = 0; i < 1; i++)
            {
                // Convert coordinates.
                coordinates[i] = ScreenUtils.ConvertCoordinate(coordinates[i]);

                ImageView tempView = new ImageView(this);

                // Set animations to fruits.
                tempView.SetBackgroundDrawable(SpinningThing);
                fruitAnimation.Add((AnimationDrawable)tempView.Background);                
                fruitAnimation[i].OneShot = false;

                // Set image scale.
                RelativeLayout.LayoutParams tempParams = new RelativeLayout.LayoutParams(fruitAnimation[i].IntrinsicWidth, fruitAnimation[i].IntrinsicHeight);

                // Set location via Parameter's margins.
                tempParams.TopMargin = (int)coordinates[i].y;
                tempParams.LeftMargin = (int)coordinates[i].x;

                // Add it to fruits list.
                fruits.Add(tempView);
                layout.AddView(fruits[i], tempParams);              
            }
            */
            /*
            FallObject_normal tempObject = new FallObject_normal(this, ScreenUtils.GAME_WIDTH / 2, ScreenUtils.GAME_HEIGHT / 2);
            RelativeLayout.LayoutParams tempParams = new RelativeLayout.LayoutParams(tempObject.Width, tempObject.Height);

            tempParams.LeftMargin = (int)tempObject.Coordinates.x;
            tempParams.TopMargin = (int)tempObject.Coordinates.y;

            layout.AddView(tempObject, tempParams);
            */

            /*
            // Setup the drawer.
            mObjDrawer = new ObjDrawer(this);
            SetContentView(mObjDrawer);

            mObjDrawer.SetWillNotDraw(false);
            */

            
            Timer timer = new Timer()
            {
                AutoReset = true,
                Interval = TimeSpan.FromMilliseconds(100).Milliseconds
            };

            timer.Elapsed += refreshObjects;
            timer.Start();
            
        }

        private void refreshObjects(object sender, ElapsedEventArgs e)
        {
            if (mObjDrawer.LIVES <= 0)
            {
                Finish();
            }

            mObjDrawer.fallObject();
            mObjDrawer.PostInvalidate();
        }


        public void reDraw(View v)
        {
            //mObjDrawer.fallObject();
            mObjDrawer.Invalidate();
        }
        


        public override bool OnTouchEvent(MotionEvent e)
        {
            //fruitAnimation[0].Start();

            float x = e.GetX();
            float y = e.GetY();

            // TODO Scale x and y to game screen.

            // TODO calculate slash distance.

            // TODO remove any objects which were "slashed" over.

            // TODO increase user's highscore.

            // TODO implement user high score.

            // TODO implement user health.


            if (e.Action == MotionEventActions.Move)
            {
                // User still presses the screen.
                slash.Add(new Coordinate(x, y));
                
                /*
                Console.Out.Write(" - Pointer location: X:");
                Console.Out.Write(x);
                Console.Out.Write(" Y:");
                Console.Out.WriteLine(y);
                */

            }
            else if (e.Action == MotionEventActions.Down)
            {
                // User pressed the screen.
            }
            else if (e.Action == MotionEventActions.Up)
            {
                // User released the finger.
                /*     
                Console.Out.Write(" - Pointer location: X:");
                Console.Out.Write(x);
                Console.Out.Write(" Y:");
                Console.Out.WriteLine(y);
                */
                mObjDrawer.checkSlash(slash);
                slash.Clear();
            }

            //reDraw(CurrentFocus);
            return base.OnTouchEvent(e);
        }
    }
}