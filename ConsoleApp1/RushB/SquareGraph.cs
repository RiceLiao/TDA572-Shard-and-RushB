/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

using System.Collections.Generic;

namespace RushB
{
    public class SquareGraph : WeightedGraph<Grid>
    {

        public static readonly Grid[] dirs = new[]
        {
            new Grid(1, 0),
            new Grid(0, -1),
            new Grid(-1, 0),
            new Grid(0, 1)
        };

        public int width, height;
        public HashSet<Grid> walls = new HashSet<Grid>();
        public HashSet<Grid> forests = new HashSet<Grid>();

        public SquareGraph(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public bool InBounds(Grid id)
        {
            return 0 <= id.x && id.x < width
                && 0 <= id.y && id.y < height;
        }

        public bool Passable(Grid id)
        {
            return !walls.Contains(id);
        }

        public double Cost(Grid a, Grid b)
        {
            return forests.Contains(b) ? 5 : 1;
        }

        public IEnumerable<Grid> Neighbors(Grid id)
        {
            foreach (var dir in dirs)
            {
                Grid next = new Grid(id.x + dir.x, id.y + dir.y);
                if (InBounds(next) && Passable(next))
                {
                    yield return next;
                }
            }
        }

        public IEnumerable<Grid> FullNeighbors(Grid id)
        {
            foreach (var dir in dirs)
            {
                Grid next = new Grid(id.x + dir.x, id.y + dir.y);
                if (InBounds(next))
                {
                    yield return next;
                }
            }
        }
    }
}

