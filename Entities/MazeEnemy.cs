using gameProject.Components;
using gameProject.Render;
using gameProject.Systems;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Entities
{
    public class MazeEnemy : Entity
    {
        public MazeEnemy(Point startPosition, MazeGenerator maze, Texture2D texture)
        {
            AddComponent(new GridPositionComponent(startPosition));
            AddComponent(new MazeEnemyAIComponent(this, maze));
            AddComponent(new SpriteComponent(new Sprite(texture, 1)));
        }

        public void Update(GameTime gameTime, Point playerPosition)
        {
            GetComponent<MazeEnemyAIComponent>()?.Update(gameTime, playerPosition);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var gridPos = GetComponent<GridPositionComponent>()?.Position ?? Point.Zero;
            var spriteComp = GetComponent<SpriteComponent>();
            if (spriteComp != null)
            {
                Rectangle dest = new Rectangle(gridPos.X * GridPositionComponent.CellSize, gridPos.Y * GridPositionComponent.CellSize, GridPositionComponent.CellSize, GridPositionComponent.CellSize);
                spriteBatch.Draw(spriteComp.Sprite.Texture, dest, Color.LightSalmon);
            }
        }
    }

}
