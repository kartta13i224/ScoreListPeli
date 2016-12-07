using Android.Content;
using Android.Widget;

using Android.Graphics;
using Android.Graphics.Drawables;
using System.Timers;

namespace ScoreListPeli.Classes
{
    class FallObject_normal : ImageView
    {
        public Coordinate Coordinates { get; set; }
        new public int Width { get; set; }
        new public int Height { get; set; }
        public int FallingSpeed { get; set; }
        
        public AnimationDrawable animation;

        public FallObject_normal(Context c, float x, float y)
            : base(c)
        {
            Width = 79;
            Height = 79;

            animation = (AnimationDrawable)c.Resources.GetDrawable(Resource.Drawable.SpinningThing);
            ScreenUtils.ScaleAnimation(ref animation, (int)(Width * ScreenUtils.SCREEN_W_RATIO), (int)(Height * ScreenUtils.SCREEN_H_RATIO));
            SetBackgroundDrawable(animation);          

            animation.OneShot = false;
            
            Coordinates = ScreenUtils.ConvertCoordinate(new Coordinate(x, y));
            FallingSpeed = 1;
        }

        public void start()
        {
            animation.Start();

            /*
            double interval = 1000 / FallingSpeed;
            Timer timer = new Timer()
            {
                AutoReset = true,
                Interval = interval
            };
            timer.Elapsed += Fall;
            timer.Start();
            */
        }

        // Changes the location of this view.
        public void changeViewTest(float x, float y)
        {
            //this.Animate().X(x).Y(y).SetDuration(0).Start();
        }

        // Drops view
        private void Fall(object sender, ElapsedEventArgs e)
        {
            Height++;
            Width++;
            ScreenUtils.ScaleAnimation(ref animation, (int)(Width * ScreenUtils.SCREEN_W_RATIO), (int)(Height * ScreenUtils.SCREEN_H_RATIO));
            SetBackgroundDrawable(animation);
            //Coordinates.y = Coordinates.y + 1;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            start(); // Starts animation + falling
        }
    }
}