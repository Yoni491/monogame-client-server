namespace GameClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class AlgorithmBase
    {
        protected Grid _Grid;
        protected List<Node> _Closed;
        protected List<Coord> _Path;
        protected Coord _Origin;
        protected Coord _Destination;
        protected int _Id;
        protected Node _CurrentNode;
        protected int _Operations;
        public string _AlgorithmName;


        protected AlgorithmBase()
        {

        }

        public abstract SearchDetails GetPathTick();

        /// <summary>
        /// Find the coords that are above, below, left, and right of the current cell, assuming they are valid
        /// </summary>
        /// <param name="current"></param>
        /// <returns>The valid coords around the current cell</returns>
        protected virtual IEnumerable<Coord> GetNeighbours(Node current)
        {
            var neighbours = new List<Cell>
            {
                _Grid.GetCell(current.Coord.X - 1, current.Coord.Y),
                _Grid.GetCell(current.Coord.X + 1, current.Coord.Y),
                _Grid.GetCell(current.Coord.X, current.Coord.Y - 1),
                _Grid.GetCell(current.Coord.X, current.Coord.Y + 1)
            };

            return neighbours.Where(x => x.Type != Enums.CellType.Invalid && x.Type != Enums.CellType.Solid).Select(x => x.Coord).ToArray();
        }

        protected abstract SearchDetails GetSearchDetails();

        protected static bool CoordsMatch(Coord a, Coord b) => a.X == b.X && a.Y == b.Y;

        /// <summary>
        /// Get the total blocks horizontally and vertically from one coord to another
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <returns>Distance in blocks</returns>
        protected static int GetManhattenDistance(Coord origin, Coord destination)
        {
            return Math.Abs(origin.X - destination.X) + Math.Abs(origin.Y - destination.Y);
        }

        /// <summary>
        /// Get the cost of the path between A and B
        /// </summary>
        /// <returns>Cost of the path or 0 if no path has been found</returns>
        protected int GetPathCost()
        {
            if (_Path == null) return 0;

            var cost = 0;
            foreach (var step in _Path)
                cost += _Grid.GetCell(step.X, step.Y).Weight;

            return cost;
        }
    }
}
