using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;

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

        // Returns true if everything went ok, otherwise false.
        public bool write(HiScoreObj.ScoreObj score)
        {

            return PostScore(score);
          
            /*
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
               
        }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
            */
        }

        protected bool PostScore(HiScoreObj.ScoreObj score)
        {
            WebClient client = new WebClient();
            Uri uri = new Uri(URL_BASE + GET_POST);

            string ScoresAsJson = JsonConvert.SerializeObject(score);
            try
            {
                client.UploadStringCompleted += client_UploadStringCompleted;
                client.UploadStringAsync(uri, ScoresAsJson);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;

            /*
            try
            {
                    using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                       { "Name", score.Name },
                       { "Score", score.Score.ToString() }
                    };

                    var content = new FormUrlEncodedContent(values);

                    var response = await client.PostAsync(URL_BASE + GET_POST, content);

                    var responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
            */
        }

        void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            // Get's called when sending post is done!
            Console.WriteLine(e);
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
        /*
        // Checks for the device's internet connection.
        public bool checkInternetConnection()
        {
            return false;
        }
        */
    }
}