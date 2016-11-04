using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

namespace ScoreListPeli
{
    public class ObjDrawer : Android.Views.View
    {
        private ShapeDrawable _shape;

        public ObjDrawer(Android.Content.Context context) :
            base(context)
        {
            SetBackgroundResource(Resource.Drawable.rainbow_texture679532534);
            Initialize();
        }

        private void Initialize()
        {
            // TODO Initialization
            

            // Create android paint object.
            var objPaint = new Paint();

            // Set color
            objPaint.SetARGB(255, 200, 255, 0);
            objPaint.SetStyle(Paint.Style.Stroke);
            objPaint.StrokeWidth = 4;

            _shape = new ShapeDrawable(new OvalShape());
            _shape.Paint.Set(objPaint);


            _shape.SetBounds(20, 20, 300, 200);
        }

        protected override void OnDraw (Canvas canvas)
        {
            // TODO draw functions
            _shape.Draw(canvas);
        }
    }
}