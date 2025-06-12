using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Utils
{
    public static class SoundEffects
    {
        public static SoundEffect HitSound;

        public static void Load(ContentManager content)
        {
            HitSound = content.Load<SoundEffect>("hit");
        }
    }
}
