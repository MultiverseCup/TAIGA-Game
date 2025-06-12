using gameProject.Entities;
using gameProject.Systems;
using gameProject.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class MazeEnemyAIComponent
    {
        private MazeGenerator _maze;
        private Queue<Point> _path = new();
        private float _timer = 0;
        private float _moveDelay = 0.5f;
        private float _speedTimer = 0;

        private readonly Entity _owner;

        public MazeEnemyAIComponent(Entity owner, MazeGenerator maze)
        {
            _owner = owner;
            _maze = maze;
        }

        public void Update(GameTime gameTime, Point target)
        {
            _speedTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_speedTimer > 8f)
            {
                _speedTimer = 0f;
                if (_moveDelay > 0.2f)
                    _moveDelay -= 0.06f;
            }

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer < _moveDelay) return;

            _timer = 0f;

            var gridComp = _owner.GetComponent<GridPositionComponent>();
            if (gridComp == null) return;

            _path = new Queue<Point>(AStarPathfinder.FindPath(_maze, gridComp.Position, target));

            if (_path.Count > 1)
                gridComp.Position = _path.ElementAt(1);
        }
    }

}
