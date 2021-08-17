using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    class InGameUI
    {
        SpriteFont _font;
        Vector2 _positionLevel,_positionGold;
        int _levelShowing=-1,_goldShowing=-1;
        public string _textLevel { get; set; }
        public string _textGold { get; set; }
        Color _background;
        GraphicsDevice _graphicsDevice;
        InventoryManager _inventoryManager;

        public InGameUI()
        {
            
        }
        public void Initialize(GraphicsDevice graphicDevice, InventoryManager inventoryManager)
        {
            _graphicsDevice = graphicDevice;
            _font = GraphicManager.GetBasicFont("basic_22");
            _positionLevel = new Vector2(100, 0);
            _positionGold = new Vector2(200, 0);
            _background = new Color(Color.Black, 0.1f);
            _inventoryManager = inventoryManager;

        }
        public void Update()
        {
            if (_levelShowing != LevelManager._currentLevel)
            {
                _levelShowing = LevelManager._currentLevel;
                _textLevel = "Level: " + _levelShowing.ToString();
            }
            if (_goldShowing != _inventoryManager._gold)
            {
                _goldShowing = _inventoryManager._gold;
                _textGold = "Gold: " + _goldShowing.ToString();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!string.IsNullOrEmpty(_textLevel))
            {
                spriteBatch.DrawString(_font, _textLevel, _positionLevel, Color.White, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.99f);
            }
            if (!string.IsNullOrEmpty(_textGold))
            {
                spriteBatch.DrawString(_font, _textGold, _positionGold, Color.White, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.99f);
            }
        }
        public void ResetGraphics()
        {

        }
        public void Initialize()
        {

        }
    }
}
