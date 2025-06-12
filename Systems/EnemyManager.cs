using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gameProject.Core;
using System.Diagnostics;
using gameProject.Utils;
using gameProject.Render;
using gameProject.Components;
using gameProject.Entities;

namespace gameProject.Systems
{
    public class EnemyManager
    {
        private List<Enemy> _enemies;
        private GraphicsDeviceManager _graphics;
        private Texture2D _enemyTexture;
        private Player _player;
        private float _spawnCooldown = 1f; // интервал между спавнами (в секундах)
        private float _timeSinceLastSpawn = 0f;

        private float _difficulty = 1f;
        private float _timeSinceDiffIncrease = 0f;
        private float _diffIncreaseInterval = 10f; // интервал между увеличением сложности

        private float _invincibleTime = 0.8f; // время неуязвимости врага после удара

        private float _timeSinceLastHit = 0f;


        private Texture2D _gemTexture;
        private GemManager _gemManager;
        private Random _random;

        private Dictionary<EnemyType, EnemyStats> _templates;

        public EnemyManager(GraphicsDeviceManager graphics, Player player)
        {
            _enemies = new List<Enemy>();
            _graphics = graphics;
            _player = player;
            _gemTexture = Textures.gemTexture;
            _gemManager = new GemManager(_player);
            _random = new Random();

            _templates = new Dictionary<EnemyType, EnemyStats>
            {
                [EnemyType.Weak] = new EnemyStats { Texture = Textures.dragonflySpritelist, Health = 5, Speed = 80f, GemSprite = new Sprite(Textures.gemTexture, 15) },
                [EnemyType.Normal] = new EnemyStats { Texture = Textures.plagueDoctor, Health = 10, Speed = 120f, GemSprite = new Sprite(Textures.gemTexture, 15) },
                [EnemyType.Fast] = new EnemyStats { Texture = Textures.wolfSpriteList, Health = 7, Speed = 250f, GemSprite = new Sprite(Textures.gemTexture, 15) },
                [EnemyType.Tank] = new EnemyStats { Texture = Textures.tankTexture, Health = 20, Speed = 60f, GemSprite = new Sprite(Textures.gemTexture, 15) },
                [EnemyType.FastTank] = new EnemyStats { Texture = Textures._fastTankTexture, Health = 20, Speed = 200f, GemSprite = new Sprite(Textures.gemTexture, 15) }
            };
        }

        public enum EnemyType { Weak, Normal, Fast, Tank, FastTank }

        public class EnemyStats
        {
            public Texture2D Texture;
            public int Health;
            public float Speed;
            public Sprite GemSprite;
        }

        public List<Enemy> Enemies => _enemies;

        public void SpawnEnemies()
        {
            int[] a = [-1, 1];
            int index = _random.Next(2);
            int index2 = _random.Next(2);
            int randomizeSpawnSide = a[index];
            int randomizeSpawnSide2 = a[index2];

            int enemyY = _random.Next((int)_player.GetComponent<PositionComponent>().Position.Y - _graphics.PreferredBackBufferHeight,
                                      (int)_player.GetComponent<PositionComponent>().Position.Y + _graphics.PreferredBackBufferHeight);

            SpawnEnemy(new Vector2(
                _player.GetComponent<PositionComponent>().Position.X + randomizeSpawnSide * (_graphics.PreferredBackBufferWidth),
                enemyY * randomizeSpawnSide2
            ));

            int enemyX = _random.Next((int)_player.GetComponent<PositionComponent>().Position.X - _graphics.PreferredBackBufferWidth,
                                      (int)_player.GetComponent<PositionComponent>().Position.X + _graphics.PreferredBackBufferWidth);

            SpawnEnemy(new Vector2(
                enemyX * randomizeSpawnSide,
                _player.GetComponent<PositionComponent>().Position.Y + randomizeSpawnSide2 * (_graphics.PreferredBackBufferHeight)
            ));
        }

        private void SpawnEnemy(Vector2 spawnPosition)
        {
            var type = ChooseEnemyType();
            var stats = _templates[type];

            var sprite = new Sprite(stats.Texture, 5);
            if (type == EnemyType.Weak)
                sprite = new Sprite(stats.Texture, 5, totalFrames:4);
            if (type == EnemyType.Normal)
                sprite = new Sprite(stats.Texture, 2, totalFrames: 12);
            if (type == EnemyType.Fast)
                sprite = new Sprite(stats.Texture, 5, totalFrames: 7);
            if (type == EnemyType.Tank)
                sprite = new Sprite(stats.Texture, 4, totalFrames: 12);
            if (type == EnemyType.FastTank)
                sprite = new Sprite(stats.Texture, 3);

            var enemy = new Enemy(sprite, spawnPosition, (int)stats.Speed, _player, stats.Health, type);

            _enemies.Add(enemy);
        }

        private EnemyType ChooseEnemyType()
        {
            float roll = (float)_random.NextDouble() * _difficulty;
            if (roll < 1) return EnemyType.Weak;
            if (roll < 2) return EnemyType.Normal;
            if (roll < 3) return EnemyType.Fast;
            if (roll < 4) return EnemyType.Tank;
            return EnemyType.FastTank;
        }

        public void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _timeSinceDiffIncrease += elapsedTime;
            if (_timeSinceDiffIncrease >= _diffIncreaseInterval)
            {
                _difficulty++;
                _timeSinceDiffIncrease = 0;
            }

            _timeSinceLastSpawn += elapsedTime;

            if (_timeSinceLastSpawn >= _spawnCooldown)
            {
                SpawnEnemies();
                _timeSinceLastSpawn = 0f;
            }

            foreach (var enemy in _enemies)
            {              
                enemy.Update(gameTime);
                if (enemy.TryGetComponent<HealthComponent>(out var hpComp) && enemy.TryGetComponent<LootDropComponent>(out var lootComp) && enemy.TryGetComponent<PositionComponent>(out var posComp))
                {
                    if (hpComp.IsDead && !lootComp.HasDroppedLoot)
                    {
                        _gemManager.AddGem(new Gem(new Sprite(_gemTexture, 3), posComp.Position));
                        lootComp.HasDroppedLoot = true;
                    }
                }
            }

            _enemies.RemoveAll(e => e.TryGetComponent(out HealthComponent health) && health.IsDead);

            for (int i = 0; i < _enemies.Count; i++)
            {
                for (int j = i + 1; j < _enemies.Count; j++)
                {
                    var enemyA = _enemies[i];
                    var enemyB = _enemies[j];

                    var rectA = enemyA.GetBounds();
                    var rectB = enemyB.GetBounds();

                    if (enemyA.HasComponent<PositionComponent>())
                        Collision.ResolveEnemiesCollision(ref enemyA.GetComponent<PositionComponent>().Position, rectA, ref enemyB.GetComponent<PositionComponent>().Position, rectB);
                }
            }


            foreach (var enemy in _enemies)
            {
                if (enemy.TryGetComponent<HealthComponent>(out var hpComp) && enemy.TryGetComponent<PositionComponent>(out var posComp))
                {
                    hpComp.TimeSinceLastHit += elapsedTime;
                    var enemyRect = enemy.GetBounds();
                    var playerRect = _player.GetBounds();
                    Collision.ResolvePlayerCollision(ref _player.GetComponent<PositionComponent>().Position, playerRect, ref posComp.Position, enemyRect, _player);

                    if (_player.HasComponent<WeaponComponent>())
                    {
                        foreach (var weapon in _player.GetComponent<WeaponComponent>().Weapons)
                        {
                            if (hpComp.TimeSinceLastHit >= _invincibleTime)
                            {
                                Collision.ResolveWeaponCollision(enemy, weapon);
                            }
                        }
                    }
                }
            }

            _gemManager.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemy in _enemies)
            {
                enemy.Draw(spriteBatch);
                //DebugDraw.DrawRectangle(spriteBatch, enemy.GetBounds(), Color.Red);
            }
            _gemManager.Draw(spriteBatch);
        }

    }

    public class GemManager
    {
        private List<Gem> _gems;
        private Player _player;

        public GemManager(Player player)
        {
            _player = player;
            _gems = new List<Gem>();
        }

        public void AddGem(Gem gem) => _gems.Add(gem);

        public void Update(GameTime gameTime)
        {
            foreach (var gem in _gems)
            {
                gem.Update(gameTime);
                Collision.ResolveGemCollision(gem, _player);
            }
            _gems.RemoveAll(g => g.TryGetComponent<GemCollectableComponent>(out var gemComp) && gemComp.IsCollected);
            //Debug.WriteLine(_player.LevelPoints);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var gem in _gems)
            {
                if (gem.HasComponent<SpriteComponent>())
                {
                    gem.GetComponent<SpriteComponent>().Sprite.Draw(spriteBatch);
                    //DebugDraw.DrawRectangle(spriteBatch, gem.GetBounds(), Color.White);
                }
            }
        }
    }
}
