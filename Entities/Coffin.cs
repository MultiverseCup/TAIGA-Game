using gameProject.Components;
using gameProject.Entities.Weapons;
using gameProject.Render;
using gameProject.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Entities
{
    public class Coffin : Entity
    {
        private Sprite _closedSprite;
        private Sprite _openSprite;
        private bool _isOpen = false;
        private bool _playerNear = false;
        private Player _player;
        private EnemyManager _enemyManager;

        public bool IsOpen => _isOpen;

        public Coffin(Sprite closedSprite, Sprite openSprite, Vector2 position, Player player, EnemyManager enemyManager)
        {
            _closedSprite = closedSprite;
            _openSprite = openSprite;
            _closedSprite.Position = position;
            _openSprite.Position = position;
            _enemyManager = enemyManager;

            _player = player;

            AddComponent(new PositionComponent(position));
            AddComponent(new SpriteComponent(_closedSprite));
        }

        public void Update(GameTime gameTime)
        {
            var playerPos = _player.GetComponent<PositionComponent>().Position;
            var coffinPos = GetComponent<PositionComponent>().Position;

            _playerNear = Vector2.Distance(playerPos, coffinPos) < 500;

            if (_playerNear && Keyboard.GetState().IsKeyDown(Keys.F) && !_isOpen)
            {
                _isOpen = true;
                GetComponent<SpriteComponent>().Sprite = _openSprite;

                var noteWeapon = new NoteWeapon(_player, Textures.noteTexture, _enemyManager);
                _player.GetComponent<WeaponComponent>().Weapons.Add(noteWeapon);
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            GetComponent<SpriteComponent>().Draw(spriteBatch);

            if (_playerNear && !_isOpen)
            {
                var pos = GetComponent<PositionComponent>().Position;
                spriteBatch.DrawString(font, "Нажмите F чтобы открыть", pos + new Vector2(-120, -40), Color.White, 0,  default, 3f, default, default);
            }

            if (_isOpen)
            {
                var pos = GetComponent<PositionComponent>().Position;
                spriteBatch.DrawString(font, "Вы получили новое оружие!", pos + new Vector2(-120, -40), Color.White, 0, default, 3f, default, default);
                spriteBatch.Draw(Textures.guitar, pos + new Vector2(-5, 20), null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            }
        }
    }

}
