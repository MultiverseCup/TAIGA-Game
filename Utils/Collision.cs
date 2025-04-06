using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Utils
{
    public static class Collision
    {

        public static void ResolveRectangleCollision(
            ref Vector2 posA, Rectangle rectA,
            ref Vector2 posB, Rectangle rectB)
        {
            int a = 1;
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
        }
    }

}
