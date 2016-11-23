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
using Android.Graphics;

namespace ScoreListPeli.Classes
{
    public static class ScreenUtils
    {
        // A fixed numbers for resizing components for the device's screen.
        public static readonly int GAME_WIDTH = 400;
        public static readonly int GAME_HEIGHT = 800;
        public static readonly int GAME_RATIO = GAME_HEIGHT / GAME_WIDTH;
        public static readonly int GAME_TEXT_SIZE = GAME_WIDTH / 10;

        // Multipliers to resize the components.
        public static float SCREEN_W_RATIO = 1;
        public static float SCREEN_H_RATIO = 1;
        public static float SCREEN_RATIO;

        public static int w_PX; // Device screen width in pixels.
        public static int h_PX; // Device screen height in pixels.

        public static void screenInitialization(int width, int height)
        {
            w_PX = width;
            h_PX = height;

            // Initialize screen size ratios.
            SCREEN_H_RATIO = (float)h_PX / (float)GAME_HEIGHT;
            SCREEN_W_RATIO = (float)w_PX / (float)GAME_WIDTH;
            SCREEN_RATIO = SCREEN_H_RATIO / SCREEN_W_RATIO;
        }
        
                

        public static Bitmap ScaleBitmap(Bitmap temp, int newWidth, int newHeight)
        {
            float width = temp.Width;
            float height = temp.Height;

            float scaleWidth = (newWidth * SCREEN_W_RATIO) / width;
            float scaleHeight = (newHeight * SCREEN_H_RATIO) / height;

            Matrix matrix = new Matrix();

            matrix.PostScale(scaleWidth, scaleHeight);

            Bitmap resizedBitmap = Bitmap.CreateBitmap(temp, 0, 0, (int)width, (int)height, matrix, false);

            temp.Recycle();
            return resizedBitmap;
        }

        // Converts Coordinates for display screen size.
        public static Coordinate ConvertCoordinate(Coordinate input)
        {
            input.x = input.x * SCREEN_W_RATIO;
            input.y = input.y * SCREEN_H_RATIO;

            return input;
        }

        // Converts bounds for display screen size.
        public static int[] convertBounds(float left, float top, float right, float bottom)
        {
            int[] bounds = new int[4];
            bounds[0] = (int)(left * SCREEN_W_RATIO); // LEFT
            bounds[1] = (int)(top * SCREEN_H_RATIO); // TOP
            bounds[2] = (int)(right * SCREEN_W_RATIO); // RIGHT
            bounds[3] = (int)(bottom * SCREEN_H_RATIO); // BOTTOM

            return bounds;
        }

        // Converts bounds for display screen size.
        public static int[] convertBounds(float x, float y)
        {
            int[] bounds = new int[4];
            bounds[0] = (int)(x * SCREEN_W_RATIO); // LEFT
            bounds[1] = (int)(y * SCREEN_H_RATIO); // TOP

            return bounds;
        }
    }
}