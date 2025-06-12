using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class MazePlayerRenderComponent
    {
        private Texture2D _texture;
        private int _cellSize;

        public MazePlayerRenderComponent(Texture2D texture, int cellSize = 64)
        {
            _texture = texture;
            _cellSize = cellSize;
        }

        public void Draw(SpriteBatch spriteBatch, Point position)
        {
            var rect = new Rectangle(position.X * _cellSize, position.Y * _cellSize, _cellSize, _cellSize);
            spriteBatch.Draw(_texture, rect, Color.White);
        }
    }
}
