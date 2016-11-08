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

namespace ScoreListPeli.Classes
{
    public class Coordinate
    {
        public float x { get; set; }
        public float y { get; set; }

        public Coordinate(float Tx, float Ty)
        {
            x = Tx;
            y = Ty;
        }
    }
}