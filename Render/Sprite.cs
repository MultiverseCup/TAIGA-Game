using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gameProject.Render
{
    public class Sprite
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; set; }
        public float Scale { get; set; }

        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }

        public int TotalFrames { get; set; } = 1; // кол-во кадров в картинке
        public int CurrentFrame { get; set; } = 0;
        public float FrameTime { get; set; } = 0.1f;
        private float _timeSinceLastFrame = 0f;

        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        public bool IsAnimating { get; set; } = true;

        public int Width => FrameWidth * (int)Scale;
        public int Height => FrameHeight * (int)Scale;

        public Rectangle Rect => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

        public Sprite(Texture2D texture, float scale, int totalFrames = 1)
        {
            Texture = texture;
            Scale = scale;
            TotalFrames = totalFrames;

            FrameWidth = texture.Width / totalFrames;
            FrameHeight = texture.Height;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsAnimating || TotalFrames <= 1) return;

            _timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timeSinceLastFrame >= FrameTime)
            {
                CurrentFrame = (CurrentFrame + 1) % TotalFrames;
                _timeSinceLastFrame = 0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var sourceRect = new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
            spriteBatch.Draw(
            Texture,
            Position,
            sourceRect,
            Color.White,
            0f,
            Vector2.Zero,
            Scale,
            Effects,
            0f
        );
        }

        public static Vector2 CalculateSpritePosition(Vector2 entityPosition, Sprite entitySprite)
        {
            return new Vector2(
                entityPosition.X - entitySprite.Width / 2,
                entityPosition.Y - entitySprite.Height / 2
            );
        }
    }
}
