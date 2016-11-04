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
    public class ScoreAdapter : BaseAdapter<HiScoreObj.ScoreObj>
    {
        Context context;
        HiScoreObj.ScoreObj[] data;
        private static LayoutInflater inflater = null;

        public ScoreAdapter(Context context, HiScoreObj.ScoreObj[] data)
        {
            // Connect the context, data and inflater with each other.
            this.context = context;
            this.data = data;
            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View vi = convertView;
            if (vi == null)
            {
                vi = inflater.Inflate(Resource.Layout.item_score_row, null);
            }
            TextView nick = (TextView)vi.FindViewById(Resource.Id.nick);
            TextView points = (TextView)vi.FindViewById(Resource.Id.points);

            nick.Text = data[position].nick;
            points.Text = data[position].points.ToString();

            return vi;
        }

        public override HiScoreObj.ScoreObj this[int position]
        {
            get { return data[position]; }
        }

        public override int Count
        {
            get { return data.Length; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}