using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    class ProgressData
    {
        int _playerID;
        int _level;
        int _animationNum;
        int _totalHealth;
        int _healthLeft;
        int _gunNum;
        List<ItemStock> _item_list;

        public ProgressData (int playerID, int level, int animationNum, int totalHealth, int healthLeft, int gunNum, List<ItemStock> item_list)
        {
            _playerID = playerID;
            _level = level;
            _animationNum = animationNum;
            _totalHealth = totalHealth;
            _healthLeft = healthLeft;
            _gunNum = gunNum;
            _item_list = item_list;
        }

    }
}
