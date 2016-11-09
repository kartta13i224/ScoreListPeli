using System;
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
        private const string URL_BASE = "http://home.tamk.fi/~e5tjokin/HighScorePeli/index.php";
        private const string GET_POST = "/highscores";
        private const string GET_9 = "/gettop9";
        private const string JSON_GET = "http://home.tamk.fi/~e5tjokin/HighScorePeli/HiScores.json";

        // Constructor
        public WebScoreTool()
        {}

        public async Task<string> getHighScores()
        {
            // URL_BASE + GET_POST
            string ScoreJSON = await FetchScoreList(URL_BASE + GET_POST);
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

        public void write(HiScoreObj.ScoreObj score)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL_BASE + GET_POST);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string ScoresAsJson = JsonConvert.SerializeObject(score);
                    streamWriter.Write(ScoresAsJson);

                    streamWriter.Flush();
                    streamWriter.Close();
                }
                /*
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
                */
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected async Task<string> FetchScoreList(string url)
        {

            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
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