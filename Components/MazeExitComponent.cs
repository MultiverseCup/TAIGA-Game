using gameProject.Entities;
using gameProject.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class MazeExitComponent
    {
        private Entity _owner;
        private MazeGenerator _maze;
        private int _requiredCollectibles;
        public bool ExitReached { get; private set; } = false;

        public MazeExitComponent(Entity owner, MazeGenerator maze, int requiredCollectibles)
        {
            _owner = owner;
            _maze = maze;
            _requiredCollectibles = requiredCollectibles;
        }

        public void Update()
        {
            var grid = _owner.GetComponent<GridPositionComponent>();
            var col = _owner.GetComponent<MazeCollectibleComponent>();

            if (grid == null || col == null || !_maze.ExitCell.HasValue) return;

            if (grid.Position == _maze.ExitCell.Value && col.Collected >= _maze.Collectibles.Count)
            {
                ExitReached = true;
            }
        }
    }
}
