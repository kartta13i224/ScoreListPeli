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
    public class WebScoreTool
    {
        // URL where to get data from.
        private const string URL = "http://home.tamk.fi/~e5tjokin/scorelist/HiScores.json";


        // Constructor
        public WebScoreTool()
        {}

        public async Task<string> getHighScores()
        {
            string ScoreJSON = await FetchScoreList(URL);
            // Call function to parse it if not null.
            if (ScoreJSON != null && ScoreJSON.Length > 1)
                return ScoreJSON;
            else
            {
                // Send out error message to UI.
                return null;
                //Android.Widget.Toast.MakeText(this, "Check your internet connection!", Android.Widget.ToastLength.Short).Show();
            }
        }

        public void write()
        {
            try
            {
                List<HiScoreObj.ScoreObj> scoreList = new List<HiScoreObj.ScoreObj>();
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

        protected async Task<string> FetchScoreList(string URL)
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
            catch (WebException ex)
            {
                Console.Out.WriteLine("Internet connection error!");
                Console.Out.WriteLine(ex);
                return null;
            }
        }

        

    }
}