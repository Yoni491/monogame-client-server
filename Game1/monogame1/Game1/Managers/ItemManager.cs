using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient
{
    public class ItemManager
    {
        static CollectionManager _collectionManager;
        static List<Item> _itemsOnTheGround;
        public ItemManager(CollectionManager collectionManager)
        {
            _collectionManager = collectionManager;
            _itemsOnTheGround = new List<Item>();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in _itemsOnTheGround)
            {
                item.DrawOnGround(spriteBatch);
            }
        }
        static public void DropItem(int []items, Vector2 position)
        {
            foreach (var item_id in items)
            {
                Item item = _collectionManager.GetItem(item_id).Drop();
                if(item!=null)
                {
                    item._position = position;
                    _itemsOnTheGround.Add(item);
                    return;
                }
            }
        }
        static public void DropGold(int amount, Vector2 position)
        {
            for (int i = 0; i < amount; i++)
            {
                Item item = _collectionManager.GetItem(10).Drop();
                if (item != null)
                {
                    item._position = position;
                    _itemsOnTheGround.Add(item);
                }
            }
            
        }
        public Item findClosestItem(Vector2 position)
        {
            Item _item = null;
            Vector2 object_position = position;
            float distance;
            float closest_object_distance = float.MaxValue;
            foreach (var item in _itemsOnTheGround)
            {
                distance = Vector2.Distance(position, item._position);
                if (distance < closest_object_distance && distance < 50)
                {
                    closest_object_distance = distance;
                    object_position = item._position;
                    _item = item;
                }
            }
            return _item;
        }
        public void RemoveItemFromFloor(Item item)
        {
            _itemsOnTheGround.Remove(item);
        }
    }
}
