using gameProject.Components;
using gameProject.Core;
using gameProject.Render;
using gameProject.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Entities
{
    public class MazePlayer : Entity
    {
        public MazePlayer(Point startCell, MazeGenerator maze)
        {
            AddComponent(new GridPositionComponent(startCell));
            AddComponent(new MazePlayerInputComponent(this, maze, moveCooldown: 0.15f));
            AddComponent(new MazeCollectibleComponent());
            AddComponent(new MazeHealthComponent(3));
            AddComponent(new MazeExitComponent(this, maze, requiredCollectibles: 10));
            AddComponent(new MazePlayerRenderComponent(Textures.playerTexture));

        }

        public void Update(GameTime gameTime)
        {
            GetComponent<MazePlayerInputComponent>()?.Update(gameTime);
            GetComponent<MazeExitComponent>()?.Update();
            GetComponent<MazeHealthComponent>()?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var pos = GetComponent<GridPositionComponent>()?.Position ?? Point.Zero;
            GetComponent<MazePlayerRenderComponent>()?.Draw(spriteBatch, pos);
        }

        

        public int CollectedItems => GetComponent<MazeCollectibleComponent>()?.Collected ?? 0;
        public MazeHealthComponent HealthComp => TryGetComponent<MazeHealthComponent>(out var healthComp) ? healthComp : null;
        public bool ExitReached => GetComponent<MazeExitComponent>()?.ExitReached ?? false;
    }
}
