using gameProject.Components;
using gameProject.Entities;
using gameProject.Render;
using gameProject.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Entities.Weapons
{
    public class WhipWeapon : Entity, IWeapon
    {
        private readonly Player _player;

        public int Damage
        {
            get => GetComponent<DamageComponent>().Damage;
            set => GetComponent<DamageComponent>().Damage = value;
        }

        public int Level
        {
            get => GetComponent<WeaponLevelComponent>().Level;
            set => GetComponent<WeaponLevelComponent>().Level = value;
        }

        public WhipWeapon(Sprite sprite, Player player)
        {
            _player = player;

            AddComponent(new PositionComponent(Vector2.Zero));
            AddComponent(new SpriteComponent(sprite));
            AddComponent(new DamageComponent(5));
            AddComponent(new WeaponLevelComponent());
            AddComponent(new AttackDurationComponent(attackCooldown: 1.5f, attackDuration: 0.8f));

            var spriteComp = GetComponent<SpriteComponent>();
            spriteComp.Sprite.IsAnimating = false;
            spriteComp.Sprite.FrameTime = 0.1f;
        }

        public void Update(GameTime gameTime)
        {
            if (_player.GetComponent<HealthComponent>().IsDead) return;

            var playerPos = _player.GetComponent<PositionComponent>().Position;

            var posComp = GetComponent<PositionComponent>();
            posComp.Position = playerPos;

            var spriteComp = GetComponent<SpriteComponent>();
            spriteComp.Update(gameTime, playerPos); 

            var durComp = GetComponent<AttackDurationComponent>();
            durComp.Update(gameTime);

            if (durComp.ShouldAttack)
            {
                Attack(gameTime);
            }

            if (durComp.ShouldStop)
            {
                durComp.StopAttack();
                spriteComp.Sprite.IsAnimating = false;
            }

            spriteComp.Sprite.Update(gameTime);
        }

        public void Attack(GameTime gameTime)
        {
            var sprite = GetComponent<SpriteComponent>().Sprite;
            var durComp = GetComponent<AttackDurationComponent>();

            if (!durComp.IsAttacking && durComp.TimeSinceLastAttack >= durComp.AttackCooldown)
            {
                durComp.StartAttack();
                sprite.IsAnimating = true;
                sprite.CurrentFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var spriteComp = GetComponent<SpriteComponent>();
            var timer = GetComponent<AttackDurationComponent>();

            if (!timer.IsAttacking) return;

            spriteComp.Draw(spriteBatch);
            //DebugDraw.DrawRectangle(spriteBatch, GetBounds(), Color.White); //отобразить хитбокс
        }

        public Rectangle GetBounds()
        {
            var timer = GetComponent<AttackDurationComponent>();
            if (!timer.IsAttacking) return Rectangle.Empty;

            var sprite = GetComponent<SpriteComponent>().Sprite;

            return new Rectangle(
                (int)(sprite.Position.X + 3 * sprite.Scale),
                (int)(sprite.Position.Y + 3 * sprite.Scale),
                (int)(sprite.Width),
                (int)(sprite.Height - 6 * sprite.Scale)
            );
        }

        public void Upgrade()
        {
            var dmgComp = GetComponent<DamageComponent>();
            var durComp = GetComponent<AttackDurationComponent>();
            var lvlComp = GetComponent<WeaponLevelComponent>();

            lvlComp.Level++;

            switch (lvlComp.Level)
            {
                case 2:
                    dmgComp.Damage += 3;
                    break;
                case 3:
                    durComp.AttackCooldown -= 0.3f;
                    dmgComp.Damage += 3;
                    break;
                case 4:
                    durComp.AttackCooldown -= 0.2f;
                    dmgComp.Damage += 3;
                    break;
                case 5:
                    dmgComp.Damage += 1;
                    durComp.AttackCooldown -= 0.1f;
                    break;
            }
        }
    }


}