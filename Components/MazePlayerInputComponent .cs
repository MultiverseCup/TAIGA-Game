using gameProject.Entities;
using gameProject.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class MazePlayerInputComponent
    {
        private Entity _owner;
        private MazeGenerator _maze;
        private float _moveCooldown;
        private float _moveTimer = 0f;

        public MazePlayerInputComponent(Entity owner, MazeGenerator maze, float moveCooldown)
        {
            _owner = owner;
            _maze = maze;
            _moveCooldown = moveCooldown;
        }

        public void Update(GameTime gameTime)
        {
            _moveTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_moveTimer > 0) return;

            var gridPos = _owner.GetComponent<GridPositionComponent>();
            if (gridPos == null) return;

            Point next = gridPos.Position;
            var k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.W)) next.Y--;
            else if (k.IsKeyDown(Keys.S)) next.Y++;
            else if (k.IsKeyDown(Keys.A)) next.X--;
            else if (k.IsKeyDown(Keys.D)) next.X++;
            else return;

            if (next != gridPos.Position && _maze.GetCell(next.X, next.Y) != MazeCell.Wall)
            {
                gridPos.Position = next;
                _moveTimer = _moveCooldown;

                if (_maze.GetCell(next.X, next.Y) == MazeCell.Collectible)
                {
                    _maze.SetCell(next, MazeCell.Floor);
                    _owner.GetComponent<MazeCollectibleComponent>()?.Collect();

                    if (_owner.GetComponent<MazeCollectibleComponent>()?.Collected >= _maze.RequiredCollectibles && _maze.ExitCell == null)
                        _maze.OpenExit(next);
                }
            }
        }
    }
}
