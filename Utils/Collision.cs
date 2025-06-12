using gameProject.Components;
using gameProject.Entities;
using gameProject.Entities.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Utils
{
    public static class Collision
    {
        public static void ResolveBasicCollision(
            ref Vector2 posA, Rectangle rectA,
            ref Vector2 posB, Rectangle rectB,
            Action onCollide = null)
        {
            if (!rectA.Intersects(rectB)) return;

            float overlapX = Math.Min(rectA.Right, rectB.Right) - Math.Max(rectA.Left, rectB.Left);
            float overlapY = Math.Min(rectA.Bottom, rectB.Bottom) - Math.Max(rectA.Top, rectB.Top);

            if (overlapX < overlapY)
            {
                float separation = overlapX / 2f;
                if (rectA.Center.X < rectB.Center.X)
                {
                    posA.X -= separation;
                    posB.X += separation;
                }
                else
                {
                    posA.X += separation;
                    posB.X -= separation;
                }
            }
            else
            {
                float separation = overlapY / 2f;
                if (rectA.Center.Y < rectB.Center.Y)
                {
                    posA.Y -= separation;
                    posB.Y += separation;
                }
                else
                {
                    posA.Y += separation;
                    posB.Y -= separation;
                }
            }

            onCollide?.Invoke();
        }

        public static void ResolveEnemiesCollision(ref Vector2 posA, Rectangle rectA, ref Vector2 posB, Rectangle rectB)
        {
            ResolveBasicCollision(ref posA, rectA, ref posB, rectB);
        }

        public static void ResolvePlayerCollision(ref Vector2 posA, Rectangle rectA, ref Vector2 posB, Rectangle rectB, Player player)
        {
            ResolveBasicCollision(ref posA, rectA, ref posB, rectB, () =>
            {
                if (player.TryGetComponent<HealthComponent>(out var health))
                {
                    health.Health -= 1;
                }
            });
        }

        public static void ResolveWeaponCollision(Enemy enemy, IWeapon weapon)
        {
            if (enemy.TryGetComponent<HealthComponent>(out var hpComp) &&
                weapon != null &&
                weapon.GetBounds().Intersects(enemy.GetBounds()) &&
                weapon.GetBounds().Height != 0)
            {
                enemy.TakeDamage(weapon.Damage);
                hpComp.TimeSinceLastHit = 0;
            }
        }

        public static void ResolveGemCollision(Gem gem, Player player)
        {
            if (gem.TryGetComponent<GemCollectableComponent>(out var gemComp) &&
                gem.GetBounds().Intersects(player.GetBounds()) &&
                !gemComp.IsCollected &&
                player.TryGetComponent<LevelComponent>(out var level))
            {
                level.LevelPoints += 1;
                gemComp.IsCollected = true;
            }
        }
    }

}
