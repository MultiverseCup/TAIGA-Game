using gameProject.Components;
using gameProject.Entities;
using gameProject.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.UI
{
    public class ArrowPointer
    {
        private Player _player;
        private Coffin _coffin;
        private Texture2D _texture;
        private Vector2 _screenCenter;
        private bool _active = true;

        public ArrowPointer(Player player, Coffin coffin, Texture2D texture, int screenWidth, int screenHeight)
        {
            _player = player;
            _coffin = coffin;
            _texture = texture;
            _screenCenter = new Vector2(screenWidth / 2f, screenHeight / 2f);
        }

        public void Update()
        {
            if (_coffin.IsOpen)
            {
                _active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_active) return;

            var playerPos = _player.GetComponent<PositionComponent>().Position;
            var coffinPos = _coffin.GetComponent<PositionComponent>().Position;

            Vector2 direction = coffinPos - playerPos;

            if (direction.LengthSquared() < 10f) return;

            direction.Normalize();
            float angle = (float)Math.Atan2(direction.Y, direction.X);

            Vector2 arrowPos = _screenCenter + direction * 200f;

            spriteBatch.Draw(
                _texture,
                arrowPos,
                null,
                Color.White,
                angle,
                new Vector2(_texture.Width / 2f, _texture.Height / 2f),
                3f,
                SpriteEffects.None,
                0
            );
        }
    }

}
