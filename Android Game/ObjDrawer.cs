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
        
        // A fixed numbers for resizing components for the device's screen.
        private static int GAME_WIDTH = 100;
        private static int GAME_HEIGHT = 100;

        // Multipliers to resize the components.
        private double SCREEN_W_RATIO = 1;
        private double SCREEN_H_RATIO = 1;

        private ShapeDrawable _shape;

        private int widthInDp; // Device screen width.
        private int heightInDp; // Device screen height.
        

        public ObjDrawer(Android.Content.Context context, int wDP, int hDP) :
            base(context)
        {
            SetBackgroundResource(Resource.Drawable.rainbow_texture679532534);
            widthInDp = wDP;
            heightInDp = hDP;
            Initialize();
        }

        private void Initialize()
        {
            // TODO Initialization

            SCREEN_H_RATIO = (double)heightInDp / (double)GAME_HEIGHT;
            SCREEN_W_RATIO = (double)widthInDp / (double)GAME_WIDTH;
            // Create android paint object.
            var objPaint = new Paint();

            // Set color
            objPaint.SetARGB(255, 100, 125, 255);
            objPaint.SetStyle(Paint.Style.Stroke);
            objPaint.StrokeWidth = 4;

            _shape = new ShapeDrawable(new OvalShape());
            _shape.Paint.Set(objPaint);

            // setShapeSize(_shape, coordinate(x,y), width, height);
            _shape = setShapeSize(_shape, new Classes.Coordinate(50, 50), 25, 25);

        }

        // Set's the shape in a correct place at correct size/ratio depending on the device.
        // inputShape is the given Shape which's bounds we need to modify.
        // coord is a Coordinate object where the object should be placed in x = width and y = height.
        // coord is the upper left corner of the object where it will be placed.
        // y max is the device's bottom and y 0 is the top of the device.
        // x max is the device's right side and x 0 is the left side of the device.
        // height is the shape's height.
        // width is the shape's width.
        protected ShapeDrawable setShapeSize(ShapeDrawable inputShape, Classes.Coordinate coord, double width, double height){

            // TODO CALCULATE BOUNDS
            width = width * SCREEN_W_RATIO;
            height = height * SCREEN_H_RATIO;

            // TODO check that the bounds don't exceed the device's screen.

            // Horizontal bound locations.
            double leftSide = coord.x * SCREEN_W_RATIO;
            double rightSide = coord.x + width;

            // Vertical bound locations.
            double topSide = coord.y * SCREEN_Y_RATIO;
            double bottomSide = coord.y + height;

            //   SetBounds(int left side, int top side, int right side, int bottom side)
            inputShape.SetBounds((int)leftSide, (int)topSide, (int)rightSide, (int)bottomSide);

            return inputShape;
        }





        protected double widthRatio(int size)
        {
            return size * (widthInDp / 100);
        }

        protected double heightRatio(int size)
        {
            return size * (heightInDp / 100);
        }

        protected override void OnDraw (Canvas canvas)
        {
            // TODO draw functions
            _shape.Draw(canvas);
        }
    }
}