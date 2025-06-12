using gameProject.Entities;
using gameProject.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class EnemyAIComponent
    {
        private Player _player;

        public EnemyAIComponent(Player player)
        {
            _player = player;
        }

        public void Update(GameTime gameTime, Entity enemy)
        {
            if (!enemy.TryGetComponent(out PositionComponent position) ||
                !enemy.TryGetComponent(out SpeedComponent speed))
                return;

            var direction = _player.GetComponent<PositionComponent>().Position - position.Position;
            if (direction != Vector2.Zero)
                direction.Normalize();

            var velocity = direction * speed.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Position += velocity;
        }
    }
}
