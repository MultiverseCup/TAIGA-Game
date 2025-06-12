using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class AttackDurationComponent
    {
        public float TimeSinceLastAttack = 0f;
        public float AttackDuration;
        public float AttackCooldown;
        public bool IsAttacking = false;

        public AttackDurationComponent(float attackCooldown, float attackDuration)
        {
            AttackCooldown = attackCooldown;
            AttackDuration = attackDuration;
        }

        public void Update(GameTime gameTime)
        {
            TimeSinceLastAttack += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public bool ShouldAttack => !IsAttacking && TimeSinceLastAttack >= AttackCooldown;

        public void StartAttack()
        {
            IsAttacking = true;
            TimeSinceLastAttack = 0f;
        }

        public bool ShouldStop => IsAttacking && TimeSinceLastAttack >= AttackDuration;

        public void StopAttack()
        {
            IsAttacking = false;
            TimeSinceLastAttack = 0f;
        }
    }

}
