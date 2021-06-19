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
        GraphicsDevice _graphicsDevice;
        Vector2 _position;
        List<ItemStock> _item_list;
        (Rectangle, ItemStock)[] _inventory_rectangles;
        int width = 55;
        int height = 35;
        int _itemBlockAmount = 8;
        MouseState _previousMouse, _currentMouse;

        public InventoryManager(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _inventoryBlock = new Texture2D(graphicsDevice, width, height);
            Color[] data2 = new Color[width * height];
            for (int i = 0; i < data2.Length; ++i)
            {
                if (i >= data2.Length - width || i <= width || i % width == 0 || (i + 1) % width == 0)
                    data2[i] = Color.White;
                else
                    data2[i] = Color.SaddleBrown;
            }
            _inventoryBlock.SetData(data2);
            _item_list = new List<ItemStock>();
            Vector2 fixedPosition = GetInventoryPosition();
            Rectangle Dest_rectangle;
            _inventory_rectangles = new (Rectangle, ItemStock)[_itemBlockAmount];
            for (int i = 0; i < _itemBlockAmount; i++)
            {
                Dest_rectangle = new Rectangle((int)fixedPosition.X + width * i + i, (int)fixedPosition.Y, width, height);
                _inventory_rectangles[i] = ((Dest_rectangle, null));
            }
        }
        public Vector2 GetInventoryPosition()
        {
            _position = new Vector2((_graphicsDevice.Viewport.Bounds.Width / 2),
            _graphicsDevice.Viewport.Bounds.Height - (_graphicsDevice.Viewport.Bounds.Height / 20));
            return new Vector2(_position.X - _itemBlockAmount / 2 * width - (_itemBlockAmount / 2 - 1), _position.Y);
        }
        public void Initialize(Player player, ItemManager itemManager)
        {
            _itemManager = itemManager;
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
                    tuple.Item2._item.DrawInventory(spriteBatch, new Vector2(tuple.Item1.X, tuple.Item1.Y));
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
        public void ResetGraphics()
        {
            Vector2 fixedPosition = GetInventoryPosition();
            for (int i = 0; i < _inventory_rectangles.Length; i++)
            {
                _inventory_rectangles[i].Item1 = new Rectangle((int)fixedPosition.X + width * i + i, (int)fixedPosition.Y, width, height);
            }
        }
        //used for removing an item from the inventory automaticly, example: key when opening door
        public bool RemoveItemFromInventory(int itemID)
        {
            int index = 0;
            bool foundItem = false;
            if (_item_list != null)
                foreach (var tuple in _inventory_rectangles)
                {
                    ItemStock itemStock = tuple.Item2;
                    if (itemStock != null)
                    {
                        if (itemStock._item._itemId == itemID)
                        {
                            foundItem = true;
                            break;                            
                        }
                    }
                    index++;
                }
            if(foundItem)
            {
                if(--_inventory_rectangles[index].Item2._amount == 0)
                {
                    _inventory_rectangles[index].Item2 = null;
                }
                return true;
            }
            return false;
        }
        public void AddItemToInventory(Item itemToAdd)
        {
            int index = 0;
            if (_item_list != null)
                foreach (var tuple in _inventory_rectangles)
                {
                    ItemStock itemStock = tuple.Item2;
                    if (itemStock != null)
                    {
                        if (itemStock._item._itemId == itemToAdd._itemId)
                        {
                            if (itemStock._amount < tuple.Item2._item._invenotryAmountAllowed)
                            {
                                itemStock._amount++;
                                _itemManager.RemoveItemFromFloor(itemToAdd);
                                AudioManager.PlaySound("PickingItem");
                                return;
                            }
                        }
                    }
                }
            foreach (var tuple in _inventory_rectangles)
            {
                if (tuple.Item2 == null)
                {
                    _itemManager.RemoveItemFromFloor(itemToAdd);
                    itemToAdd.MakeInventoryItem(new Vector2(tuple.Item1.X, tuple.Item1.Y));
                    ItemStock itemStock = new ItemStock(1, itemToAdd);
                    _item_list.Add(itemStock);
                    _inventory_rectangles[index] = (tuple.Item1, itemStock);
                    AudioManager.PlaySound("PickingItem");
                    return;
                }
                index++;
            }
        }
        public bool MouseClick()
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            for (int i = 0; i < 8; i++)
            {
                if (CollisionManager.isMouseCollidingRectangle(_inventory_rectangles[i].Item1))
                {
                    Player._mouseIntersectsUI = true;
                    if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                    {
                        if (_inventory_rectangles[i].Item2 != null)
                        {
                            Gun gun = _inventory_rectangles[i].Item2._item._gun;
                            if (gun != null)
                            {
                                if(_player._gun!=null)
                                    _player._gun._bullets.Clear();
                                _player.EquipGun(gun);
                                _inventory_rectangles[i].Item2 = null;
                            }
                            else if(_inventory_rectangles[i].Item2._item._itemHealing>0)
                            {
                                AudioManager.PlaySound("UsingPotion");
                                _player._health._health_left += _inventory_rectangles[i].Item2._item._itemHealing;
                                if (--_inventory_rectangles[i].Item2._amount == 0)
                                {
                                    _inventory_rectangles[i].Item2 = null;
                                }
                            }

                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
