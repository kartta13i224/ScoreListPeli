using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using ScoreListPeli.Classes;
using Android.Views.InputMethods;
using Android.Net;
using Android.Views;
using System.Threading.Tasks;

namespace ScoreListPeli
{
    [Activity(Label = "Fruity Click Mix 3000", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainMenu : Activity
    {
        private static string LOG_TAG = "MainMenu_Activity"; // Activity log tag.
        
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
            Console.Out.WriteLine(LOG_TAG + " in onCreate");
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

                // Hide keyboard when game starts.
                View view = CurrentFocus;
                if (view != null)
                {
                    InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(view.WindowToken, 0);
                }

                Console.Out.WriteLine(LOG_TAG + " New game button pressed!");
                var GameActivity = new Intent(this, typeof(GameScreen));
                StartActivityForResult(GameActivity, 0);
            };

            HighScores.Click += delegate
            {
                Console.Out.WriteLine(LOG_TAG + " High Score List button pressed!");
                StartActivity(typeof(HiScores));
            };

            Rate.Click += delegate
            {
                // TODO Rate application
                Toast.MakeText(this, "To be implemented in a future version.", ToastLength.Short).Show();
                Console.Out.WriteLine(LOG_TAG + " Rate button pressed!");
            };


            // Initialize game screen; Sends pixel width and pixel height.
            ScreenUtils.screenInitialization(w_px, h_px);
        }

        // Returned from the game. Show user score.
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                if (data != null)
                {
                    int userScore = 0;
                    userScore = data.GetIntExtra("score", userScore);

                    var scoreDialog = new AlertDialog.Builder(this);
                    EditText userNickInput = new EditText(this);

                    string userName = string.Empty;
                    //userNickInput.Text = GetSavedInput(input, out selectedInput); // If screen portrait changed, not used.
                    userNickInput.InputType = Android.Text.InputTypes.TextVariationShortMessage; // Type of input.

                    scoreDialog.SetTitle("Hiscore: " + userScore); // Set title.
                    scoreDialog.SetView(userNickInput); // Set editText object into dialog.

                    // Create an OK button.
                    scoreDialog.SetPositiveButton("Send Hiscore", (EventHandler<DialogClickEventArgs>)null);
                    scoreDialog.SetNegativeButton("Discard", (EventHandler<DialogClickEventArgs>)null);

                    var dialog = scoreDialog.Create();

                    dialog.Show();
                    //ShowKeyboard(userNickInput);

                    var BTN_send = dialog.GetButton((int)DialogButtonType.Positive);
                    var BTN_disc = dialog.GetButton((int)DialogButtonType.Negative);

                    BTN_send.Click += (sender, args) =>
                    {
                        if (userNickInput.Text != string.Empty && userNickInput.Length() > 0)
                        {
                            // Check internet connection access.
                            ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
                            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
                            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;
                            NetworkInfo wifiInfo = connectivityManager.GetNetworkInfo(ConnectivityType.Wifi); // Get wifi state
                            NetworkInfo mobileInfo = connectivityManager.GetNetworkInfo(ConnectivityType.Mobile); // Get mobile state

                            // If either of the networks have access to internet...
                            if (wifiInfo.IsConnected || mobileInfo.IsConnected)
                            {
                                WebScoreTool webScoreTool = new WebScoreTool();
                                HiScoreObj.ScoreObj temp = new HiScoreObj.ScoreObj(userNickInput.Text, userScore); // Create temporary hiscore Object.

                                if (webScoreTool.write(temp))
                                {
                                    // Sent successfully.
                                    //HideKeyboard(userNickInput); // Hide keyboard.
                                    dialog.Dismiss(); // Dismiss dialog.
                                }
                                // Error sending.
                                else
                                {
                                    Toast.MakeText(this, "Check your internet connection!", ToastLength.Short).Show();
                                }
                            }

                            else
                            {
                                Toast.MakeText(this, "Check your internet connection!", ToastLength.Short).Show();
                            }
                        }

                        // Not valid username.
                        else
                        {
                            Toast.MakeText(this, "Write a proper username!", ToastLength.Short).Show();
                        }
                    };

                    // Exit button. No hiscores will be saved.
                    BTN_disc.Click += (sender, args) =>
                    {
                        //HideKeyboard(userNickInput);
                        dialog.Dismiss(); // Dismiss dialog.
                    };
                }

            }
        }

        /*
        // Shows keyboard within the given EditText component.
        private void ShowKeyboard(EditText userInput)
        {
            userInput.RequestFocus();
            InputMethodManager imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            imm.ToggleSoftInput(ShowFlags.Forced, 0);
        }

        // Hides the keyboard when not in the EditText component.
        private void HideKeyboard(EditText userInput)
        {
            if (userInput != null)
            {
                InputMethodManager imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(userInput.WindowToken, 0);
            }
            
        }
        */

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