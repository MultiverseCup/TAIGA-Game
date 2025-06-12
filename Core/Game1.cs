using gameProject.Components;
using gameProject.Entities;
using gameProject.Entities.Weapons;
using gameProject.Render;
using gameProject.Systems;
using gameProject.UI;
using gameProject.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace gameProject.Core
{
    public class Game1 : Game
    {

        private float _victoryTime = 2.5f * 60; // время для победы (в секундах)
        private SpriteFont _font;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameState _gameState = GameState.MainMenu;
        private MainMenu _mainMenu;
        private MazeScene _mazeScene;
        private GameOverScreen _gameOverScreen;

        private BackgroundRenderer _backgroundRenderer;
        private Player _player;
        private Camera _camera;

        private WhipWeapon _whip;   
        private RifleWeapon _rifle;
        private LevelUpMenu _levelUpMenu;
        public EnemyManager EnemyManager { get; private set; }
        public LevelUpManager LevelUpManager { get; private set; }
        private double _playingTime;
        private VictoryScreen _victoryScreen;

        private Coffin _coffin;
        private ArrowPointer _arrowPointer;


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

            _font = Content.Load<SpriteFont>("DefaultFont");

            Textures.Load(Content, GraphicsDevice);
            SoundEffects.Load(Content);

            MusicSystem.Initialize(Content);


            _mainMenu = new MainMenu(_font, Textures.logoTexture, GraphicsDevice, _graphics);
            _gameOverScreen = new GameOverScreen(_font, GraphicsDevice);
            _victoryScreen = new VictoryScreen(_font, GraphicsDevice);

            _mazeScene = new MazeScene(GraphicsDevice, Content, _font);

            _backgroundRenderer = new BackgroundRenderer(Textures.mapTexture, 600);

            _player = Initializer.InitializePlayer();

            EnemyManager = Initializer.InitializeEnemyManager(_graphics, _player);
            _levelUpMenu = new LevelUpMenu(_player, _font, EnemyManager, Textures.weaponTexture, Textures.bulletTexture);
            LevelUpManager = new LevelUpManager(_player, _levelUpMenu);

            //_whip = new WhipWeapon(new Sprite(Textures.weaponTexture, 1.0f, totalFrames: 4), _player);
            _rifle = new RifleWeapon(new Sprite(Textures.bulletTexture, 3), _player, EnemyManager);

            _player.GetComponent<WeaponComponent>()?.Weapons.Add(_rifle);
            //var noteWeapon = new NoteWeapon(_player, Textures.noteTexture, EnemyManager);
            //_player.GetComponent<WeaponComponent>()?.Weapons.Add(noteWeapon);

             _coffin = new Coffin(
                new Sprite(Textures.coffin, 3f),
                new Sprite(Textures.coffinOpen, 3f),
                _player.GetComponent<PositionComponent>().Position + new Vector2(10000, -4000),
                _player,
                EnemyManager
            );
            _arrowPointer = new ArrowPointer(_player, _coffin, Textures.arrowTexture, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);


            MediaPlayer.Play(Content.Load<Song>("Music/MainMenuTheme"));
            DebugDraw.Initialize(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (_gameState == GameState.MainMenu) // главное меню
            {

                _mainMenu.Update(gameTime);
                if (_mainMenu.StartGameRequested)
                {
                    _gameState = GameState.Maze;
                    MusicSystem.SwitchTo(_gameState);
                }
                return;
            }

            if (_gameState == GameState.Maze) // лабиринт
            {
                _mazeScene.Update(gameTime);

                if (_mazeScene.IsGameOver)
                {
                    _gameState = GameState.GameOver;
                    MusicSystem.SwitchTo(_gameState);
                    return;
                }

                if (_mazeScene.ExitReached)
                {
                    _gameState = GameState.Playing;
                    MusicSystem.SwitchTo(_gameState);
                }

                return;
            }

            if (_gameState == GameState.Playing) // основная игра
            {
                if (_player.HasComponent<HealthComponent>() && _player.GetComponent<HealthComponent>().IsDead)
                {
                    _gameState = GameState.GameOver;
                    MusicSystem.SwitchTo(_gameState);
                    return;
                }

                _playingTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (_playingTime >= _victoryTime) // необходимое время для победы (в секундах)
                {
                    _gameState = GameState.Victory;
                    MusicSystem.SwitchTo(_gameState);
                    return;
                }
                _arrowPointer?.Update();
                _coffin.Update(gameTime);
                _player.Update(gameTime);
                EnemyManager.Update(gameTime);
                LevelUpManager.Update(gameTime);
                _levelUpMenu.Update(gameTime);
                _camera.Follow(_player.GetComponent<PositionComponent>().Position);

                //Debug.WriteLine(_player.GetComponent<LevelComponent>().LevelPoints);
            }

            if (_gameState == GameState.GameOver) // экран поражения
            {
                _gameOverScreen.Update(gameTime);
                if (_gameOverScreen.RestartRequested)
                {
                    Restart();
                    _gameOverScreen.Reset();
                    _gameState = GameState.Maze;
                }

            }

            if (_gameState == GameState.Victory)
            {
                _victoryScreen.Update(gameTime);
                if (_victoryScreen.RestartRequested)
                {
                    Restart();
                    _victoryScreen.Reset();
                    _gameState = GameState.Maze;
                    MusicSystem.SwitchTo(_gameState);
                }
                else if (_victoryScreen.ExitRequested)
                {
                    Exit();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.OliveDrab);

            if (_gameState == GameState.MainMenu)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

                _mainMenu.Draw(_spriteBatch);

                _spriteBatch.End();
            }

            if (_gameState == GameState.Maze)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _mazeScene.Draw(_spriteBatch);
                _spriteBatch.End();
            }

            

            if (_gameState == GameState.Playing)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.TransformMatrix);
                _backgroundRenderer.Draw(_spriteBatch, _camera);
                _player.Draw(_spriteBatch);
                EnemyManager.Draw(_spriteBatch);
                _coffin.Draw(_spriteBatch, _font);

                _spriteBatch.End();

                _spriteBatch.Begin();
                _arrowPointer?.Draw(_spriteBatch);
                _levelUpMenu.Draw(_spriteBatch);
                _spriteBatch.DrawString(_font, $"Время: {Math.Round(_playingTime)} / {_victoryTime}", new Vector2(10, 10), Color.White, default, default, 2, default, default);

                _spriteBatch.End();
            }

            if (_gameState == GameState.GameOver)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _gameOverScreen.Draw(_spriteBatch);
                _spriteBatch.End();
            }

            else if (_gameState == GameState.Victory)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _victoryScreen.Draw(_spriteBatch);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
            
        }

        private void Restart()
        {
            _mazeScene = new MazeScene(GraphicsDevice, Content, _font);

            _player = Initializer.InitializePlayer();

            EnemyManager = Initializer.InitializeEnemyManager(_graphics, _player);
            _levelUpMenu = new LevelUpMenu(_player, _font, EnemyManager, Textures.weaponTexture, Textures.bulletTexture);
            LevelUpManager = new LevelUpManager(_player, _levelUpMenu);

            _rifle = new RifleWeapon(new Sprite(Textures.bulletTexture, 3), _player, EnemyManager);
            _player.GetComponent<WeaponComponent>()?.Weapons.Add(_rifle);

            _coffin = new Coffin(
                new Sprite(Textures.coffin, 3f),
                new Sprite(Textures.coffinOpen, 3f),
                _player.GetComponent<PositionComponent>().Position + new Vector2(10000, -4000),
                _player,
                EnemyManager
            );
            _arrowPointer = new ArrowPointer(_player, _coffin, Textures.arrowTexture, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            _playingTime = 0;
            _gameState = GameState.Maze;

            MusicSystem.SwitchTo(_gameState);
        }
    }
}
