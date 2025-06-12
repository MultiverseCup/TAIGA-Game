using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using gameProject.Render;
using gameProject.Components;
using static gameProject.Systems.EnemyManager;
using System.Runtime.CompilerServices;



namespace gameProject.Entities
{
    public class Enemy : Entity
    {
        private Vector2 _lastPosition;
        private EnemyType _enemyType;

        public Enemy(Sprite sprite, Vector2 position, int speed, Player player, int health, EnemyType enemyType)
        {
            AddComponent(new PositionComponent(position)); // неотъемлемый компонент
            AddComponent(new SpriteComponent(sprite));
            AddComponent(new SpeedComponent(speed));
            AddComponent(new HealthComponent(health));
            AddComponent(new EnemyAIComponent(player));
            AddComponent(new LootDropComponent());
            AddComponent(new DamageableComponent());

            _lastPosition = position;
            _enemyType = enemyType;

            var pos = GetComponent<PositionComponent>().Position;
            if (TryGetComponent<SpriteComponent>(out var sprComp))
                sprComp.Sprite.Position = Sprite.CalculateSpritePosition(pos, sprComp.Sprite);
        }

        public Rectangle GetBounds()
        {
            if (HasComponent<HealthComponent>() && GetComponent<HealthComponent>().IsDead)
            {
                return Rectangle.Empty;
            }


            if (HasComponent<SpriteComponent>())
            {
                var sprite = GetComponent<SpriteComponent>()?.Sprite;
                switch (_enemyType)
                {
                    case (EnemyType.Weak):
                        return new Rectangle(
                            (int)sprite.Position.X + sprite.Width / 4,
                            (int)(sprite.Position.Y + 13 * sprite.Scale),
                            (int)(sprite.Width - 14 * sprite.Scale),
                            (int)(sprite.Height - 22 * sprite.Scale)
                        );
                    case (EnemyType.Normal):
                        return new Rectangle(
                            (int)(sprite.Position.X + 32 * sprite.Scale),
                            (int)(sprite.Position.Y + 5 * sprite.Scale),
                            (int)(sprite.Width - 61 * sprite.Scale),
                            (int)(sprite.Height - 14 * sprite.Scale)
                        );
                    case (EnemyType.Fast):
                        return new Rectangle(
                            (int)sprite.Position.X,
                            (int)sprite.Position.Y + sprite.Height / 4,
                            sprite.Width,
                            sprite.Height - sprite.Height / 4
                        );
                    case (EnemyType.Tank):
                        return new Rectangle(
                            (int)(sprite.Position.X + 8 * sprite.Scale),
                            (int)(sprite.Position.Y + 13 * sprite.Scale),
                            (int)(sprite.Width - 14 * sprite.Scale),
                            (int)(sprite.Height - 13 * sprite.Scale)
                        );
                    case (EnemyType.FastTank):
                        return new Rectangle(
                            (int)(sprite.Position.X + 10 * sprite.Scale),
                            (int)(sprite.Position.Y + 11 * sprite.Scale),
                            (int)(sprite.Width - 31 * sprite.Scale),
                            (int)(sprite.Height - 11 * sprite.Scale)
                        );
                    default:              
                        return new Rectangle(
                            (int)sprite.Position.X + sprite.Width / 4,
                            (int)sprite.Position.Y,
                            sprite.Width / 2,
                            sprite.Height
                        );
                }
            }
            return Rectangle.Empty;
        }

        public void Update(GameTime gameTime)
        {
            if (TryGetComponent<HealthComponent>(out var hpComp))
                if (hpComp.IsDead) return;

            var posComponent = GetComponent<PositionComponent>();
            var spriteComp = GetComponent<SpriteComponent>();

            var currentPos = posComponent.Position;

            var movement = currentPos - _lastPosition;
            if (movement.X < 0)
                spriteComp.Sprite.Effects = SpriteEffects.FlipHorizontally;
            else if (movement.X > 0)
                spriteComp.Sprite.Effects = SpriteEffects.None;

            _lastPosition = currentPos;

            GetComponent<EnemyAIComponent>()?.Update(gameTime, this);
            spriteComp?.Update(gameTime, currentPos);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (HasComponent<HealthComponent>() && !GetComponent<HealthComponent>().IsDead)
            {
                GetComponent<SpriteComponent>()?.Draw(spriteBatch);
            }
        }

        public void TakeDamage(int damage)
        {
            if (HasComponent<HealthComponent>())
                GetComponent<DamageableComponent>()?.TakeDamage(damage, GetComponent<HealthComponent>());
        }
    }


    public class Gem : Entity
    {
        public Gem(Sprite sprite, Vector2 position)
        {
            AddComponent(new PositionComponent(position));
            AddComponent(new SpriteComponent(sprite));
            AddComponent(new GemCollectableComponent());

            sprite.Position = position;
        }

        public Rectangle GetBounds()
        {
            var sprite = GetComponent<SpriteComponent>().Sprite;
            var position = GetComponent<PositionComponent>().Position;

            return new Rectangle(
                (int)position.X - sprite.Width / 4,
                (int)(position.Y - (sprite.Height - 12 * sprite.Scale) / 2),
                sprite.Width / 2,
                (int)(sprite.Height - 12 * sprite.Scale)
            );
        }

        public void Update(GameTime gameTime)
        {
            GetComponent<SpriteComponent>().Update(gameTime, GetComponent<PositionComponent>().Position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GetComponent<SpriteComponent>().Draw(spriteBatch);
        }
    }
}
