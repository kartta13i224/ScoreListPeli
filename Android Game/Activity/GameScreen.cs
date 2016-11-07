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
    [Activity(Label = "GameScreen", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class GameScreen : Activity
	{

        private static string LOG_TAG = "GameScreen_Activity";
        protected ObjDrawer mObjDrawer;
        private int width = 0;
        private int height = 0;

		protected override void OnCreate(Bundle savedInstanceState)
		{
            Console.Out.WriteLine(LOG_TAG + " in onCreate");
			base.OnCreate(savedInstanceState);

            // Create your application here

            //var getIntent = new Intent();
            height = Intent.GetIntExtra("DevHeight", height);
            width = Intent.GetIntExtra("DevWidth", width);

            Console.Out.WriteLine("Device Height" + height);
            Console.Out.WriteLine("Device Width" + width);

            // Set our view from the "main" layout resource
            mObjDrawer = new ObjDrawer(this, width, height);
            SetContentView(mObjDrawer);

            // Setup the drawer.
            



        }
	}
}