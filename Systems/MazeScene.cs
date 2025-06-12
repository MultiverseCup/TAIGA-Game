using gameProject.Components;
using gameProject.Entities;
using gameProject.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Systems
{
    public class MazeScene
    {
        private MazePlayer _player;
        private MazeEnemy _enemy1;
        private MazeEnemy _enemy2;
        private MazeGenerator _maze;
        private Texture2D _floorTexture;
        private Texture2D _wallTexture;
        private SpriteFont _font;
        private int _сellSize = 64;
        public bool IsGameOver { get; private set; }

        public bool ExitReached => _player.ExitReached;

        public MazeScene(GraphicsDevice graphicsDevice, ContentManager content, SpriteFont font)
        {
            _font = font;

            _floorTexture = Textures.floorTexture;
            _wallTexture = Textures.wallTexture;

            _maze = new MazeGenerator(25, 15);
            _maze.Generate();

            var playerStart = _maze.GetNearestFloorToCenter();
            var enemyStart1 = _maze.GetFurthestFloorCell(playerStart);
            var enemyStart2 = _maze.GetFurthestFloorCell(enemyStart1);

            _player = new MazePlayer(playerStart, _maze);
            _enemy1 = new MazeEnemy(enemyStart1, _maze, Textures.agentEnemyTexture);
            _enemy2 = new MazeEnemy(enemyStart2, _maze, Textures.agentEnemyTexture);
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            _enemy1.Update(gameTime, _player.GetComponent<GridPositionComponent>().Position);
            _enemy2.Update(gameTime, _player.GetComponent<GridPositionComponent>().Position);

            if (_enemy1.GetComponent<GridPositionComponent>().Position == _player.GetComponent<GridPositionComponent>().Position ||
                _enemy2.GetComponent<GridPositionComponent>().Position == _player.GetComponent<GridPositionComponent>().Position)
            {
                _player.HealthComp.OnHit(() =>
                {
                    if (_player.HealthComp.Lives <= 0)
                    {
                        IsGameOver = true;
                    }
                    else
                    {
                        Restart();
                    }
                });
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < _maze.Width; x++)
                for (int y = 0; y < _maze.Height; y++)
                {
                    var cell = _maze.GetCell(x, y);
                    Rectangle dest = new Rectangle(x * _сellSize, y * _сellSize, _сellSize, _сellSize);

                    if (cell == MazeCell.Wall)
                        spriteBatch.Draw(_wallTexture, dest, Color.White);
                    else
                    {
                        spriteBatch.Draw(_floorTexture, dest, Color.White);

                        if (cell == MazeCell.Collectible)
                        {
                            var collectibleRect = new Rectangle(dest.X + _сellSize / 4, dest.Y + _сellSize / 4, _сellSize / 2, _сellSize / 2);
                            spriteBatch.Draw(Textures.gemTexture, collectibleRect, Color.Yellow);
                        }
                    }
                }

            _player.Draw(spriteBatch);
            _enemy1.Draw(spriteBatch);
            _enemy2.Draw(spriteBatch);

            if (_maze.ExitCell.HasValue && _player.GetComponent<MazeCollectibleComponent>().Collected >= _maze.Collectibles.Count)
            {
                var exit = _maze.ExitCell.Value;
                var exitRect = new Rectangle(exit.X * _сellSize, exit.Y * _сellSize, _сellSize, _сellSize);
                spriteBatch.Draw(Textures.logoTexture, exitRect, Color.White);
            }

            var lives = _player.GetComponent<MazeHealthComponent>()?.Lives ?? 0;
            spriteBatch.DrawString(_font, $"Жизни: {lives}", new Vector2(10, 10), Color.White, default, default, 2, default, default);
        }

        public void Restart()
        {
            IsGameOver = false;
            _maze = new MazeGenerator(_maze.Width, _maze.Height);
            _maze.Generate();

            var playerStart = _maze.GetNearestFloorToCenter();
            var enemyStart1 = _maze.GetFurthestFloorCell(playerStart);
            var enemyStart2 = _maze.GetFurthestFloorCell(enemyStart1);

            _player = new MazePlayer(playerStart, _maze);
            _enemy1 = new MazeEnemy(enemyStart1, _maze, Textures.agentEnemyTexture);
            _enemy2 = new MazeEnemy(enemyStart2, _maze, Textures.agentEnemyTexture);
        }
    }

}
