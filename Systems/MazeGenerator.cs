using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Systems
{
    public class MazeGenerator
    {
        public int Width { get; }
        public int Height { get; }
        private MazeCell[,] _cells;
        private Random _random = new();
        public Point? ExitCell { get; set; }
        public List<Point> Collectibles { get; private set; } = new();
        public int RequiredCollectibles { get; private set; } = 10;


        public MazeGenerator(int width, int height)
        {
            Width = width;
            Height = height;
            _cells = new MazeCell[width, height];
        }

        public void Generate()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    _cells[x, y] = MazeCell.Wall;

            CarvePassage(1, 1);
            AddExtraPassages(0.2f);
            PlaceCollectibles(RequiredCollectibles);
        }

        private void CarvePassage(int x, int y)
        {
            _cells[x, y] = MazeCell.Floor;
            var directions = new[] { (2, 0), (-2, 0), (0, 2), (0, -2) }.OrderBy(_ => _random.Next()).ToList();

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx, ny = y + dy;
                if (IsInBounds(nx, ny) && _cells[nx, ny] == MazeCell.Wall)
                {
                    _cells[x + dx / 2, y + dy / 2] = MazeCell.Floor;
                    CarvePassage(nx, ny);
                }
            }
        }

        private void AddExtraPassages(float chance)
        {
            for (int x = 1; x < Width - 1; x++)
            {
                for (int y = 1; y < Height - 1; y++)
                {
                    if (_cells[x, y] != MazeCell.Wall) continue;

                    bool horizontal = _cells[x - 1, y] == MazeCell.Floor && _cells[x + 1, y] == MazeCell.Floor;
                    bool vertical = _cells[x, y - 1] == MazeCell.Floor && _cells[x, y + 1] == MazeCell.Floor;

                    if ((horizontal || vertical) && _random.NextDouble() < chance)
                    {
                        _cells[x, y] = MazeCell.Floor;
                    }
                }
            }
        }

        private void PlaceCollectibles(int count)
        {
            var floorCells = GetAllFloorCells().OrderBy(_ => _random.Next()).ToList();
            for (int i = 0; i < count; i++)
            {
                var p = floorCells[i];
                _cells[p.X, p.Y] = MazeCell.Collectible;
                Collectibles.Add(p);
            }
        }

        private bool IsInBounds(int x, int y) => x > 0 && y > 0 && x < Width - 1 && y < Height - 1;

        public MazeCell GetCell(int x, int y) => _cells[x, y];

        public void SetCell(Point point, MazeCell cell)
        {
            _cells[point.X, point.Y] = cell;
            if (cell != MazeCell.Collectible)
                Collectibles.Remove(point);
        }

        public Point GetRandomFloorCell()
        {
            List<Point> floorCells = new();
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (_cells[x, y] == MazeCell.Floor)
                        floorCells.Add(new Point(x, y));
            return floorCells[_random.Next(floorCells.Count)];
        }

        public Point GetNearestFloorToCenter() // тут используем поиск в ширину
        {
            var center = new Point(Width / 2, Height / 2);

            if (_cells[center.X, center.Y] == MazeCell.Floor)
                return center;

            var visited = new HashSet<Point>();
            var queue = new Queue<Point>();
            queue.Enqueue(center);
            visited.Add(center);

            var directions = new[] { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) };

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var dir in directions)
                {
                    var next = new Point(current.X + dir.X, current.Y + dir.Y);
                    if (!IsInBounds(next.X, next.Y) || visited.Contains(next))
                        continue;

                    if (_cells[next.X, next.Y] == MazeCell.Floor)
                        return next;

                    queue.Enqueue(next);
                    visited.Add(next);
                }
            }

            return GetRandomFloorCell();
        }

        public Point GetFurthestFloorCell(Point from)
        {
            Point furthest = from;
            int maxDist = -1;
            foreach (var cell in GetAllFloorCells())
            {
                int dist = Math.Abs(cell.X - from.X) + Math.Abs(cell.Y - from.Y);
                if (dist > maxDist)
                {
                    maxDist = dist;
                    furthest = cell;
                }
            }
            return furthest;
        }

        public IEnumerable<Point> GetAllFloorCells()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (_cells[x, y] == MazeCell.Floor)
                        yield return new Point(x, y);
        }

        public void OpenExit(Point from)
        {
            ExitCell = GetFurthestFloorCell(from);
        }
    }
}
