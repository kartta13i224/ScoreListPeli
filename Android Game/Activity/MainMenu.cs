using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace ScoreListPeli
{
    [Activity(Label = "Fruity Click Mix 3000", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainMenu : Activity
    {

        // INSTALL_FAILED_UPDATE_INCOMPATIBLE <- Poista ohjelma laitteesta.

        Button StartGame;
        Button HighScores;
        Button Rate;
        // TextView Text;

        // Variables that store screen size.
        private int w_px;
        private int h_px;
        private float w_dp;
        private float h_dp;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            setScreenVariables();

            if (w_dp >= 600)
            {
                Console.Out.WriteLine("Large view activated.");
                SetContentView(Resource.Layout.mainMenu_sw600dp);
            }
            else if (w_dp >= 360)
            {
                Console.Out.WriteLine("Medium view activated.");
                SetContentView(Resource.Layout.mainMenu_sw360dp);
            }
            else
            {
                Console.Out.WriteLine("Small view activated.");
                SetContentView(Resource.Layout.mainMenu);
            }
                


            StartGame = FindViewById<Button>(Resource.Id.BTN_NewGame);
            HighScores = FindViewById<Button>(Resource.Id.BTN_HighScore);
            Rate = FindViewById<Button>(Resource.Id.BTN_Rate);

            StartGame.Click += delegate {
                // TODO open new game activity
                Console.Out.WriteLine("New game button pressed!");
                var GameActivity = new Intent(this, typeof(GameScreen));
                GameActivity.PutExtra("DevHeight", h_px);
                GameActivity.PutExtra("DevWidth", w_px);
                StartActivity(GameActivity);
            };

            HighScores.Click += delegate
            {
                Console.Out.WriteLine("High Score List button pressed!");
                StartActivity(typeof(HiScores));
            };

            Rate.Click += delegate
            {
                // TODO Rate application
                Console.Out.WriteLine("Rate button pressed!");
            };
        }

        private float ConvertPixelsToDp(float pixelValue)
        {
            float dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }

        private void setScreenVariables()
        {
            var metrics = Resources.DisplayMetrics;
            w_px = metrics.WidthPixels;
            h_px = metrics.HeightPixels;

            w_dp = ConvertPixelsToDp(w_px);
            h_dp = ConvertPixelsToDp(h_px);
        }
    }
}