namespace ScoreListPeli.Classes
{
    class ObjectBounds
    {
        public int top { get; set; }
        public int left { get; set; }
        public int right { get; set; }
        public int bottom { get; set; }
        public float speed { get; set; }
        public int size_x { get; set; }
        public int size_y { get; set; }
        private int score;

        private bool willBounce = false;
        private bool scoreGiven = false;

        private int speedBoost;
        public ObjectBounds(int maxSpeed)
        {
            int givenSize = ScreenUtils.randonInt(ScreenUtils.GAME_WIDTH / 16, ScreenUtils.GAME_WIDTH / 5);
            if (1 == ScreenUtils.randonInt(1, 3))
                willBounce = true;

            speedBoost = 0;
            speed = ScreenUtils.randonInt(1, maxSpeed); // default speed
            size_x = givenSize;
            size_y = givenSize;
            score = 100 / givenSize; // Score is based on size, smaller size = higher score. + It's speed.

            // Randomized x starting point.
            left = ScreenUtils.randonInt(ScreenUtils.GAME_WIDTH / 10, ScreenUtils.GAME_WIDTH - ScreenUtils.GAME_WIDTH / 10 - size_x);

            // Set starting coordinates and convert them.
            int[] rectBounds = ScreenUtils.convertBounds(left, 0, left + size_x, size_y);
            left = rectBounds[0];
            top = rectBounds[1];
            right = rectBounds[2];
            bottom = rectBounds[3];

            // Convert size.
            size_x = size_x * (int)ScreenUtils.SCREEN_W_RATIO; // Scale it's size for the device.
            size_y = size_y * (int)ScreenUtils.SCREEN_H_RATIO; // Scale it's size for the device.
        }

        public Coordinate getCoordinate()
        {
            return new Classes.Coordinate(left, top);
        }

        public void bounce()
        {
            if (willBounce)
            {
                if (right < ScreenUtils.GAME_WIDTH * ScreenUtils.SCREEN_W_RATIO - size_x)
                {
                    left++;
                    right++;
                }
                else if (left > 0)
                {
                    left--;
                    right--;
                }
            }
            

        }

        public int getScore()
        {
            scoreGiven = true;
            return score + (int)speed / 5;
        }

        public bool is_score_given()
        {
            return scoreGiven;
        }

        // Moves object down and accelerates it.
        public void increaseSpeed()
        {
            top = top + (int)speed;
            bottom = bottom + (int)speed;
            speedBoost++;
            if (speedBoost >= 3 && speed < 20 * ScreenUtils.SCREEN_H_RATIO || speedBoost >= 10 && speed >= 20 * ScreenUtils.SCREEN_H_RATIO)
            {
                speedBoost = 0;
                speed = speed + ScreenUtils.SCREEN_H_RATIO;
            }
        }
    }
}