namespace ScoreListPeli.Classes
{
    class ObjectBounds
    {
        public int top { get; set; }
        public int left { get; set; }
        public int right { get; set; }
        public int bottom { get; set; }
        public int speed { get; set; }
        public int size_x { get; set; }
        public int size_y { get; set; }
        private int score;

        private bool scoreGiven = false;

        private int speedBoost;
        public ObjectBounds(int maxSpeed)
        {
            int givenSize = ScreenUtils.randonInt(ScreenUtils.GAME_WIDTH / 16, ScreenUtils.GAME_WIDTH / 5);
            speedBoost = 0;
            speed = ScreenUtils.randonInt(1, maxSpeed); // default speed
            size_x = givenSize;
            size_y = givenSize;
            score = 100 / givenSize; // Score is based on size, smaller size = higher score. + It's speed.
            size_x = size_x * (int)ScreenUtils.SCREEN_W_RATIO; // Scale it's size for the device.
            size_y = size_y * (int)ScreenUtils.SCREEN_W_RATIO; // Scale it's size for the device.
        }

        public Coordinate getCoordinate()
        {
            return new Classes.Coordinate(left, top);
        }

        public int getScore()
        {
            scoreGiven = true;
            return score + speed;
        }

        public bool is_score_given()
        {
            return scoreGiven;
        }

        public void increaseSpeed()
        {
            speedBoost++;

            if (speedBoost >= 4 && speed < 15 || speedBoost >= 10 && speed >= 15)
            {
                speedBoost = 0;
                speed++;
            }
        }
    }
}