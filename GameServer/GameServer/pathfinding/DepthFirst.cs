using System.Collections.Generic;
using System.Linq;
namespace GameServer
{

    public class DepthFirst : AlgorithmBase
    {
        readonly Stack<Node> _stack = new Stack<Node>();

        public DepthFirst() : base ()
        {
            _AlgorithmName = "Depth-First Search";

            // Add the first node to the stack
        }
        public void Initialize(Coord start, Coord end, Grid Grid)
        {
            _Grid = Grid;
            _Closed = new List<Node>();
            _Operations = 0;
            _Id = 1;
            _stack.Clear();
            _Origin = start;
            _Destination = end;
            _stack.Push(new Node(_Id++, null, _Origin, 0, 0));
        }

        public override SearchDetails GetPathTick()
        {
            // Check the next node on the stack to see if it is the _Destination
            _CurrentNode = _stack.Peek();
            if (CoordsMatch(_CurrentNode.Coord, _Destination))
            {
                // All the items on the stack will be the _Path so add them and reverse the order
                _Path = new List<Coord>();
                foreach (var item in _stack)
                    _Path.Add(item.Coord);

                _Path.Reverse();

                return GetSearchDetails();
            }

            // Get all the neighbours that haven't been visited
            var neighbours = GetNeighbours(_CurrentNode).Where(x => !AlreadyVisited(new Coord(x.X, x.Y))).ToArray();
            if (neighbours.Any())
            {
                foreach (var neighbour in neighbours)
                    _Grid.SetCell(neighbour.X, neighbour.Y, Enums.CellType.Open);

                // Take this neighbour and add it the stack
                var next = neighbours.First();
                var newNode = new Node(_Id++, null, next.X, next.Y, 0, 0);
                _stack.Push(newNode);
                _Grid.SetCell(newNode.Coord.X, newNode.Coord.Y, Enums.CellType.Current);
            }
            else
            {
                // Remove this unused node from the stack and add it to the closed list
                var abandonedCell = _stack.Pop();
                _Grid.SetCell(abandonedCell.Coord.X, abandonedCell.Coord.Y, Enums.CellType.Closed);
                _Closed.Add(abandonedCell);
            }

            return GetSearchDetails();
        }

        private bool AlreadyVisited(Coord coord)
        {
            return _stack.Any(x => CoordsMatch(x.Coord, coord)) || _Closed.Any(x => CoordsMatch(x.Coord, coord));
        }

        protected override SearchDetails GetSearchDetails()
        {
            return new SearchDetails
            {
                Path = _Path?.ToArray(),
                PathCost = GetPathCost(),
                LastNode = _CurrentNode,
                DistanceOfCurrentNode = _CurrentNode == null ? 0 : GetManhattenDistance(_CurrentNode.Coord, _Destination),
                OpenListSize = _stack.Count,
                ClosedListSize = _Closed.Count,
                UnexploredListSize = _Grid.GetCountOfType(Enums.CellType.Empty),
                Operations = _Operations++
            };
        }
    }
}
