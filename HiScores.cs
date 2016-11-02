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
    [Activity(Label = "HiScores")]
    public class HiScores : Activity
    {
        // Fixed items.

        //LIST OF ARRAY STRINGS WHICH WILL SERVE AS LIST ITEMS
        private ListView mListView = null;
        private List<HiScoreObj.ScoreObj> scoreList = new List<HiScoreObj.ScoreObj>();

        // URL where to get data from.
        private const string URL = "http://home.tamk.fi/~e5tjokin/scorelist/HiScores.json";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ScoreList);

            // Get our button from the layout resource,
            // and attach an event to it
            mListView = FindViewById<Android.Widget.ListView>(Resource.Id.listView);

            ////write();
            getHighScores();

        }

        private async void getHighScores()
        {
            string ScoreJSON = await FetchScoreList(URL);
            // Call function to parse it if not null.
            if (ScoreJSON != null)
                ParseScoreList(ScoreJSON);
            else
            {
                Android.Widget.Toast.MakeText(this, "Check your internet connection!", Android.Widget.ToastLength.Short).Show();
            }
        }

        private void write()
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    scoreList.Add(new HiScoreObj.ScoreObj("paras", 100));
                    scoreList.Add(new HiScoreObj.ScoreObj("huono", 5));
                    scoreList.Add(new HiScoreObj.ScoreObj("toinen", 95));
                
                    string ScoresAsJson = JsonConvert.SerializeObject(scoreList);
                    streamWriter.Write(ScoresAsJson);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task<string> FetchScoreList(string URL)
        {

            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(URL));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
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
            catch (WebException ex) {
                Console.Out.WriteLine("Internet connection error!");
                Console.Out.WriteLine(ex);
                return null;
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
                //Android.Widget.Toast.MakeText(this, "Data haettu!", Android.Widget.ToastLength.Short).Show();
                
                scoreList.Clear(); // Remove old entries from the hiscore list.
                if (obj != null && obj.HiScores != null)
                {
                    foreach (var ScoreObj in obj.HiScores)
                    {
                        HiScoreObj.ScoreObj temp = new HiScoreObj.ScoreObj(ScoreObj.nick, ScoreObj.points);
                        scoreList.Add(temp);
                    }
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

