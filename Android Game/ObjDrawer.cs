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
        private static int GAME_WIDTH = 400;
        private static int GAME_HEIGHT = 800;

        // Multipliers to resize the components.
        private float SCREEN_W_RATIO = 1;
        private float SCREEN_H_RATIO = 1;

        private int w_PX; // Device screen width in pixels.
        private int h_PX; // Device screen height in pixels.


        public ObjDrawer(Android.Content.Context context, int wDP, int hDP) :
            base(context)
        {
            SetBackgroundResource(Resource.Drawable.rainbow_texture679532534);
            w_PX = wDP;
            h_PX = hDP;
            Initialize();
        }

        private void Initialize()
        {
            // TODO Initialization

            SCREEN_H_RATIO = (float)h_PX / (float)GAME_HEIGHT;
            SCREEN_W_RATIO = (float)w_PX / (float)GAME_WIDTH;

            // setShapeSize(_shape, coordinate(x,y), width, height);
            // _shape = setShapeSize(_shape, new Classes.Coordinate(50, 50), 25, 25);
            //_shape.SetBounds(400, 300, 800, 600);

        }

        // Set's the shape in a correct place at correct size/ratio depending on the device.
        // inputShape is the given Shape which's bounds we need to modify.
        // coord is a Coordinate object where the object should be placed in x = width and y = height.
        // coord is the upper left corner of the object where it will be placed.
        // y max is the device's bottom and y 0 is the top of the device.
        // x max is the device's right side and x 0 is the left side of the device.
        // height is the shape's height.
        // width is the shape's width.
        protected RectF setShapeSize(RectF inputShape){

            // TODO CALCULATE BOUNDS
            inputShape.Left = inputShape.Left * SCREEN_W_RATIO;
            inputShape.Right = inputShape.Right * SCREEN_W_RATIO;
            inputShape.Top = inputShape.Top * SCREEN_H_RATIO;
            inputShape.Bottom = inputShape.Bottom * SCREEN_H_RATIO;

            //   SetBounds(int left side, int top side, int right side, int bottom side)
            // inputShape.SetBounds((int)leftSide, (int)topSide, (int)rightSide, (int)bottomSide);

            return inputShape;
        }

        protected Classes.Coordinate ConvertCoordinate(Classes.Coordinate input)
        {
            input.x = input.x * SCREEN_W_RATIO;
            input.y = input.y * SCREEN_H_RATIO;

            return input;
        }

        private void drawCircle(Canvas currCanvas, Paint currPaint, int radius, Classes.Coordinate loc)
        {
            loc = ConvertCoordinate(loc);
            currCanvas.DrawCircle(loc.x, loc.y, radius, currPaint);
        }

        protected override void OnDraw (Canvas canvas)
        {
            var paint = new Paint();

            paint.SetARGB(250, 50, 125, 255);
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeWidth = 5;

            // TODO draw functions
            
            RectF ovalTemp = new RectF(0, GAME_HEIGHT-(GAME_HEIGHT/8), GAME_WIDTH, GAME_HEIGHT);
            ovalTemp = setShapeSize(ovalTemp);
            canvas.DrawOval(ovalTemp,paint);

            Classes.Coordinate temp = new Classes.Coordinate(GAME_WIDTH / 2, GAME_HEIGHT / 2);
            paint.SetARGB(250, 255, 0, 0);
            drawCircle(canvas, paint, 50, temp);

        }
    }
}