using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    class ProgressData
    {
        public int _playerID { get; set; }
        public int _level { get; set; }
        public int _animationNum { get; set; }
        public HealthManager _Health { get; set; }
        public int _healthLeft { get; set; }
        public int _gunNum { get; set; }
        public List<ItemStock> _item_list { get; set; }

        public ProgressData (int playerID, int level, int animationNum, HealthManager health, int gunNum, List<ItemStock> item_list)
        {
            _playerID = playerID;
            _level = level;
            _animationNum = animationNum;
            _Health = health;
            _gunNum = gunNum;
            _item_list = item_list;
        }

    }
}
