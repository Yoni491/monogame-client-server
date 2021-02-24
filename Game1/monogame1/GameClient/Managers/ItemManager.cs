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
        static public Dictionary<int,Item> _itemsOnTheGround;
        static public int itemNumber = 0;
        static public List<int> _itemsToSend;
        static public List<(int,int)> _itemsPickedUpToSend;
        public ItemManager(CollectionManager collectionManager)
        {
            _collectionManager = collectionManager;
            _itemsOnTheGround = new Dictionary<int, Item>();
            _itemsPickedUpToSend = new List<(int, int)>();
            _itemsToSend = new List<int>();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in _itemsOnTheGround)
            {
                item.Value.DrawOnGround(spriteBatch);
            }
        }
        static public void DropItem(int []items, Vector2 position,bool alwaysDrop = false)
        {
            foreach (var item_id in items)
            {
                Item item = _collectionManager.GetItem(item_id).Drop(alwaysDrop);
                if(item!=null)
                {
                    item._position = position;
                    _itemsToSend.Add(item._itemNum);
                    _itemsOnTheGround.Add(item._itemNum,item);
                    return;
                }
            }
        }
        static public void DropItem(int itemId, Vector2 position, bool alwaysDrop = false)
        {

                Item item = _collectionManager.GetItem(itemId).Drop(alwaysDrop);
                if (item != null)
                {
                    item._position = position;
                    _itemsToSend.Add(item._itemNum);
                    _itemsOnTheGround.Add(item._itemNum, item);
                    return;
                }
        }
        static public void DropItemFromServer(int num, int id, Vector2 position)
        {
            Item item = _collectionManager.GetItem(id).Drop(true);
            if (item != null)
            {
                item._itemNum = num;
                item._position = position;
                _itemsOnTheGround.Add(item._itemNum, item);
                return;
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
                    _itemsToSend.Add(item._itemNum);
                    _itemsOnTheGround.Add(item._itemNum,item);
                }
            }
            
        }
        public Item findClosestItem(Vector2 position)
        {
            Item itemResult = null;
            Vector2 object_position = position;
            float distance;
            float closest_object_distance = float.MaxValue;
            foreach (var item in _itemsOnTheGround)
            {
                if (!item.Value._aboutToBeSent)
                {
                    distance = Vector2.Distance(position, item.Value._position);
                    if (distance < closest_object_distance && distance < 50)
                    {
                        closest_object_distance = distance;
                        object_position = item.Value._position;
                        itemResult = item.Value;
                    }
                }
            }
            return itemResult;
        }
        public void RemoveItemFromFloor(Item item)
        {
            _itemsOnTheGround.Remove(item._itemNum);
        }
        public static void Reset()
        {
            if (!Game_Client._isServer && !Game_Client._isServer)
            {
                _itemsOnTheGround.Clear();
            }
            foreach (var item in _itemsOnTheGround)
            {
                _itemsPickedUpToSend.Add((-1, item.Key));
            }
        }
    }
}
