using gameProject.Core;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Systems
{
    public enum MusicMode
    {
        MainMenu,
        Gameplay,
        Maze
    }
    public static class MusicSystem
    {
        private static Dictionary<GameState, Song> _songs;
        private static GameState _currentState = GameState.MainMenu;

        private static bool _initialized = false;

        public static void Initialize(ContentManager content)
        {
            if (_initialized) return;

            _songs = new Dictionary<GameState, Song>
            {
            { GameState.MainMenu, content.Load<Song>("Music/MainMenuTheme") },
            { GameState.Maze, content.Load<Song>("Music/MazeTheme") },
            { GameState.Playing, content.Load<Song>("Music/GameplayTheme") },
            { GameState.GameOver, content.Load<Song>("Music/GameOverTheme") },
            { GameState.Victory, content.Load<Song>("Music/VictoryTheme") }
            };

            MediaPlayer.IsRepeating = true;
            _initialized = true;
        }

        public static void SwitchTo(GameState newState)
        {
            if (_currentState == newState || !_songs.ContainsKey(newState))
                return;

            _currentState = newState;
            MediaPlayer.Stop();
            MediaPlayer.Play(_songs[newState]);
        }
    }

}
