using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class ItemStock
    {
        public int _amount { get; set; }
        public Item _item { get; set; }
        public ItemStock(int amount,Item item)
        {
            _amount = amount;
            _item = item;
        }
        
    }
}
