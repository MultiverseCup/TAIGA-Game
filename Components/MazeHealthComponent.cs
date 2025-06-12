using gameProject.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class MazeHealthComponent
    {
        public int Lives { get; private set; }
        private float _invincibilityTimer = 0f;
        private const float InvincibilityDuration = 1f;
        public bool IsInvincible => _invincibilityTimer > 0;

        public MazeHealthComponent(int lives)
        {
            Lives = lives;
        }

        public void Update(GameTime gameTime)
        {
            if (_invincibilityTimer > 0)
                _invincibilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void OnHit(Action onDeath)
        {
            if (IsInvincible)
                return;

            SoundEffects.HitSound?.Play();
            Lives--;
            _invincibilityTimer = InvincibilityDuration;

            if (Lives <= 0)
            {
                onDeath.Invoke();
            }
            Debug.WriteLine(Lives);
        }
    }
}
