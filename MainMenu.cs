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

        Button StartGame;
        Button HighScores;
        Button Rate;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.mainMenu);

            StartGame = FindViewById<Button>(Resource.Id.BTN_NewGame);
            HighScores = FindViewById<Button>(Resource.Id.BTN_HighScore);
            Rate = FindViewById<Button>(Resource.Id.BTN_Rate);

            StartGame.Click += delegate {
                // TODO open new game activity
                Console.Out.WriteLine("New game button pressed!");
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

    }
}