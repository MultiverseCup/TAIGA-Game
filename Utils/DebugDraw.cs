using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Utils
{
    public static class DebugDraw
    {
        private static Texture2D _pixel;
        private static int _thickness = 2;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, _thickness), color); //верх
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y + rect.Height - _thickness, rect.Width, _thickness), color);//низ
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, _thickness, rect.Height), color); //лево
            spriteBatch.Draw(_pixel, new Rectangle(rect.X + rect.Width - _thickness, rect.Y, _thickness, rect.Height), color); //право
        }

        public static void FillRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            spriteBatch.Draw(_pixel, rect, color);
        }
    }
}
