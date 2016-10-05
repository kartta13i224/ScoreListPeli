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
    [Activity(Label = "HiScores", MainLauncher = true, Icon = "@drawable/icon")]
    public class HiScores : Activity
    {
        // Fixed items.
        // 

        //LIST OF ARRAY STRINGS WHICH WILL SERVE AS LIST ITEMS
        private ListView mListView = null;
        private List<HiScoreObj.ScoreObj> scoreList = new List<HiScoreObj.ScoreObj>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            mListView = FindViewById<Android.Widget.ListView>(Resource.Id.listView);

            Android.Widget.Button button = FindViewById<Android.Widget.Button>(Resource.Id.btn1);

            button.Click += async (sender, e) =>
            {
                // URL where to get data from.
                string URL = "http://home.tamk.fi/~e5tjokin/scorelist/HiScores.json";

                string ScoreJSON = await FetchScoreList(URL);
                // Call function to parse it.
                ParseScoreList(ScoreJSON);
            };


        }

        private async Task<string> FetchScoreList(string URL)
        {


            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(URL));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                string ScoreText;
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    ScoreText = sr.ReadToEnd();
                    Console.Out.WriteLine("Response: {0}", ScoreText);
                    
                }
                return ScoreText;

            }
        }


        private void ParseScoreList(string ScoreJSON)
        {
            // Parse ScoreJSON into listView cells.

           
            // TODO DATA LISTVIEWII TÄÄL
            // Check that ScoreList is available.
            if (ScoreJSON != null)
            {
                // LINK DATA; DESERIALIZE
                HiScoreObj obj = JsonConvert.DeserializeObject<HiScoreObj> (ScoreJSON);
                Android.Widget.Toast.MakeText(this, "Data haettu!", Android.Widget.ToastLength.Short).Show();
                
                scoreList.Clear(); // Remove old entries from the hiscore list.
                foreach (var ScoreObj in obj.HiScores)
                {
                    HiScoreObj.ScoreObj temp = new HiScoreObj.ScoreObj();
                    temp.nick = ScoreObj.nick;
                    temp.points = ScoreObj.points;
                    scoreList.Add(temp);
                }

                if (scoreList != null)
                {
                    // Android.Widget.Toast.MakeText(this, obj.ToString(), Android.Widget.ToastLength.Short).Show();
                    mListView.Adapter = new ScoreAdapter(this, scoreList.ToArray());
                }

            }

        }

    }
     
}

