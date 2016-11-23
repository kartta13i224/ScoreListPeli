using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ScoreListPeli
{
    [Activity(Label = "HiScores", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class HiScores : Activity
    {
        // Fixed items.
        private static string LOG_TAG = "HiScores_Activity"; // Activity log tag.

        //LIST OF ARRAY STRINGS WHICH WILL SERVE AS LIST ITEMS
        private ListView mListView = null;
        private List<HiScoreObj.ScoreObj> scoreList = new List<HiScoreObj.ScoreObj>();

        protected WebScoreTool webScoreTool = new WebScoreTool();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Console.Out.WriteLine(LOG_TAG + " in onCreate");
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ScoreList);

            // Get our button from the layout resource,
            // and attach an event to it
            mListView = FindViewById<ListView>(Resource.Id.listView);

            webHighScore();
        }

        private async void webHighScore()
        {
            ParseScoreList(await webScoreTool.getHighScores());
        }

        private void ParseScoreList(string ScoreJSON)
        {
            // Parse ScoreJSON into listView cells.

           
            // TODO DATA LISTVIEWII TÄÄL
            // Check that ScoreList is available.      
            if (ScoreJSON != null)
            {
                ScoreJSON = ScoreJSON.Substring(1);
                // LINK DATA; DESERIALIZE
                HiScoreObj obj = JsonConvert.DeserializeObject<HiScoreObj>(ScoreJSON) ;
                //Android.Widget.Toast.MakeText(this, "Data haettu!", Android.Widget.ToastLength.Short).Show();
                
                scoreList.Clear(); // Remove old entries from the hiscore list.
                if (obj != null && obj.HiScores != null)
                {
                    foreach (var ScoreObj in obj.HiScores)
                    {
                        HiScoreObj.ScoreObj temp = new HiScoreObj.ScoreObj(ScoreObj.Name, ScoreObj.Score);
                        scoreList.Add(temp);
                    }
                }

                if (scoreList != null)
                {
                    // Android.Widget.Toast.MakeText(this, obj.ToString(), Android.Widget.ToastLength.Short).Show();
                    mListView.Adapter = new ScoreAdapter(this, scoreList.ToArray());
                }

            }

            else
                Toast.MakeText(this, "Check your internet connection!", ToastLength.Short).Show();

        }

    }
     
}

