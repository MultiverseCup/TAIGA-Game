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
        private float _spawnCooldown = 2f; // интервал между спавнами (в секундах)
        private float _timeSinceLastSpawn = 0f;

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
            int randomizeSpawnSide = a[index];
            int randomizeSpawnSide2 = a[index2];
            int enemySpeed = 200;

            int enemyY = random.Next((int)_player.Position.Y - _graphics.PreferredBackBufferHeight / 2,
                                     (int)_player.Position.Y + _graphics.PreferredBackBufferHeight / 2);

            _enemies.Add(new Enemy(new Sprite(_enemyTexture, 5),
                                   new Vector2(_player.Position.X + randomizeSpawnSide * (_graphics.PreferredBackBufferWidth / 2), enemyY * randomizeSpawnSide2),
                                   enemySpeed, _player));

            int enemyX = random.Next((int)_player.Position.X - _graphics.PreferredBackBufferWidth / 2,
                                     (int)_player.Position.X + _graphics.PreferredBackBufferWidth / 2);

            _enemies.Add(new Enemy(new Sprite(_enemyTexture, 5),
                                   new Vector2(enemyX * randomizeSpawnSide, _player.Position.Y + randomizeSpawnSide2 * (_graphics.PreferredBackBufferHeight / 2)),
                                   enemySpeed, _player));
        }

        public void Update(GameTime gameTime)
        {

            _timeSinceLastSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timeSinceLastSpawn >= _spawnCooldown)
            {
                SpawnEnemies();
                _timeSinceLastSpawn = 0f;
            }

            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime);
            }

            for (int i = 0; i < _enemies.Count; i++)
            {
                for (int j = i + 1; j < _enemies.Count; j++)
                {
                    var enemyA = _enemies[i];
                    var enemyB = _enemies[j];

                    var rectA = enemyA.GetBounds();
                    var rectB = enemyB.GetBounds();

                    Utils.Collision.ResolveRectangleCollision(ref enemyA.Position, rectA, ref enemyB.Position, rectB);
                }
            }
            foreach(var enemy in _enemies)
            {
                var enemyRect = enemy.GetBounds();
                var playerRect = _player.GetBounds();
                Utils.Collision.ResolveRectangleCollision(ref _player.Position, playerRect, ref enemy.Position, enemyRect);
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
