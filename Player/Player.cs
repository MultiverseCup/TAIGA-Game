using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace gameProject.Player
{
    public class Player
    {
        public Vector2 Position { get; private set; }
        public float Speed { get; set; }
        public Sprite Sprite { get; private set; }

        public Player(Sprite sprite, Vector2 position, float speed)
        {
            Position = position;
            Speed = speed;
            Sprite = sprite;
            Sprite.Position = Sprite.CalculateSpritePosition(Position, Sprite);
        }

        public void Update(GameTime gameTime)
        {

            MovePlayer(gameTime);
            Sprite.Update(gameTime);
            Sprite.Position = Sprite.CalculateSpritePosition(Position, Sprite);
        }

        private void MovePlayer(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            Vector2 movement = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.W))
                movement.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S))
                movement.Y += 1;
            if (keyboardState.IsKeyDown(Keys.A))
                movement.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D))
                movement.X += 1;

            if (movement != Vector2.Zero)
                movement.Normalize();

            Position += movement * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
    }

}

