using gameProject.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Utils
{
    public static class AStarPathfinder
    {
        private class Node
        {
            public Point Position;
            public float G; // длина от старта до текущей клетки
            public float H => Math.Abs(Position.X - Goal.X) + Math.Abs(Position.Y - Goal.Y); // эвристика (манхэттенское расстояние) 
            public float F => G + H;
            public Node Parent;
            public static Point Goal;
        }

        public static List<Point> FindPath(MazeGenerator maze, Point start, Point goal)
        {
            Node.Goal = goal;
            var open = new List<Node>();
            var closed = new HashSet<Point>();
            open.Add(new Node { Position = start, G = 0 });

            while (open.Count > 0)
            {
                var current = open.OrderBy(n => n.F).First();
                if (current.Position == goal)
                    return ReconstructPath(current);

                open.Remove(current);
                closed.Add(current.Position);

                foreach (var dir in new[] { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) })
                {
                    var next = current.Position + dir;
                    if (!IsWalkable(maze, next) || closed.Contains(next))
                        continue;

                    var g = current.G + 1;
                    var existing = open.FirstOrDefault(n => n.Position == next);
                    if (existing == null)
                    {
                        open.Add(new Node { Position = next, G = g, Parent = current });
                    }
                    else if (g < existing.G)
                    {
                        existing.G = g;
                        existing.Parent = current;
                    }
                }
            }
            return new List<Point>();
        }

        private static List<Point> ReconstructPath(Node node)
        {
            var path = new List<Point>();
            while (node != null)
            {
                path.Add(node.Position);
                node = node.Parent;
            }
            path.Reverse();
            return path;
        }

        private static bool IsWalkable(MazeGenerator maze, Point p)
        {
            return p.X >= 0 && p.Y >= 0 && p.X < maze.Width && p.Y < maze.Height &&
                (maze.GetCell(p.X, p.Y) == MazeCell.Floor || maze.GetCell(p.X, p.Y) == MazeCell.Collectible);
        }
    }
}
