using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gameProject.Player;

namespace gameProject.Enemy
{
    public class EnemyManager
    {
        private List<Enemy> _enemies;
        private GraphicsDeviceManager _graphics;
        private Texture2D _enemyTexture;
        private Player.Player _player;

        public EnemyManager(GraphicsDeviceManager graphics, Texture2D enemyTexture, Player.Player player)
        {
            _enemies = new List<Enemy>();
            _graphics = graphics;
            _enemyTexture = enemyTexture;
            _player = player;
        }

        public List<Enemy> Enemies => _enemies;

        public void SpawnEnemies()
        {
            var random = new Random();
            int[] a = [-1, 1];
            int index = random.Next(2);
            int index2 = random.Next(2);
            int neg = a[index];
            int neg2 = a[index2];

            int enemyY = random.Next((int)_player.Position.Y - _graphics.PreferredBackBufferHeight / 2,
                                     (int)_player.Position.Y + _graphics.PreferredBackBufferHeight / 2);

            _enemies.Add(new Enemy(new Sprite(_enemyTexture, 5),
                                   new Vector2(_player.Position.X + neg * (_graphics.PreferredBackBufferWidth / 2), enemyY * neg2),
                                   0, _player));

            int enemyX = random.Next((int)_player.Position.X - _graphics.PreferredBackBufferWidth / 2,
                                     (int)_player.Position.X + _graphics.PreferredBackBufferWidth / 2);

            _enemies.Add(new Enemy(new Sprite(_enemyTexture, 5),
                                   new Vector2(enemyX * neg, _player.Position.Y + neg2 * (_graphics.PreferredBackBufferHeight / 2)),
                                   0, _player));
        }

        public void Update(GameTime gameTime)
        {
            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemy in _enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }
    }

}
