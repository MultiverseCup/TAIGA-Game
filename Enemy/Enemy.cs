using Microsoft.Xna.Framework.Graphics;
using gameProject.Player;
using Microsoft.Xna.Framework;

namespace gameProject.Enemy
{
    public class Enemy
    {
        public Sprite Sprite { get; private set; }
        public int Speed { get; private set; }
        public int Health { get; private set; }
        public Vector2 Position;

        private Player.Player _player;

        public Enemy(Sprite sprite, Vector2 position, int speed, Player.Player player)
        {
            Sprite = sprite;
            Position = position;
            Speed = speed;
            _player = player;
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                Sprite.Width / 2,
                Sprite.Height
            );
        }

        public void Update(GameTime gameTime)
        {
            Move(gameTime);
            Sprite.Update(gameTime);
            Sprite.Position = Sprite.CalculateSpritePosition(Position, Sprite);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        private void Move(GameTime gameTime)
        {
            Vector2 direction = _player.Position - Position;
            direction.Normalize();

            Vector2 velocity = direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += velocity;
        }

        public static void SpawnEnemy(Vector2 position, Enemy enemy)
        {

        }

    }
}
