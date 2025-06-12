using gameProject.Components;
using gameProject.Entities;
using gameProject.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Systems
{
    public class LevelUpManager
    {
        private Player _player;
        private int _pointsToLevelUp = 2;
        public bool IsLevelUpPending { get; private set; }


        private LevelUpMenu _menu;


        public LevelUpManager(Player player, LevelUpMenu menu)
        {
            _player = player;
            _menu = menu;
            _menu.OnUpgradeConfirmed += ConfirmUpgrade;
        }

        public void EnableLevelProgress()
        {

            if (_player.HasComponent<LevelComponent>() && !IsLevelUpPending && _player.GetComponent<LevelComponent>().LevelPoints >= _pointsToLevelUp)
            {
                IsLevelUpPending = true;
                _player.GetComponent<LevelComponent>().LevelPoints -= _pointsToLevelUp;
                _pointsToLevelUp = (int)(_pointsToLevelUp * 1.5);
                _menu.Open();
            }
        }

        public void ConfirmUpgrade()
        {
            _player.LevelUp();
            IsLevelUpPending = false;
        }

        public void Update(GameTime gameTime)
        {
            EnableLevelProgress();
            //Debug.WriteLine(_player.LevelPoints);
        }
    }
}
