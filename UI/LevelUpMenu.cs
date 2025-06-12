using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using gameProject.Render;
using gameProject.Components;
using gameProject.Systems;
using gameProject.Entities;
using gameProject.Entities.Weapons;

namespace gameProject.UI
{
    public class LevelUpMenu
    {
        private Player _player;
        private SpriteFont _font;
        private EnemyManager _enemyManager;
        private Texture2D _whipTexture;
        private Texture2D _rifleTexture;
        public bool IsOpen { get; private set; }

        public event Action OnUpgradeConfirmed; // уведомляет LevelUpManager

        private List<string> _options = new List<string>();
        private int _selectedIndex = 0;
        private KeyboardState _previousKeyboardState;


        public LevelUpMenu(Player player, SpriteFont font, EnemyManager enemyManager, Texture2D whipTexture, Texture2D rifleTexture)
        {
            _player = player;
            _font = font;
            _enemyManager = enemyManager;
            _whipTexture = whipTexture;
            _rifleTexture = rifleTexture;
        }

        public void Open()
        {
            _options.Clear();
            _selectedIndex = 0;

            var weapons = _player.GetComponent<WeaponComponent>().Weapons;

            if (weapons.Any(w => w is WhipWeapon))
                _options.Add("Улучшить Луч");

            if (weapons.Any(w => w is RifleWeapon))
                _options.Add("Улучшить Винтовку");

            if (weapons.Any(w => w is NoteWeapon))
                _options.Add("Улучшить Гитару");

            if (!weapons.Any(w => w is WhipWeapon))
                _options.Add("Добавить новое оружие (Луч)");

            if (!weapons.Any(w => w is RifleWeapon))
                _options.Add("Добавить новое оружие (Винтовка)");

            //if (!weapons.Any(w => w is NoteWeapon))
            //    _options.Add("Добавить новое оружие (Гитара)");

            IsOpen = true;
        }
        public void Update(GameTime gameTime)
        {
            if (!IsOpen) return;

            var state = Keyboard.GetState();

            if (IsSingleKeyPress(state, Keys.Up))
            {
                _selectedIndex = (_selectedIndex - 1 + _options.Count) % _options.Count;
            }

            if (IsSingleKeyPress(state, Keys.Down))
            {
                _selectedIndex = (_selectedIndex + 1) % _options.Count;
            }

            if (IsSingleKeyPress(state, Keys.Enter))
            {
                ApplyChoice(_selectedIndex);
                IsOpen = false;
                OnUpgradeConfirmed?.Invoke();
            }

            _previousKeyboardState = state;
        }

        private bool IsSingleKeyPress(KeyboardState currentState, Keys key)
        {
            return currentState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
        }

        private void ApplyChoice(int index)
        {
            string choice = _options[index];
            var weapons = _player.GetComponent<WeaponComponent>().Weapons;

            switch (choice)
            {
                case "Улучшить Луч":
                    weapons.FirstOrDefault(w => w is WhipWeapon)?.Upgrade();
                    break;

                case "Улучшить Винтовку":
                    weapons.FirstOrDefault(w => w is RifleWeapon)?.Upgrade();
                    break;

                case "Улучшить Гитару":
                    weapons.FirstOrDefault(w => w is NoteWeapon)?.Upgrade();
                    break;

                case "Добавить новое оружие (Луч)":
                    weapons.Add(new WhipWeapon(new Sprite(_whipTexture, 10, totalFrames: 3), _player));
                    break;

                case "Добавить новое оружие (Винтовка)":
                    weapons.Add(new RifleWeapon(new Sprite(_rifleTexture, 3), _player, _enemyManager));
                    break;

                //case "Добавить новое оружие (Note)":
                //    weapons.Add(new NoteWeapon(_player, Textures.noteTexture, _enemyManager));
                //    break;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsOpen) return;

            var position = new Vector2(100, 100);

            for (int i = 0; i < _options.Count; i++)
            {
                Color color = i == _selectedIndex ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_font, _options[i], position + new Vector2(0, i * 30), color);
            }
        }
    }
}
