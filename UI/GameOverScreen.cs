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
    public class GameOverScreen
    {
        private SpriteFont _font;
        private Texture2D _buttonTexture;
        private Rectangle _restartButtonRect;
        private GraphicsDevice _graphics;

        public bool RestartRequested { get; private set; }

        public GameOverScreen(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            _font = font;
            _buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White });
            _graphics = graphicsDevice;

            _restartButtonRect = new Rectangle(700, 500, 200, 60);
        }

        public void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            var mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouse.LeftButton == ButtonState.Pressed && _restartButtonRect.Intersects(mouseRect))
            {
                RestartRequested = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var screenCenter = new Vector2(_graphics.Viewport.Width / 2 - 90, 200);

            var text = "GAME OVER";
            var textSize = _font.MeasureString(text);
            var textPos = screenCenter - textSize / 2;

            spriteBatch.DrawString(_font, text, textPos, Color.Red, default, default, 3, default, default);

            // Restart Button
            spriteBatch.Draw(_buttonTexture, _restartButtonRect, Color.Gray);
            var btnText = "RESTART";
            var btnTextSize = _font.MeasureString(btnText);
            var btnTextPos = new Vector2(
                _restartButtonRect.X + (_restartButtonRect.Width - btnTextSize.X) / 2,
                _restartButtonRect.Y + (_restartButtonRect.Height - btnTextSize.Y) / 2
            );
            spriteBatch.DrawString(_font, btnText, btnTextPos, Color.White);
        }

        public void Reset() => RestartRequested = false;
    }

}
