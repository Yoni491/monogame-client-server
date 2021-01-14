using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GameClient
{
    public class InventoryManager
    {
        ItemManager _itemManager;
        Player _player;
        Texture2D _inventoryBlock;
        Vector2 _position;
        List<ItemStock> _item_list;
        List<(Rectangle, ItemStock)> _inventory_rectangles;
        int width = 55;
        int height = 35;
        int _itemBlockAmount = 8;
        public InventoryManager(GraphicsDevice graphicsDevice,ItemManager itemManager)
        {
            _itemManager = itemManager;
            _inventoryBlock = new Texture2D(graphicsDevice, width, height);
            Color[] data2 = new Color[width * height];
            for (int i = 0; i < data2.Length; ++i)
            {
                if(i >= data2.Length - width || i<= width || i % width == 0 || (i+1) % width == 0)
                    data2[i] = Color.White;
                else
                    data2[i] = Color.SaddleBrown;
            }
            _inventoryBlock.SetData(data2);
            _item_list = new List<ItemStock>();
            _position = new Vector2((graphicsDevice.Viewport.Bounds.Width / 2),
            graphicsDevice.Viewport.Bounds.Height - (graphicsDevice.Viewport.Bounds.Height / 10));
            Vector2 _fixedPosition =new Vector2( _position.X - _itemBlockAmount / 2 * width - (_itemBlockAmount / 2 - 1), _position.Y);
            Rectangle Dest_rectangle;
            _inventory_rectangles = new List<(Rectangle, ItemStock)>();
            for (int i = 0; i < _itemBlockAmount; i++)
            {
                Dest_rectangle = new Rectangle((int)_fixedPosition.X + width * i + i, (int)_fixedPosition.Y, width, height);
                _inventory_rectangles.Add((Dest_rectangle,null));
            }
        }
        public void Initialize(Player player)
        {
            _player = player;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            Vector2 text_pos;
            for (int i = 0; i < 8; i++)
            {
                spriteBatch.Draw(_inventoryBlock, _inventory_rectangles[i].Item1, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.98f);
            }
            foreach (var tuple in _inventory_rectangles)
            {
                if (tuple.Item2 != null)
                {
                    tuple.Item2._item.DrawInventory(spriteBatch);
                    if (tuple.Item2._amount > 1)
                    {
                        if (tuple.Item2._amount > 9)
                            text_pos = new Vector2(tuple.Item1.X, tuple.Item1.Y) + new Vector2(width - 19, height - 17);
                        else
                            text_pos = new Vector2(tuple.Item1.X, tuple.Item1.Y) + new Vector2(width - 9, height - 17);
                        spriteBatch.DrawString(GraphicManager._font, tuple.Item2._amount.ToString(), text_pos, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0.991f);
                    }
                }
            }
        }
        public void addItemToInventory(Item itemToAdd)
        {
            int index=0;
            if(_item_list != null)
                foreach (var tuple in _inventory_rectangles)
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
            foreach (var tuple in _inventory_rectangles)
            {
                if (tuple.Item2 == null)
                {
                    _itemManager.RemoveItemFromFloor(itemToAdd);
                    itemToAdd.MakeInventoryItem(new Vector2(tuple.Item1.X, tuple.Item1.Y));
                    ItemStock itemStock= new ItemStock(1, itemToAdd);
                    _item_list.Add(itemStock);
                    _inventory_rectangles[index] = (tuple.Item1, itemStock);
                    return;
                }
                index++;
            }
        }
        public bool MouseClick()
        {

            
            for (int i = 0; i < 8; i++)
            {
                if (CollisionManager.isMouseCollidingRectangle(_inventory_rectangles[i].Item1))
                {
                    if (_inventory_rectangles[i].Item2 != null)
                    {
                        Gun gun = _inventory_rectangles[i].Item2._item._gun;
                        if (gun != null)
                        {
                            _player.EquipGun(gun);
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
