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
    public class SpriteComponent
    {
        public Sprite Sprite;

        public SpriteComponent(Sprite sprite)
        {
            Sprite = sprite;
        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            Sprite.Position = Sprite.CalculateSpritePosition(position, Sprite);
            Sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
    }
}
