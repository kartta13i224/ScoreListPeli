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

namespace ScoreListPeli
{
    [Activity(Label = "Fruity Click Mix 3000", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainMenu : Activity
    {

        // INSTALL_FAILED_UPDATE_INCOMPATIBLE <- Poista ohjelma laitteesta.

        Button StartGame;
        Button HighScores;
        Button Rate;
        TextView Text;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            

            

            var metrics = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);

            

            string temp = widthInDp.ToString();
            int width = Int32.Parse(temp);

            if (width >= 600)
            {
                Console.Out.WriteLine("Large view activated.");
                SetContentView(Resource.Layout.mainMenu_sw600dp);
            }
            else if (width >= 360)
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
                StartActivity(typeof(GameScreen));
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

        private object ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
    }
}