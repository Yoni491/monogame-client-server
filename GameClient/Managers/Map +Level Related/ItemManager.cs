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
        static public List<int> _itemsToSendPicked;
        static public List<(int,Vector2)> _itemsToSendDropped;
        static public List<(int,int)> _itemsPickedUpToSend_Server;
        public ItemManager(CollectionManager collectionManager)
        {
            _collectionManager = collectionManager;
            _itemsOnTheGround = new Dictionary<int, Item>();
            _itemsPickedUpToSend_Server = new List<(int, int)>();
            _itemsToSendPicked = new List<int>();
            _itemsToSendDropped = new List<(int, Vector2)>();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in _itemsOnTheGround)
            {
                item.Value.DrawOnGround(spriteBatch);
            }
        }
        static public void DropItemFromList(int []items, Vector2 position)
        {
            foreach (var item_id in items)
            {
                Item item = _collectionManager.GetItem(item_id,false);
                if(item!=null)
                {
                    item._position = position;
                    _itemsToSendPicked.Add(item._itemNum);
                    _itemsOnTheGround.Add(item._itemNum,item);
                    return;
                }
            }
        }
        static public void DropItemSmallChest(Vector2 position)
        {
            int[] items = { 2, 2, 2, 4, 10};
            Random x = new Random();
            int y = (int)(x.NextDouble() * items.Length);
            Item item = _collectionManager.GetItem(items[y]);
            item._position = position;
            _itemsToSendPicked.Add(item._itemNum);
            _itemsOnTheGround.Add(item._itemNum, item);
        }
        static public void DropItemNormalChest(Vector2 position)
        {
            int[] items = { 4, 5, 6, 7, 8, 9, 10};
            Random x = new Random();
            int y = (int)(x.NextDouble() * items.Length);
            Item item = _collectionManager.GetItem(items[y]);
            item._position = position;
            _itemsToSendPicked.Add(item._itemNum);
            _itemsOnTheGround.Add(item._itemNum, item);
        }
        static public void DropItem(int itemId, Vector2 position, bool alwaysDrop = false)
        {
            if (!Game_Client._isServer)
            {
                _itemsToSendDropped.Add((itemId, position));
            }
            if(!Game_Client._isMultiplayer)
            {
                Item item = _collectionManager.GetItem(itemId,alwaysDrop);
                if (item != null)
                {
                    item._position = position;
                    _itemsToSendPicked.Add(item._itemNum);
                    _itemsOnTheGround.Add(item._itemNum, item);
                    return;
                }
            }
        }
        //Client function to drop item that server sends
        static public void DropItemFromServer(int num, int id, Vector2 position)
        {
            Item item = _collectionManager.GetItem(id);
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
                Item item = _collectionManager.GetItem(10,false);
                if (item != null)
                {
                    item._position = position;
                    _itemsToSendPicked.Add(item._itemNum);
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
            _itemsOnTheGround.Clear();
            _itemsToSendPicked.Clear();
            _itemsToSendDropped.Clear();
            _itemsPickedUpToSend_Server.Clear();
        }
    }
}
