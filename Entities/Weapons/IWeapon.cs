using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Entities.Weapons
{
    public interface IWeapon
    {
        int Damage { get; set; }
        public int Level { get; set; }
        void Update(GameTime gameTime);
        void Attack(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        Rectangle GetBounds();

        void Upgrade();
    }
}
