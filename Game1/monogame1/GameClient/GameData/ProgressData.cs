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


        private ProgressData()
        {

        }
        public ProgressData(int playerID, int level, int animationNum, HealthManager health, int gunNum, List<ItemStock> item_list)
        {
            _playerID = playerID;
            _level = level;
            _animationNum = animationNum;
            _Health = health;
            _gunNum = gunNum;
            _item_list = new List<ProgressDataItem>();
            foreach (var item in item_list)
            {
                _item_list.Add(new ProgressDataItem(item._item._itemId, item._amount));
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
        public ProgressDataItem(int itemID,int amount)
        {
            _itemID = itemID;
            _amount = amount;
        }
    }
}
