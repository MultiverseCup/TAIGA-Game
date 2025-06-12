using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class GridPositionComponent
    {
        public Point Position { get; set; }
        public static int CellSize = 64;

        public GridPositionComponent(Point initialPosition)
        {
            Position = initialPosition;
        }
    }
}
