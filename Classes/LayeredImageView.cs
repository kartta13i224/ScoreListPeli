using System;
using System.Collections.Generic;

using Android.Content;
using Android.Content.Res;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views.Animations;

namespace ScoreListPeli.Classes
{
    class LayeredImageView
    {
        /*
        private static string LOG_TAG = "LayeredImageView"; // Activity log tag.
        private List<Layer> mLayers;
        private Matrix mDrawMatrix;

        private Resources mResources;
        private Context mContext;

        public LayeredImageView(Context context) : base(context)
        {
            mContext = context;
            init();
        }

        public LayeredImageView(Context context, IAttributeSet set) : base(context, set)
        {
            mContext = context;
            init();

            int[] attrs = {
                Android.Resource.Attribute.Src
            };

            TypedArray a = context.ObtainStyledAttributes(set, attrs);
            TypedValue outValue = new TypedValue();
            if (a.GetValue(0, outValue))
            {
                SetImageResource(outValue.ResourceId);
            }
            a.Recycle();
        }

        private void init()
        {
            mLayers = new List<Layer>();
            mDrawMatrix = new Matrix();
            mResources = new LayeredImageViewResources(mContext);
        }

        override protected bool VerifyDrawable(Drawable dr)
        {
            for (int i = 0; i < mLayers.Capacity; i++)
            {
                Layer layer = mLayers[i];
                if (layer.drawable == dr)
                {
                    return true;
                }
            }
            return base.VerifyDrawable(dr);
        }

        override public void InvalidateDrawable(Drawable dr)
        {
            if (VerifyDrawable(dr))
            {
                Invalidate();
            }
            else
            {
                base.InvalidateDrawable(dr);
            }
        }

        public Resources GetResources()
        {
            return mResources;
        }
        
        // NOT USED
        override public void SetImageBitmap(Bitmap bm)
        {
            string detailMessage = "setImageBitmap not supported, use: setImageDrawable() " +
                "or setImageResource()";
            throw new Java.Lang.RuntimeException(detailMessage);
        }

        
        // DO NOT USE
        override public void SetImageURI(Uri uri)
        {
            string detailMessage = "setImageURI not supported, use: setImageDrawable() " +
                "or setImageResource()";
            throw new Java.Lang.RuntimeException(detailMessage);
        }
        

        override protected void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            Matrix matrix = ImageMatrix;

            if (matrix != null)
            {
                int numLayers = mLayers.Capacity;
                bool pendingAnimations = false;
                for (int i = 0; i < numLayers; i++)
                {
                    mDrawMatrix.Set(matrix);
                    Layer layer = mLayers[i];
                    if (layer.matrix != null)
                    {
                        mDrawMatrix.PreConcat(layer.matrix);
                    }
                    if (layer.animation == null)
                    {
                        draw(canvas, layer.drawable, mDrawMatrix, 255);
                    }
                    else
                    {
                        Animation a = layer.animation;

                        if (!a.IsInitialized)
                        {
                            Rect bounds = layer.drawable.Bounds;
                            Drawable parentDrawable = Drawable;
                            if (parentDrawable != null)
                            {
                                Rect parentBounds = parentDrawable.Bounds;

                                a.Initialize(bounds.Width(), bounds.Height(), parentBounds.Width(), parentBounds.Height());
                            }
                            else
                            {
                                a.Initialize(bounds.Width(), bounds.Height(), 0, 0);
                            }
                        }

                        long currentTime = AnimationUtils.CurrentAnimationTimeMillis();
                        bool running = a.GetTransformation(currentTime, layer.transformation);
                        if (running)
                        {
                            // animation is running: draw animation frame
                            Matrix animationFrameMatrix = layer.transformation.Matrix;
                            mDrawMatrix.PreConcat(animationFrameMatrix);

                            int alpha = (int)(255 * layer.transformation.Alpha);
                            Console.Out.WriteLine(LOG_TAG, "onDraw ********** [" + i + "], alpha: " + alpha + ", matrix: " + animationFrameMatrix);
                            draw(canvas, layer.drawable, mDrawMatrix, alpha);
                            pendingAnimations = true;
                        }
                        else
                        {
                            // animation ended: set it to null
                            layer.animation = null;
                            draw(canvas, layer.drawable, mDrawMatrix, 255);
                        }
                    }
                }
                if (pendingAnimations)
                {
                    // invalidate if any pending animations
                    Invalidate();
                }
            }
        }

        private void draw(Canvas canvas, Drawable drawable, Matrix matrix, int alpha)
        {
            canvas.Save(SaveFlags.Matrix);
            canvas.Concat(matrix);
            drawable.SetAlpha(alpha);
            drawable.Draw(canvas);
            canvas.Restore();
        }

        public Layer addLayer(Drawable d, Matrix m)
        {
            Layer layer = new Layer(d, m, this);
            mLayers.Add(layer);
            Invalidate();
            return layer;
        }

        public Layer addLayer(Drawable d)
        {
            return addLayer(d, null);
        }

        public Layer addLayer(int idx, Drawable d, Matrix m)
        {
            Layer layer = new Layer(d, m, this);
            mLayers.Insert(idx, layer);
            Invalidate();
            return layer;
        }

        public Layer addLayer(int idx, Drawable d)
        {
            return addLayer(idx, d, null);
        }

        public void removeLayer(Layer layer)
        {
            layer.valid = false;
            mLayers.Remove(layer);
        }

        public void removeAllLayers()
        {
            mLayers.Clear();
        }

        public int getLayersSize()
        {
            return mLayers.Capacity;
        }
        
        public class Layer
        {
            public Drawable drawable { get; set; }
            public Animation animation { get; set; }
            public Transformation transformation { get; set; }
            public Matrix matrix { get; set; }
            public bool valid { get; set; }
            
            public Layer(Drawable d, Matrix m, LayeredImageView liv)
            {
                drawable = d;
                transformation = new Transformation();
                matrix = m;
                valid = true;
                Rect bounds = d.Bounds;
                if (bounds.IsEmpty)
                {
                    if (d is BitmapDrawable) {
                        int right = d.IntrinsicWidth;
                        int bottom = d.IntrinsicHeight;
                        d.Bounds = new Rect (0, 0, right, bottom);
                    } else {
                        string detailMessage = "Drawable bounds are empty.";
                        throw new Java.Lang.RuntimeException(detailMessage);
                    }
                }
                // http://stackoverflow.com/questions/16729169/how-to-maintain-multi-layers-of-imageviews-and-keep-their-aspect-ratio-based-on
                //d.SetCallback(LayeredImageView.this);
                d.SetCallback(liv); 
            }

            public void startLayerAnimation(Animation a)
            {
                try
                {
                    if (!valid)
                    {
                        string detailMessage = "this layer has already been removed";
                        throw new Java.Lang.RuntimeException(detailMessage);
                    }

                    transformation.Clear();
                    animation = a;

                    if (a != null)
                    {
                        a.Start();
                    }

                    // Invalidate();  
                    drawable.InvalidateSelf();
                }
                catch (Java.Lang.RuntimeException ex)
                {
                    Console.Out.WriteLine("Runtime exception at startLayerAnimation: ");
                    Console.Out.WriteLine(ex);

                    throw ex;
                }
            }

            public void stopLayerAnimation(int idx)
            {
                try
                {
                    if (!valid)
                    {
                        string detailMessage = "This layer has already been removed";
                        throw new Java.Lang.RuntimeException(detailMessage);
                    }
                    if (animation != null)
                    {
                        animation = null;
                        // Invalidate();
                        drawable.InvalidateSelf();
                    }
                }
                catch (Java.Lang.RuntimeException ex)
                {
                    Console.Out.WriteLine("Runtime exception at stopLayerAnimation: ");
                    Console.Out.WriteLine(ex);
                    throw ex;
                }

            }

        }
        

        public class Layer
        {
            public Drawable drawable { get; set; }
            public Matrix matrix { get; set; }
            public bool valid { get; set; }
            public FallObject_normal fallObject { get; set; }

            public Layer(Drawable d, Matrix m, LayeredImageView liv)
            {
                drawable = d;
                matrix = m;
                valid = true;
                Rect bounds = d.Bounds;
                if (bounds.IsEmpty)
                {
                    if (d is BitmapDrawable)
                    {
                        int right = d.IntrinsicWidth;
                        int bottom = d.IntrinsicHeight;
                        d.Bounds = new Rect(0, 0, right, bottom);
                    }
                    else
                    {
                        string detailMessage = "Drawable bounds are empty.";
                        throw new Java.Lang.RuntimeException(detailMessage);
                    }
                }
                // http://stackoverflow.com/questions/16729169/how-to-maintain-multi-layers-of-imageviews-and-keep-their-aspect-ratio-based-on
                //d.SetCallback(LayeredImageView.this);
                d.SetCallback(liv);
            }
            
            public void startLayerAnimation(Animation a)
            {
                try
                {
                    if (!valid)
                    {
                        string detailMessage = "this layer has already been removed";
                        throw new Java.Lang.RuntimeException(detailMessage);
                    }

                    transformation.Clear();
                    animation = a;

                    if (a != null)
                    {
                        a.Start();
                    }

                    // Invalidate();  
                    drawable.InvalidateSelf();
                }
                catch (Java.Lang.RuntimeException ex)
                {
                    Console.Out.WriteLine("Runtime exception at startLayerAnimation: ");
                    Console.Out.WriteLine(ex);

                    throw ex;
                }
            }

            public void stopLayerAnimation(int idx)
            {
                try
                {
                    if (!valid)
                    {
                        string detailMessage = "This layer has already been removed";
                        throw new Java.Lang.RuntimeException(detailMessage);
                    }
                    if (animation != null)
                    {
                        animation = null;
                        // Invalidate();
                        drawable.InvalidateSelf();
                    }
                }
                catch (Java.Lang.RuntimeException ex)
                {
                    Console.Out.WriteLine("Runtime exception at stopLayerAnimation: ");
                    Console.Out.WriteLine(ex);
                    throw ex;
                }

            }
            
        }
        private class LayeredImageViewResources : Resources
        {

            public LayeredImageViewResources(Context context) : base(context.Assets, new DisplayMetrics(), null)
            {
                // Resources created.
            }

            override public Drawable GetDrawable(int id)
            {
                try
                {
                    Drawable d;

                    d = base.GetDrawable(id);
                    if (d is BitmapDrawable ) {
                        BitmapDrawable bd = (BitmapDrawable)d;

                        bd.Bitmap.Density = (int)DisplayMetricsDensity.Default;
                        bd.SetTargetDensity((int)DisplayMetricsDensity.Default);
                    }

                    return d;

                }
                catch (NotFoundException ex)
                {
                    Console.Out.WriteLine("Runtime exception at stopLayerAnimation: ");
                    Console.Out.WriteLine(ex);

                    throw ex;
                }
            }
        }
       */
    }
}