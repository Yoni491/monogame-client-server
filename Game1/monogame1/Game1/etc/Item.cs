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
        Texture2D _framedTexture;
        public int _item_id;
        private string _name;
        private float _dropRate;
        private int _itemLvl;
        private bool _isConsumeable;
        private bool _isUseable;
        private bool _isCraftable;
        private Recipe _recipe;
        private Gun _gun;
        public Vector2 _position;
        private bool _inInventory = false;
        public int _invenotryAmountAllowed;

        public Item(Texture2D texture, Texture2D framedTexture, int item_id, string name, float dropRate, int itemLvl, bool isConsumeable, bool isUseable, bool isCraftable, Recipe recipe, Gun gun,int invenotryAmountAllowed)
        {
            _texture = texture;
            _framedTexture = framedTexture;
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
        public Item Drop()
        {
            Random x = new Random();
            if((float)x.NextDouble()<_dropRate)
            {
                return new Item(_texture,_framedTexture, _item_id, _name, _dropRate, _itemLvl, _isConsumeable, _isUseable, _isCraftable, _recipe, _gun, _invenotryAmountAllowed);
            }
            return null;
            
        }
        public void MakeInventoryItem(Vector2 position)
        {
            _position = position;
            _inInventory = true;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_inInventory)
            {
                spriteBatch.Draw(_framedTexture, _position, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0.99f);
            }
            else
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0.1f);

        }
    }

}
