using gameProject.Components;
using gameProject.Core;
using gameProject.Entities;
using gameProject.Render;
using gameProject.Systems;
using gameProject.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Entities.Weapons
{
    public class RifleWeapon : Entity, IWeapon
    {
        public int Damage
        {
            get => GetComponent<DamageComponent>().Damage;
            set => GetComponent<DamageComponent>().Damage = value;
        }
        public int Level { get; set; } = 1;

        private int _projectiles = 1;
        private float _shotDelay = 0.1f;

        private int _queuedShots = 0;
        private float _timeSinceLastShot = 0f;

        private readonly List<Bullet> _bullets = new();
        private readonly Player _player;
        private readonly EnemyManager _enemyManager;

        public RifleWeapon(Sprite sprite, Player player, EnemyManager enemyManager)
        {
            _player = player;
            _enemyManager = enemyManager;

            AddComponent(new SpriteComponent(sprite));
            AddComponent(new PositionComponent(player.GetComponent<PositionComponent>().Position));
            AddComponent(new CooldownComponent(1f));
            AddComponent(new DamageComponent(4));
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            GetComponent<CooldownComponent>().Update(gameTime);

            if (GetComponent<CooldownComponent>().IsReady)
            {
                _queuedShots = _projectiles;
                GetComponent<CooldownComponent>().Reset();
            }

            if (_queuedShots > 0)
            {
                _timeSinceLastShot += deltaTime;

                if (_timeSinceLastShot >= _shotDelay)
                {
                    Attack(gameTime);
                    _queuedShots--;
                    _timeSinceLastShot = 0f;
                }
            }

            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                _bullets[i].Update(deltaTime);

                foreach (var enemy in _enemyManager.Enemies)
                {
                    if (_bullets[i].GetBounds().Intersects(enemy.GetBounds()))
                    {
                        enemy.TakeDamage(Damage);
                        _bullets.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void Attack(GameTime gameTime)
        {
            var nearestEnemy = FindNearestEnemy();
            if (nearestEnemy == null) return;

            var playerPos = _player.GetComponent<PositionComponent>().Position;
            var enemyPos = nearestEnemy.GetComponent<PositionComponent>().Position;
            var direction = Vector2.Normalize(enemyPos - playerPos);

            var bulletSprite = new Sprite(GetComponent<SpriteComponent>().Sprite.Texture, GetComponent<SpriteComponent>().Sprite.Scale);
            _bullets.Add(new Bullet(bulletSprite, playerPos, direction * 1000f));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var bullet in _bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public Rectangle GetBounds()
        {
            return Rectangle.Empty;
        }

        public void Upgrade()
        {
            Level++;
            switch (Level)
            {
                case 2:
                    Damage += 1;
                    break;
                case 3:
                    GetComponent<CooldownComponent>().Cooldown -= 0.3f;
                    break;
                case 4:
                    GetComponent<CooldownComponent>().Cooldown -= 0.2f;
                    Damage += 3;
                    break;
                case 5:
                    _projectiles++;
                    Damage += 1;
                    GetComponent<CooldownComponent>().Cooldown -= 0.1f;
                    break;
            }
        }

        private Enemy FindNearestEnemy()
        {
            if (_enemyManager == null || _enemyManager.Enemies.Count == 0)
                return null;

            var playerPos = _player.GetComponent<PositionComponent>().Position;
            float minDistance = float.MaxValue;
            Enemy nearest = null;

            foreach (var enemy in _enemyManager.Enemies)
            {
                float distance = Vector2.Distance(playerPos, enemy.GetComponent<PositionComponent>().Position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = enemy;
                }
            }

            return nearest;
        }
    }

    public class Bullet(Sprite sprite, Vector2 position, Vector2 velocity)
    {
        public Sprite Sprite { get; } = sprite;
        public Vector2 Position { get; private set; } = position;
        public Vector2 Velocity { get; } = velocity;
        public Rectangle GetBounds()
        {
            return new Rectangle(
            (int)Position.X - Sprite.Width / 16,
            (int)Position.Y - Sprite.Height / 16,
            Sprite.Width / 8,
            Sprite.Height / 8
            );
        }

        public void Update(float deltaTime)
        {
            Position += Velocity * deltaTime;
            Sprite.Position = Sprite.CalculateSpritePosition(Position, Sprite);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
            //DebugDraw.DrawRectangle(spriteBatch, GetBounds(), Color.White);
        }
    }
}
