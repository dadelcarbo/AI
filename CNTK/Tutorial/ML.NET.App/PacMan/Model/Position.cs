namespace ML.NET.App.PacMan.Model
{
    public struct Position
    {
        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public Position(Position position)
        {
            this.X = position.X;
            this.Y = position.Y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public static bool operator ==(Position first, Position second)
        {
            return (first.X == second.X) && (first.Y == second.Y);
        }

        public static bool operator !=(Position first, Position second)
        {
            return (first.X != second.X) && (first.Y != second.Y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Position))
            {
                return false;
            }

            var other = (Position)obj;
            return this == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
