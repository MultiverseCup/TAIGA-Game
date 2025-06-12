using gameProject.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.UI
{
    public class VictoryScreen
    {
        private SpriteFont _font;
        private GraphicsDevice _graphics;
        private Rectangle _restartButton;
        private Rectangle _exitButton;
        private MouseState _previousMouse;
        public bool RestartRequested { get; private set; }
        public bool ExitRequested { get; private set; }

        public VictoryScreen(SpriteFont font, GraphicsDevice graphics)
        {
            _font = font;
            _graphics = graphics;
            int centerX = graphics.Viewport.Width / 2;

            _restartButton = new Rectangle(centerX - 100, 400, 200, 50);
            _exitButton = new Rectangle(centerX - 100, 470, 200, 50);
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            Point mousePos = mouse.Position;

            if (mouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
            {
                if (_restartButton.Contains(mousePos))
                    RestartRequested = true;
                else if (_exitButton.Contains(mousePos))
                    ExitRequested = true;
            }

            _previousMouse = mouse;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var center = new Vector2(_graphics.Viewport.Width / 2 - 70, 200);
            var victoryText = "VICTORY";
            var textSize = _font.MeasureString(victoryText);
            spriteBatch.DrawString(_font, victoryText, center - textSize / 2, Color.Yellow, default, default, 3, default, default);

            DrawButton(spriteBatch, _restartButton, "RESTART");
            DrawButton(spriteBatch, _exitButton, "EXIT");
        }

        private void DrawButton(SpriteBatch spriteBatch, Rectangle rect, string text)
        {
            spriteBatch.Draw(Textures.pixel, rect, Color.DarkSlateGray);
            var textSize = _font.MeasureString(text);
            Vector2 textPos = new Vector2(rect.X + (rect.Width - textSize.X) / 2, rect.Y + (rect.Height - textSize.Y) / 2);
            spriteBatch.DrawString(_font, text, textPos, Color.White);
        }

        public void Reset()
        {
            RestartRequested = false;
            ExitRequested = false;
        }
    }

}
