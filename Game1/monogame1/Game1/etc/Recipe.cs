using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    
    public class Recipe
    {
        Item _item_to_craft;
        List<(Item, int)> _item_amount_list;
        public Recipe(Item item_to_craft, List<(Item, int)> item_amount_list)
        {
            _item_to_craft = item_to_craft;
            _item_amount_list = item_amount_list;
        }
    }
}
