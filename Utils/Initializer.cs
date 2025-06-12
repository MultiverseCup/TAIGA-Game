using gameProject.Components;
using gameProject.Entities;
using gameProject.Render;
using gameProject.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Utils
{
    public static class Initializer
    {
        public static Player InitializePlayer()
        {
            var sprite = new Sprite(Textures.playerTexture, 2);
            var player = new Player(sprite, Vector2.Zero);
            return player;
        }

        public static EnemyManager InitializeEnemyManager(GraphicsDeviceManager graphics, Player player)
        {
            return new EnemyManager(
                graphics,
                player
            );
        }
    }
}
