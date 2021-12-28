namespace GameClient
{
    using System;
    using System.Linq;
    using static Enums;

    public class Grid
    {
        public readonly Cell[,] _grid;
        public int _horizontalCells, _verticalCells;
        public Grid(int horizontalCells, int verticalCells)
        {
            _horizontalCells = horizontalCells;
            _verticalCells = verticalCells;
            _grid = new Cell[horizontalCells, verticalCells];
            for (var x = 0; x < _grid.GetLength(0); x++)
            {
                for (var y = 0; y < _grid.GetLength(1); y++)
                {
                    SetCell(x, y, CellType.Empty);
                }
            }

            //SetStartAndEnd();
        }

        public Cell GetCell(int x, int y)
        {
            if (x > _grid.GetLength(0) - 1 || x < 0 || y > _grid.GetLength(1) - 1 || y < 0) return new Cell { Coord = new Coord(-1, -1), Type = CellType.Invalid };

            return _grid[x, y];
        }

        public Cell GetStart()
        {
            return _grid.Cast<Cell>().FirstOrDefault(cell => cell.Type == CellType.A);
        }

        public Cell GetEnd()
        {
            return _grid.Cast<Cell>().FirstOrDefault(cell => cell.Type == CellType.B);
        }

        public void SetCell(int x, int y, CellType type)
        {
            _grid[x, y] = new Cell
            {
                Coord = new Coord(x, y),
                Type = type,
                Weight = GetCell(x, y)?.Weight ?? 0
            };
        }
        public void SetCell(int x, int y, CellType type, int weight)
        {
            _grid[x, y] = new Cell
            {
                Coord = new Coord(x, y),
                Type = type,
                Weight = GetCell(x, y).Weight == 0 ? 0 : weight
            };
        }

        public void SetCell(Coord coord, CellType type)
        {
            SetCell(coord.X, coord.Y, type);
        }

        public int GetCountOfType(CellType type)
        {
            var total = 0;
            foreach (var cell in _grid)
            {
                total += cell.Type == type ? 1 : 0;
            }

            return total;
        }

        public int GetTraversableCells()
        {
            return GetCountOfType(CellType.Open) + GetCountOfType(CellType.A) + GetCountOfType(CellType.B);
        }

        public void SetStartAndEnd(Coord start, Coord end)
        {
            _grid[start.X, start.Y] = new Cell
            {
                Coord = new Coord(start.X, start.Y),
                Type = CellType.A
            };
            _grid[end.X, end.Y] = new Cell
            {
                Coord = new Coord(_grid.GetLength(0) - 1, _grid.GetLength(1) - 1),
                Type = CellType.B
            };
        }
        public Grid GetGridCopy()
        {
            Grid grid = new Grid(_horizontalCells, _verticalCells);
            for (var x = 0; x < _grid.GetLength(0); x++)
            {
                for (var y = 0; y < _grid.GetLength(1); y++)
                {
                    grid.SetCell(x, y, _grid[x, y].Type);
                }
            }
            return grid;
        }
    }
}
