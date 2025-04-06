using gameProject;
using gameProject.Enemy;
using gameProject.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace _2semProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _mapTexture;

        private Texture2D _playerTexture;
        private Sprite _playerSprite;
        private Player _player;

        private Enemy _enemy;
        private Texture2D _enemyTexture;
        private EnemyManager _enemyManager;

        private Camera _camera;
        private Vector2 _startingPlayerPosition;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1600; // Ширина окна
            _graphics.PreferredBackBufferHeight = 900; // Высота окна
            _graphics.ApplyChanges();

            _camera = new Camera(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _mapTexture = Content.Load<Texture2D>("Sprite-0003");

            _startingPlayerPosition = Vector2.Zero;

            _playerTexture = Content.Load<Texture2D>("Sprite-0003");
            _playerSprite = new Sprite(_playerTexture, 5, _startingPlayerPosition);
            _player = new Player(_playerSprite, _startingPlayerPosition, 500);

            _enemyTexture = Content.Load<Texture2D>("Sprite-0003");
            _enemyManager = new EnemyManager(_graphics, _enemyTexture, _player);
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            _player.Update(gameTime);

            //_enemyManager.SpawnEnemies();
            _enemyManager.Update(gameTime);

            _camera.Follow(_player.Position);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.TransformMatrix);

            _spriteBatch.Draw(_mapTexture, Vector2.Zero, Color.White);

            _player.Draw(_spriteBatch);

            
            //_enemy.Draw(_spriteBatch);
            _enemyManager.Draw(_spriteBatch);

            _spriteBatch.End(); 

            base.Draw(gameTime);
        }
    }
}
