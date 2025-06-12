using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class PositionComponent
    {
        public Vector2 Position;

        public PositionComponent(Vector2 startPos)
        {
            Position = startPos;
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
