using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class HealthComponent
    {
        public int Health { get; set; }
        public bool IsDead => Health <= 0;
        public float TimeSinceLastHit { get; set; } = 0;


        public HealthComponent(int health)
        {
            Health = health;
        }
    }
}
