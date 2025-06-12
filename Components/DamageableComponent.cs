using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class DamageableComponent
    {
        public void TakeDamage(int damage, HealthComponent health)
        {
            health.Health -= damage;
        }
    }
}
