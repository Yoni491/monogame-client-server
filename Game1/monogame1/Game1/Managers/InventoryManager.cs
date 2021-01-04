using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GameClient
{
    public class InventoryManager
    {
        ItemManager _itemManager;
        int width = 35;
        int height = 35;
        Texture2D _inventoryBlock;
        Vector2 _position;
        List<ItemStock> _item_list;
        List<(Vector2, ItemStock)> _inventory_positions;
        public InventoryManager(GraphicsDevice graphicDevice,ItemManager itemManager)
        {
            _itemManager = itemManager;
            _inventoryBlock = new Texture2D(Client.game.GraphicsDevice, width, height);
            Color[] data2 = new Color[width * height];
            for (int i = 0; i < data2.Length; ++i)
            {
                if(i >= data2.Length - height|| i<= height || i % width == 0 || (i+1) % width == 0)
                    data2[i] = Color.White;
                else
                    data2[i] = Color.Black;
            }
            _inventoryBlock.SetData(data2);
            _item_list = new List<ItemStock>();
            _position = new Vector2(3 * (graphicDevice.Viewport.Bounds.Width / 10),
            graphicDevice.Viewport.Bounds.Height - (graphicDevice.Viewport.Bounds.Height / 10));
            _inventory_positions = new List<(Vector2, ItemStock)>();
            for (int i = 0; i < 8; i++)
            {
                _inventory_positions.Add((_position + new Vector2(width * i + i, 0),null));
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 8; i++)
            {
                spriteBatch.Draw(_inventoryBlock, _inventory_positions[i].Item1, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0.98f);
            }
            foreach (var tuple in _inventory_positions)
            {
                if (tuple.Item2 != null)
                {
                    tuple.Item2._item.Draw(spriteBatch);
                    Vector2 text_pos = tuple.Item1 + new Vector2(width-12, height-17);
                    spriteBatch.DrawString(GraphicManager._font, tuple.Item2._amount.ToString(), text_pos, Color.White,0, new Vector2(0, 0),1,SpriteEffects.None,0.991f);
                }
            }
        }
        public void addItemToInventory(Item itemToAdd)
        {
            int index=0;
            if(_item_list != null)
                foreach (var tuple in _inventory_positions)
                {
                    ItemStock itemStock = tuple.Item2;
                    if (itemStock != null)
                    {
                        if (itemStock._item._item_id == itemToAdd._item_id)
                        {
                            if (itemStock._amount < tuple.Item2._item._invenotryAmountAllowed)
                            {
                                itemStock._amount++;
                                _itemManager.RemoveItemFromFloor(itemToAdd);
                                return;
                            }
                        }
                    }
                    index++;
                }
            index = 0;
            foreach (var tuple in _inventory_positions)
            {
                if (tuple.Item2 == null)
                {
                    _itemManager.RemoveItemFromFloor(itemToAdd);
                    itemToAdd.MakeInventoryItem(tuple.Item1);
                    ItemStock itemStock= new ItemStock(1, itemToAdd);
                    _item_list.Add(itemStock);
                    _inventory_positions[index] = (tuple.Item1, itemStock);
                    return;
                }
                index++;
            }
        }
    }
}
