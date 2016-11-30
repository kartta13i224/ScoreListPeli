using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Timers;

namespace ScoreListPeli
{
    [Activity(Label = "GameScreen", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class GameScreen : Activity
	{
        private static string LOG_TAG = "GameScreen_Activity"; // Activity log tag.

        protected ObjDrawer mObjDrawer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Console.Out.WriteLine(LOG_TAG + " in onCreate");
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
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
            
        }

        
        private void reDraw1(object sender, ElapsedEventArgs e)
        {
            mObjDrawer.PostInvalidate();
        }
        

        public void reDraw(View v)
        {
            mObjDrawer.Invalidate();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
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