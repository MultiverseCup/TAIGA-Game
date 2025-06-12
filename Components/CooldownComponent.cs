using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class CooldownComponent
    {
        public float Cooldown;
        public float TimeSinceLastAction;

        public CooldownComponent(float cooldown)
        {
            Cooldown = cooldown;
            TimeSinceLastAction = cooldown;
        }

        public void Update(GameTime gameTime)
        {
            TimeSinceLastAction += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public bool IsReady => TimeSinceLastAction >= Cooldown;

        public void Reset()
        {
            TimeSinceLastAction = 0f;
        }
    }

}
