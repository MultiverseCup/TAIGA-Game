using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace gameProject.UI
{
    public class MainMenu
    {
        private SpriteFont _font;
        private Texture2D _buttonTexture;
        private Rectangle _buttonBounds;
        private MouseState _prevMouse;
        private string _buttonText = "START";
        private Texture2D _logoTexture;

        private Rectangle _fullscreenButton;
        private string _fullscreenText = "FULLSCREEN";
        private GraphicsDeviceManager _graphics;

        private List<string> _startPhrases = new List<string> { 
        "уйти из зоопарка",
        "победить пластмассовый мир",
        "выползти из груши",
        "убить в себе государство",
        "поблагодарить за хлеб и за соль",
        "ходить по лесу",
        "продолжить безнадёжно и безвременно спать",
        "переключить на чёрно-белый режим",
        "накормить толпу",
        "пожалеть беззвучными словами",
        "увидеть Солнце",
        "закинуть за спину язык",
        "прищемить добровольные пальцы",
        "разбить окно",
        "остаться таким же, как был",
        "разложиться на плесень и на липовый мёд",
        "не заметить потери бойца",
        "взять шинель",
        "идти в тишине",
        "выбрить виски",
        "уйти в тайгу",
        "ненавидеть красный цвет",
        "убегать с мишенью на спине",
        "записать 27 альбомов"
        };

        private string _currentPhrase = "уйти в тайгу";
        private double _phraseTimer = 0;
        private Random _random = new Random();



        public MainMenu(SpriteFont font, Texture2D logoTexture, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            _font = font;
            _graphics = graphics;
            _buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White });
            _logoTexture = logoTexture;

            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;
            int buttonWidth = 300;
            int buttonHeight = 60;

            _buttonBounds = new Rectangle(
                (screenWidth - buttonWidth) / 2,
                (screenHeight - buttonHeight) / 2 + 100,
                buttonWidth,
                buttonHeight
            );

            _fullscreenButton = new Rectangle(
                (screenWidth - buttonWidth) / 2,
                _buttonBounds.Bottom + 20,
                buttonWidth,
                buttonHeight
            );
        }


        public bool StartGameRequested { get; private set; }

        public void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            if (_buttonBounds.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
            {
                StartGameRequested = true;
            }
            else if (_fullscreenButton.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
            {
                ToggleFullscreen();
            }

            _phraseTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_phraseTimer >= 1)
            {
                _currentPhrase = _startPhrases[_random.Next(_startPhrases.Count)];
                _phraseTimer = 0;
            }

            _prevMouse = mouse;
        }



        public void Draw(SpriteBatch spriteBatch)
        {

            //лого
            if (_logoTexture != null)
            {
                int screenWidth = spriteBatch.GraphicsDevice.Viewport.Width;
                float scale = 0.5f;
                Vector2 logoPosition = new Vector2(
                    (screenWidth - _logoTexture.Width * scale) / 2f,
                    30
                );

                spriteBatch.Draw(_logoTexture, logoPosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }

            // кнопка start
            spriteBatch.Draw(_buttonTexture, _buttonBounds, Color.ForestGreen);
            Vector2 textSize = _font.MeasureString(_buttonText);
            Vector2 textPosition = new Vector2(
                _buttonBounds.X + (_buttonBounds.Width - textSize.X) / 2,
                _buttonBounds.Y + (_buttonBounds.Height - textSize.Y) / 2
            );
            spriteBatch.DrawString(_font, _buttonText, textPosition, Color.White);

            // кнопка fullscreen
            spriteBatch.Draw(_buttonTexture, _fullscreenButton, Color.DarkGreen);
            Vector2 fsSize = _font.MeasureString(_fullscreenText);
            Vector2 fsPosition = new Vector2(
                _fullscreenButton.X + (_fullscreenButton.Width - fsSize.X) / 2,
                _fullscreenButton.Y + (_fullscreenButton.Height - fsSize.Y) / 2
            );
            spriteBatch.DrawString(_font, _fullscreenText, fsPosition, Color.White);


            // фраза
            if (!string.IsNullOrEmpty(_currentPhrase))
            {
                string tipText = $"Нажмите START, чтобы {_currentPhrase}";
                Vector2 tipSize = _font.MeasureString(tipText);
                int screenWidth = spriteBatch.GraphicsDevice.Viewport.Width;
                int screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;
                Vector2 tipPosition = new Vector2((screenWidth - tipSize.X) / 2f, screenHeight - tipSize.Y - 20);

                spriteBatch.DrawString(_font, tipText, tipPosition, Color.LightYellow);
            }
        }

        private void ToggleFullscreen()
        {
            _graphics.IsFullScreen = !_graphics.IsFullScreen;
            _graphics.ApplyChanges();
        }

    }
}
