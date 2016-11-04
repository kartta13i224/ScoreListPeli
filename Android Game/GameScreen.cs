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
	[Activity(Label = "GameScreen")]
	public class GameScreen : Activity
	{

        protected ObjDrawer mObjDrawer;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            // Create your application here

            // Set our view from the "main" layout resource
            mObjDrawer = new ObjDrawer(this);
            SetContentView(mObjDrawer);

            // Setup the drawer.
            



        }
	}
}