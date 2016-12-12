using System.Collections.Generic;
namespace ScoreListPeli
{
    // Luokka jonka kautta JSON data talletetaan.
    public class HiScoreObj
    {
        public IEnumerable<ScoreObj> HiScores { get; set; }

        public class ScoreObj
        {
            public string Name { get; set; }
            public int Score { get; set; }

            public ScoreObj(string n, int p)
            {
                Name = n;
                Score = p;
            }
        }

    }
}