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
    // Luokka jonka kautta JSON data talletetaan.
    public class HiScoreObj
    {
        public IEnumerable<ScoreObj> HiScores { get; set; }

        public class ScoreObj
        {
            public string nick { get; set; }
            public int points { get; set; }

            public ScoreObj(string n, int p)
            {
                nick = n;
                points = p;
            }
        }

    }
}