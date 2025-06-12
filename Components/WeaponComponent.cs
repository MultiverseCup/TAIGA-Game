using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gameProject.Entities.Weapons;

namespace gameProject.Components
{
    public class WeaponComponent
    {
        public List<IWeapon> Weapons = new();

        public void Update(GameTime gameTime)
        {
            foreach (var weapon in Weapons)
                weapon.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var weapon in Weapons)
                weapon.Draw(spriteBatch);
        }
    }
}
