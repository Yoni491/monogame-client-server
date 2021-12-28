using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class ProgressData
    {
        public int _playerID { get; set; }
        public int _level { get; set; }
        public int _animationNum { get; set; }
        public HealthManager _Health { get; set; }
        public int _gunNum { get; set; }
        public List<ProgressDataItem> _item_list { get; set; }
        public int _gold { get; set; }

        public String _playerName { get; set; }


        private ProgressData()
        {

        }
        public ProgressData(int playerID, int level, int animationNum, HealthManager health,
            int gunNum, (Rectangle, ItemStock)[] _inventory_rectangles, string playerName, int gold)
        {
            _playerID = playerID;
            _level = level;
            _animationNum = animationNum;
            _Health = health;
            _gunNum = gunNum;
            _item_list = new List<ProgressDataItem>();
            _playerName = playerName;
            _gold = gold;
            foreach (var item in _inventory_rectangles)
            {
                if (item.Item2 != null)
                    _item_list.Add(new ProgressDataItem(item.Item2._item._itemId, item.Item2._amount));
            }
        }

    }
    public class ProgressDataItem
    {
        public int _itemID { get; set; }
        public int _amount { get; set; }
        private ProgressDataItem()
        {

        }
        public ProgressDataItem(int itemID, int amount)
        {
            _itemID = itemID;
            _amount = amount;
        }
    }
}
