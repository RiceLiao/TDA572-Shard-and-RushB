/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    public class Grid
    {
        public readonly int x, y;
        public Grid(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator== (Grid b, Grid c)
        {
            if (b.x == c.x && b.y == c.y)
                return true;
            return false;
        }

        public static bool operator!= (Grid b, Grid c)
        {
            if (b.x != c.x || b.y != c.y)
                return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            Grid g = obj as Grid;
            return this.x == g.x && this.y == g.y;
        }

        public override int GetHashCode()
        {
            return (x * 597) ^ (y * 1173);
        }

        public override string ToString()
        {
            return "[" + x + "," + y + "]";
        }
    }
}

