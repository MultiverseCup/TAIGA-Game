using gameProject.Components;
using gameProject.Render;
using gameProject.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace gameProject.Entities
{
    public class Player : Entity
    {
        private Vector2 _lastPosition;

        public Player(Sprite sprite, Vector2 position)
        {

            AddComponent(new PositionComponent(position)); // неотъемлемый компонент игрока
            AddComponent(new SpeedComponent(500));
            AddComponent(new SpriteComponent(sprite));
            AddComponent(new HealthComponent(100));
            AddComponent(new LevelComponent());
            AddComponent(new WeaponComponent());
            AddComponent(new InputComponent(GetComponent<SpeedComponent>(), GetComponent<PositionComponent>()));

            if (HasComponent<PositionComponent>() && HasComponent<SpriteComponent>())
            {
                var pos = GetComponent<PositionComponent>().Position;
                var spr = GetComponent<SpriteComponent>().Sprite;
                spr.Position = Sprite.CalculateSpritePosition(pos, spr);
            }
        }

        public void LevelUp()
        {
            if (HasComponent<LevelComponent>())
            {
                GetComponent<LevelComponent>().Level++;
            }
        }

        public Rectangle GetBounds() // подстраивается под спрайт, следовательно не существует, если нет спрайта
        {
            if (HasComponent<SpriteComponent>())
            {
                var sprite = GetComponent<SpriteComponent>().Sprite;
                return new Rectangle(
                    (int)(sprite.Position.X + 29 * sprite.Scale),
                    (int)(sprite.Position.Y + 5 * sprite.Scale),
                    sprite.Width / 2 - 4,
                    (int)(sprite.Height - 5 * sprite.Scale)
                );
            }
            else
                return Rectangle.Empty;
        }

        public void Update(GameTime gameTime)
        {
            var posComponent = GetComponent<PositionComponent>();
            var spriteComp = GetComponent<SpriteComponent>();

            if (HasComponent<HealthComponent>() && GetComponent<HealthComponent>().IsDead)
                return;

            if (HasComponent<SpeedComponent>() && HasComponent<InputComponent>())
                GetComponent<InputComponent>().Update(gameTime);

            posComponent.Update(gameTime);

            if (HasComponent<SpriteComponent>())
                spriteComp.Update(gameTime, posComponent.Position);

            

            var currentPos = posComponent.Position;

            var movement = currentPos - _lastPosition;
            if (movement.X < 0)
                spriteComp.Sprite.Effects = SpriteEffects.FlipHorizontally;
            else if (movement.X > 0)
                spriteComp.Sprite.Effects = SpriteEffects.None;

            _lastPosition = currentPos;


            GetComponent<WeaponComponent>()?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (HasComponent<HealthComponent>() && GetComponent<HealthComponent>().IsDead)
                return;

            if (HasComponent<SpriteComponent>())
                GetComponent<SpriteComponent>().Draw(spriteBatch);

            GetComponent<WeaponComponent>()?.Draw(spriteBatch);
            //DebugDraw.DrawRectangle(spriteBatch, GetBounds(), Color.White);
        }
    }
}

