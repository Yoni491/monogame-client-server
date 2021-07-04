namespace GameClient
{
    using System.Collections.Generic;
    using System.Linq;

    public class AStar : AlgorithmBase
    {
        private readonly List<Node> _openList = new List<Node>();
        private readonly List<Coord> _neighbours;

        public AStar() : base()
        {
            _AlgorithmName = "A*";
            _neighbours = new List<Coord>();

            // Put the origin on the open list

        }
        public void Initialize(Coord start, Coord end, Grid grid)
        {
            _Grid = grid;
            _Closed = new List<Node>();
            _Operations = 0;
            _Id = 1;
            _openList.Clear();
            _Origin = start;
            _Destination = end;
            _openList.Add(new Node(_Id++, null, _Origin, 0, GetH(_Origin, _Destination)));
        }

        public override SearchDetails GetPathTick()
        {
            if (_CurrentNode == null)
            {
                if (!_openList.Any()) return GetSearchDetails();

                // Take the current node off the open list to be examined
                _CurrentNode = _openList.OrderBy(x => x.F).ThenBy(x => x.H).First();

                // Move it to the closed list so it doesn't get examined again
                _openList.Remove(_CurrentNode);
                _Closed.Add(_CurrentNode);
                _Grid.SetCell(_CurrentNode.Coord, Enums.CellType.Closed);

                _neighbours.AddRange(GetNeighbours(_CurrentNode));
            }

            if (_neighbours.Any())
            {
                _Grid.SetCell(_CurrentNode.Coord, Enums.CellType.Current);

                var thisNeighbour = _neighbours.First();
                _neighbours.Remove(thisNeighbour);

                // If the neighbour is the destination
                if (CoordsMatch(thisNeighbour, _Destination))
                {
                    // Construct the path by tracing back through the closed list until there are no more parent id references
                    _Path = new List<Coord> { thisNeighbour };
                    int? parentId = _CurrentNode.Id;
                    while (parentId.HasValue)
                    {
                        var nextNode = _Closed.First(x => x.Id == parentId);
                        _Path.Add(nextNode.Coord);
                        parentId = nextNode.ParentId;
                    }

                    // Reorder the path to be from origin to destination and return
                    _Path.Reverse();

                    return GetSearchDetails();
                }

                // Get the cost of the current node plus the extra step weight and heuristic
                var hFromHere = GetH(thisNeighbour, _Destination);
                var cellWeight = _Grid.GetCell(thisNeighbour.X, thisNeighbour.Y).Weight;
                var neighbourCost = _CurrentNode.G + cellWeight + hFromHere;

                // Check if the node is on the open list already and if it has a higher cost path
                var openListItem = _openList.FirstOrDefault(x => x.Id == GetExistingNode(true, thisNeighbour));
                if (openListItem != null && openListItem.F > neighbourCost)
                {
                    // Repoint the openlist node to use this lower cost path
                    openListItem.F = neighbourCost;
                    openListItem.ParentId = _CurrentNode.Id;
                }

                // Check if the node is on the closed list already and if it has a higher cost path
                var closedListItem = _Closed.FirstOrDefault(x => x.Id == GetExistingNode(false, thisNeighbour));
                if (closedListItem != null && closedListItem.F > neighbourCost)
                {
                    // Repoint the closedlist node to use this lower cost path
                    closedListItem.F = neighbourCost;
                    closedListItem.ParentId = _CurrentNode.Id;
                }

                // If the neighbour node isn't on the open or closed list, add it
                if (openListItem != null || closedListItem != null) return GetSearchDetails();
                _openList.Add(new Node(_Id++, _CurrentNode.Id, thisNeighbour, _CurrentNode.G + cellWeight, hFromHere));
                _Grid.SetCell(thisNeighbour.X, thisNeighbour.Y, Enums.CellType.Open);
            }
            else
            {
                _Grid.SetCell(_CurrentNode.Coord, Enums.CellType.Closed);
                _CurrentNode = null;
                return GetPathTick();
            }

            return GetSearchDetails();
        }

        private static int GetH(Coord origin, Coord destination)
        {
            return GetManhattenDistance(origin, destination);
        }

        private int? GetExistingNode(bool checkOpenList, Coord coordToCheck)
        {
            return checkOpenList ? _openList.FirstOrDefault(x => CoordsMatch(x.Coord, coordToCheck))?.Id : _Closed.FirstOrDefault(x => CoordsMatch(x.Coord, coordToCheck))?.Id;
        }

        protected override SearchDetails GetSearchDetails()
        {
            return new SearchDetails
            {
                Path = _Path?.ToArray(),
                PathCost = GetPathCost(),
                LastNode = _CurrentNode,
                DistanceOfCurrentNode = _CurrentNode == null ? 0 : GetH(_CurrentNode.Coord, _Destination),
                OpenListSize = _openList.Count,
                ClosedListSize = _Closed.Count,
                UnexploredListSize = _Grid.GetCountOfType(Enums.CellType.Empty),
                Operations = _Operations++
            };
        }
    }
}
