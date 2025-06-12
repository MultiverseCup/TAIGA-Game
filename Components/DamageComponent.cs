using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class DamageComponent
    {
        public int Damage { get; set; }

        public DamageComponent(int damage)
        {
            Damage = damage;
        }
    }
}
