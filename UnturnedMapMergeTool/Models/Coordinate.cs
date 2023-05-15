namespace UnturnedMapMergeTool.Models
{
    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return $"[X: {X}, Y: {Y}]";
        }

        public override bool Equals(object obj)
        {
            if (obj is Coordinate coordinate)
            {
                return X == coordinate.X && Y == coordinate.Y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
