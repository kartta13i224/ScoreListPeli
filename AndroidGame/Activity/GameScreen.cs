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

        List<ImageView> fruits;
        List<Matrix> fruitMatrix;
        List<AnimationDrawable> fruitAnimation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Console.Out.WriteLine(LOG_TAG + " in onCreate");
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.game);

            RelativeLayout layout = FindViewById<RelativeLayout>(Resource.Id.myDrawing);

            mObjDrawer = new ObjDrawer(this);
            layout.AddView(mObjDrawer, new ViewGroup.LayoutParams(
                RelativeLayout.LayoutParams.MatchParent,
                RelativeLayout.LayoutParams.MatchParent
            ));

            // Initialize imageViews.
            fruits = new List<ImageView>();
            fruitMatrix = new List<Matrix>();
            fruitAnimation = new List<AnimationDrawable>();

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

            // Convert coordinates.
            for (int i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = ScreenUtils.ConvertCoordinate(coordinates[i]);
            }
            

            fruits.Add(FindViewById<ImageView>(Resource.Id.imageView1));
            fruits.Add(FindViewById<ImageView>(Resource.Id.imageView2));
            fruits.Add(FindViewById<ImageView>(Resource.Id.imageView3));
            fruits.Add(FindViewById<ImageView>(Resource.Id.imageView4));
            fruits.Add(FindViewById<ImageView>(Resource.Id.imageView5));
            fruits.Add(FindViewById<ImageView>(Resource.Id.imageView6));
            fruits.Add(FindViewById<ImageView>(Resource.Id.imageView7));
            fruits.Add(FindViewById<ImageView>(Resource.Id.imageView8));

            
            for (int i = 0; i < fruits.Capacity; i++)
            {
                // Create matrixes for each Fruit.
                fruitMatrix.Add(new Matrix());
                fruitMatrix[i].Reset();
                
                fruitMatrix[i].PostTranslate(coordinates[i].x, coordinates[i].y); // Set coordinates to matrix.

                // Set matrix to specific fruit.
                fruits[i].SetScaleType(ImageView.ScaleType.Matrix);
                fruits[i].ImageMatrix = fruitMatrix[i];

                //fruits[i].Layout((int)coordinates[i].x, (int)coordinates[i].y, (int)(coordinates[i].x + fruits[i].Width * ScreenUtils.SCREEN_W_RATIO), (int)(coordinates[i].y + fruits[i].Height * ScreenUtils.SCREEN_H_RATIO));

                // Set animations to fruits.
                fruits[i].SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.SpinningThing));
                fruitAnimation.Add((AnimationDrawable)fruits[i].Background);
                fruitAnimation[i].OneShot = false;
            }
            


            //ScreenUtils.SCREEN_H_RATIO;
            //ScreenUtils.SCREEN_W_RATIO;

            /*
            // Setup the drawer.
            mObjDrawer = new ObjDrawer(this);
            SetContentView(mObjDrawer);

            mObjDrawer.SetWillNotDraw(false);

            Timer timer = new Timer()
            {
                AutoReset = true,
                Interval = TimeSpan.FromMilliseconds(100).Milliseconds
            };
            timer.Elapsed += reDraw1;
            timer.Start();
            */
        }

        /*
        private void reDraw1(object sender, ElapsedEventArgs e)
        {
            mObjDrawer.PostInvalidate();
        }
        */

        public void reDraw(View v)
        {
            mObjDrawer.Invalidate();
        }
        


        public override bool OnTouchEvent(MotionEvent e)
        {
            for (int i = 0; i < fruitAnimation.Capacity; i++)
            {
                fruitAnimation[i].Start();
            }

            float x = e.GetX();
            float y = e.GetY();

            // TODO Scale x and y to game screen.

            // TODO calculate slash distance.

            // TODO remove any objects which were "slashed" over.

            // TODO increase user's highscore.

            // TODO implement user high score.

            // TODO implement user health.


            if (e.Action == MotionEventActions.Down)
            {
                // User pressed the screen.
                Console.Out.Write(" - Pointer location: X:");
                Console.Out.Write(x);
                Console.Out.Write(" Y:");
                Console.Out.WriteLine(y);
            }

            else if (e.Action == MotionEventActions.Up)
            {
                // User released the finger.
                Console.Out.Write(" - Pointer location: X:");
                Console.Out.Write(x);
                Console.Out.Write(" Y:");
                Console.Out.WriteLine(y);


            }

            reDraw(CurrentFocus);
            return base.OnTouchEvent(e);
        }
    }
}