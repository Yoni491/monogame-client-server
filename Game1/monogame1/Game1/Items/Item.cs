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
        public int _item_id;
        private string _name;
        private float _dropRate;
        private int _itemLvl;
        private bool _isConsumeable;
        private bool _isUseable;
        private bool _isCraftable;
        private Recipe _recipe;
        public Gun _gun;
        public Vector2 _position;
        private bool _inInventory;
        public int _invenotryAmountAllowed;

        public Item(Texture2D texture, Texture2D framedTexture, int item_id, string name, float dropRate, int itemLvl, bool isConsumeable, bool isUseable, bool isCraftable, Recipe recipe, Gun gun, int invenotryAmountAllowed)
        {
            _texture = texture;
            if(framedTexture == null)
                _inventoryTexture = texture;
            else
                _inventoryTexture = framedTexture;
            _item_id = item_id;
            _name = name;
            _dropRate = dropRate;
            _itemLvl = itemLvl;
            _isConsumeable = isConsumeable;
            _isUseable = isUseable;
            _isCraftable = isCraftable;
            _recipe = recipe;
            _gun = gun;
            _invenotryAmountAllowed = invenotryAmountAllowed;
        }
        public void DrawInventory(SpriteBatch spriteBatch,Vector2 position)
        {
            float scale = _gun == null ? 1 : 0.7f;
            spriteBatch.Draw(_inventoryTexture, position, null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0.99f);
        }
        public void DrawOnGround(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0.1f);
        }
        public Item Copy()
        {
            Gun gun = null;
            if (_gun != null)
                gun = _gun.Copy(1,false);
            return new Item(_texture, _inventoryTexture, _item_id, _name, _dropRate, _itemLvl, _isConsumeable, _isUseable, _isCraftable, _recipe, gun, _invenotryAmountAllowed);
        }
        public Item Drop()
        {
            Random x = new Random();
            if ((float)x.NextDouble() < _dropRate)
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
    }

}