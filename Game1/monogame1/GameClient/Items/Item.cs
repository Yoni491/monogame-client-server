using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameClient
{
    public class Item
    {
        Texture2D _texture;
        Texture2D _inventoryTexture;
        public int _itemId { get; set; }
        public int _itemNum;
        private string _name;
        private float _dropRate;
        private int _itemLvl;
        private bool _isConsumeable;
        private bool _isUseable;
        private bool _isCraftable;
        public Gun _gun;
        public Vector2 _position;
        private bool _inInventory;
        public int _invenotryAmountAllowed;
        public int _itemHealing;
        public bool _aboutToBeSent;
        private float _drawScale=1;
        public Item(Texture2D texture, Texture2D framedTexture, int item_id,float scale, string name, float dropRate, int itemLvl, bool isConsumeable, bool isUseable, bool isCraftable, Gun gun, int invenotryAmountAllowed,int itemHealing = 0)
        {
            _texture = texture;
            _itemNum = ItemManager.itemNumber++;
            if (framedTexture == null)
                _inventoryTexture = texture;
            else
                _inventoryTexture = framedTexture;
            _itemId = item_id;
            _drawScale = scale;
            _name = name;
            _dropRate = dropRate;
            _itemLvl = itemLvl;
            _isConsumeable = isConsumeable;
            _isUseable = isUseable;
            _isCraftable = isCraftable;
            _gun = gun;
            _invenotryAmountAllowed = invenotryAmountAllowed;
            _itemHealing = itemHealing;
        }
        public void DrawInventory(SpriteBatch spriteBatch,Vector2 position)
        {
            
            spriteBatch.Draw(_inventoryTexture, position, null, Color.White, 0, new Vector2(0, 0), _drawScale, SpriteEffects.None, 0.99f);
        }
        public void DrawOnGround(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(0, 0), _drawScale, SpriteEffects.None, 0.015f + _itemId * 0.00001f);
        }
        public Item Copy()
        {
            Gun gun = null;
            if (_gun != null)
                gun = _gun.Copy(false,true,null);
            return new Item(_texture, _inventoryTexture, _itemId, _drawScale, _name, _dropRate, _itemLvl, _isConsumeable, _isUseable, _isCraftable, gun, _invenotryAmountAllowed,_itemHealing);
        }
        public Item Drop(bool dropAlways = false)
        {
            Random x = new Random();
            if ((float)x.NextDouble() < _dropRate || dropAlways)
            {
                return Copy();
            }
            return null;

        }
        public void MakeInventoryItem(Vector2 position)
        {
            _position = position;
            _inInventory = true;
        }
        public void UpdatePacket(Packet packet)
        {
            packet.WriteInt(_itemNum);
            packet.WriteInt(_itemId);
            packet.WriteVector2(_position);
        }
        public void UpdatePacketNum(Packet packet)
        {
            packet.WriteInt(_itemNum);
        }
    }

}