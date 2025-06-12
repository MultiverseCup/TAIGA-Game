using gameProject.Components;
using gameProject.Systems;
using gameProject.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Entities.Weapons
{
    public class NoteWeapon : IWeapon
    {
        public int Damage { get; set; } = 5;
        public int Level { get; set; } = 1;

        private Player _player;
        private EnemyManager _enemyManager;
        private Texture2D _noteTexture;
        private List<NoteProjectile> _notes = new();

        private Dictionary<Enemy, float> _enemyHitTimers = new Dictionary<Enemy, float>();
        private const float DamageInterval = 1f;

        private float _rotationSpeed = 2f;
        private float _radius = 230f;
        private int _projectileCount = 1;

        public NoteWeapon(Player player, Texture2D noteTexture, EnemyManager enemyManager)
        {
            _player = player;
            _noteTexture = noteTexture;
            _enemyManager = enemyManager;

            CreateProjectiles();
        }

        private void CreateProjectiles()
        {
            _notes.Clear();
            for (int i = 0; i < _projectileCount; i++)
            {
                float angle = MathHelper.TwoPi * i / _projectileCount;
                _notes.Add(new NoteProjectile(_noteTexture, angle, _radius));
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 playerPosition = _player.GetComponent<PositionComponent>().Position;

            foreach (var note in _notes)
            {
                note.Update(playerPosition, _rotationSpeed * deltaTime);

                foreach (var enemy in _enemyManager.Enemies)
                {
                    if (enemy.GetComponent<HealthComponent>().IsDead)
                        continue;

                    // обновляем таймер для врага
                    if (!_enemyHitTimers.ContainsKey(enemy))
                        _enemyHitTimers[enemy] = 0f;

                    _enemyHitTimers[enemy] += deltaTime;

                    if (note.GetBounds().Intersects(enemy.GetBounds()))
                    {
                        if (_enemyHitTimers[enemy] >= DamageInterval)
                        {
                            enemy.TakeDamage(Damage); // например, Damage = 5
                            _enemyHitTimers[enemy] = 0f;
                        }
                    }
                }
            }

            // Удалить "мертвых" врагов из словаря
            foreach (var enemy in _enemyHitTimers.Keys.ToList())
            {
                if (enemy.GetComponent<HealthComponent>().IsDead)
                    _enemyHitTimers.Remove(enemy);
            }
        }


        public void Attack(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var note in _notes)
            {
                //DebugDraw.DrawRectangle(spriteBatch, note.GetBounds(), Color.Red);
                note.Draw(spriteBatch);
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
                    _rotationSpeed += 1f;
                    break;
                case 3:
                    _rotationSpeed += 1f;
                    Damage += 5;
                    break;
                case 4:
                    _projectileCount++;
                    CreateProjectiles();
                    break;
                case 5:
                    Damage += 2;
                    _radius += 20f;
                    break;
            }

            Console.WriteLine("note weapon upgraded");
        }
    }

    public class NoteProjectile
    {
        private Texture2D _texture;
        private float _angle;
        private float _radius;
        public Vector2 Position { get; private set; }
        public Vector2 Origin => new Vector2(_texture.Width / 2f, _texture.Height / 2f);

        public NoteProjectile(Texture2D texture, float angle, float radius)
        {
            _texture = texture;
            _angle = angle;
            _radius = radius;
        }

        public void Update(Vector2 center, float angleDelta)
        {
            _angle += angleDelta;
            Position = center + new Vector2(
                (float)Math.Cos(_angle),
                (float)Math.Sin(_angle)) * _radius;
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(
                (int)(Position.X - _texture.Width / 5),
                (int)(Position.Y - _texture.Width / 4),
                _texture.Width / 3,
                _texture.Height / 3);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, _angle, Origin, 0.5f, SpriteEffects.None, 0f);
        }
    }

}
