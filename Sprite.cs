using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace gameProject
{
    public class Sprite
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; set; }
        public float Scale { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Rectangle Rect
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }


        public Sprite(Texture2D texture, float scale)
        {
            Texture = texture;
            Scale = scale;
            Width = Texture.Width * (int)Scale;
            Height = Texture.Height * (int)Scale;
        }

        public Sprite(Texture2D texture, float scale, Vector2 position)
        {
            Texture = texture;
            Scale = scale;
            Position = position;
            Width = Texture.Width * (int)Scale;
            Height = Texture.Height * (int)Scale;
        }

        
        public virtual void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rect, Color.White);
        }

        public static Vector2 CalculateSpritePosition(Vector2 entityPosition, Sprite entitySprite)
        {
            return new Vector2(entityPosition.X - entitySprite.Width / 2, entityPosition.Y - entitySprite.Width / 2);
        }
    }
}
 