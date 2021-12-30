using System.Collections.Generic;
using System.Linq;
namespace GameClient
{
    public class BreadthFirst : AlgorithmBase
    {
        private readonly Queue<Node> _q = new Queue<Node>();
        private bool _destinationFound;
        public BreadthFirst() : base()
        {
            _AlgorithmName = "Breadth-First Search";
            // Add the first node to the queue
        }
        public void Initialize(Coord start, Coord end, Grid Grid)
        {
            _Grid = Grid;
            _Closed = new List<Node>();
            _Operations = 0;
            _Id = 1;
            _q.Clear();
            _Origin = start;
            _Destination = end;
            _q.Enqueue(new Node(_Id++, null, _Origin, 0, 0));
        }
        public override SearchDetails GetPathTick()
        {
            // If there are still nodes on the queue and the destination hasn't been found, evaluate the next node.
            if (_q.Count > 0 && !_destinationFound)
            {
                // Get the next node in the queue and check it hasn't already been visited
                _CurrentNode = _q.Dequeue();
                if (AlreadyVisited(_CurrentNode.Coord)) return GetSearchDetails();
                // Add node to those already visited
                _Closed.Add(_CurrentNode);
                _Grid.SetCell(_CurrentNode.Coord.X, _CurrentNode.Coord.Y, Enums.CellType.Closed);
                // Go through the neighbours of this node and add the new ones to the queue
                var neighbours = GetNeighbours(_CurrentNode);
                foreach (var neighbour in neighbours)
                {
                    if (AlreadyVisited(neighbour)) continue;
                    var neighbourNode = new Node(_Id++, _CurrentNode.Id, neighbour.X, neighbour.Y, 0, 0);
                    _q.Enqueue(neighbourNode);
                    _Grid.SetCell(neighbour, Enums.CellType.Open);
                    if (!CoordsMatch(neighbour, _Destination)) continue;
                    // Check if we've found the destination
                    _Closed.Add(neighbourNode);
                    _destinationFound = true;
                }
            }
            else
            {
                // The path has been found, reconstruct it be following back via the parentId's and reverse the order so it goes from A to B
                _Path = new List<Coord>();
                var step = _Closed.First(x => CoordsMatch(x.Coord, _Destination));
                while (!CoordsMatch(step.Coord, _Origin))
                {
                    _Path.Add(step.Coord);
                    step = _Closed.First(x => x.Id == step.ParentId);
                }
                _Path.Add(_Origin);
                _Path.Reverse();
            }
            return GetSearchDetails();
        }
        private bool AlreadyVisited(Coord coord)
        {
            return _Closed.Any(x => CoordsMatch(x.Coord, coord));
        }
        protected override SearchDetails GetSearchDetails()
        {
            return new SearchDetails
            {
                Path = _Path?.ToArray(),
                PathCost = GetPathCost(),
                LastNode = _CurrentNode,
                DistanceOfCurrentNode = _CurrentNode == null ? 0 : GetManhattenDistance(_CurrentNode.Coord, _Destination),
                OpenListSize = _q.Count,
                ClosedListSize = _Closed.Count,
                UnexploredListSize = _Grid.GetCountOfType(Enums.CellType.Empty),
                Operations = _Operations++
            };
        }
    }
}
