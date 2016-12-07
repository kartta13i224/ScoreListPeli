namespace ScoreListPeli.Classes
{
    class ObjectBounds
    {
        public int top { get; set; }
        public int left { get; set; }
        public int right { get; set; }
        public int bottom { get; set; }
        public int speed { get; set; }
        public int size { get; set; }

        public int score { get; set; }

        private int speedBoost;
        public ObjectBounds()
        {
            speedBoost = 0;
            speed = 1; // default speed
            size = 50;
        }

        public Coordinate getCoordinate()
        {
            return new Classes.Coordinate(left + size / 2, bottom - size / 2);
        }

        public void increaseSpeed()
        {
            speedBoost++;

            if (speedBoost >= 3)
            {
                speedBoost = 0;
                speed++;
            }
        }
    }
}