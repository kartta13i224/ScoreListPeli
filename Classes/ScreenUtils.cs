using Android.Graphics;
using Android.Graphics.Drawables;
using System;
using System.Collections.Generic;
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
        //public static float SCREEN_FLUCTUATION;

        public static int w_PX; // Device screen width in pixels.
        public static int h_PX; // Device screen height in pixels.

        public static void screenInitialization(int width, int height)
        {
            w_PX = width;
            h_PX = height;

            // Initialize screen size ratios.
            SCREEN_H_RATIO = (float)h_PX / (float)GAME_HEIGHT;
            SCREEN_W_RATIO = (float)w_PX / (float)GAME_WIDTH;
            SCREEN_RATIO = (float)h_PX / (float)w_PX;
            //SCREEN_FLUCTUATION = SCREEN_RATIO * 10f;
        }

        private static Random rand = new Random(DateTime.Now.Millisecond);
        
        // Returns random integer between min and max.
        public static int randonInt(int min, int max)
        {
            return rand.Next(min, max);
        }

        public static bool checkPosition(float target, float selection, int size)
        {
            // target.left < selection
            if (target < selection && target + size > selection)
                return true;
            else
                return false;
        }

        // Scales the given Bitmap to correct size.
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
        
        public static void ScaleAnimation(ref AnimationDrawable animation, int newWidth, int newHeight)
        {
            AnimationDrawable new_animation = new AnimationDrawable();
            for (int i = 0; i < animation.NumberOfFrames; i++)
            {
                Bitmap temp = ((BitmapDrawable)animation.GetFrame(i)).Bitmap;
                temp = ScaleBitmap(temp, newWidth, newHeight);
                new_animation.AddFrame(new BitmapDrawable(temp), animation.GetDuration(i));
            }

            animation = new_animation;
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