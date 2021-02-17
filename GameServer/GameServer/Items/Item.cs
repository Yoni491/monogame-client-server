using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameServer
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
        public Gun _gun;
        public Vector2 _position;
        private bool _inInventory;
        public int _invenotryAmountAllowed;

        public Item(int item_id, string name, float dropRate, int itemLvl)
        {

            _item_id = item_id;
            _name = name;
            _dropRate = dropRate;
            _itemLvl = itemLvl;
        }
        public Item Copy()
        {
            Gun gun = null;
            if (_gun != null)
                gun = _gun.Copy(1,false);
            return new Item(_item_id, _name, _dropRate, _itemLvl);
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
    }

}