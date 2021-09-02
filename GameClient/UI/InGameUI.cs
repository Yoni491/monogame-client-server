using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    class InGameUI
    {
        SpriteFont _font;
        Vector2 _positionLevel, _positionGold;
        int _levelShowing = -1, _goldShowing = -1;
        public string _textLevel { get; set; }
        public string _textGold { get; set; }
        Color _background;
        GraphicsDevice _graphicsDevice;
        InventoryManager _inventoryManager;
        private ScreenMessage _startingLevelMessage;
        public static TextInputBox _levelTextBox;
        private Vector2 _buttonPosition;
        KeyboardState _prevState;
        bool _showLevelBox;


        public InGameUI()
        {

        }
        public void Initialize(GraphicsDevice graphicsDevice, InventoryManager inventoryManager)
        {
            _graphicsDevice = graphicsDevice;
            _font = GraphicManager.GetBasicFont("basic_22");
            _positionLevel = new Vector2(100, 0);
            _positionGold = new Vector2(200, 0);
            _background = new Color(Color.Black, 0.1f);
            _inventoryManager = inventoryManager;

            _buttonPosition = new Vector2(400, 300);
            _levelTextBox = new TextInputBox(_buttonPosition, true, 50);
            _startingLevelMessage = new ScreenMessage(graphicsDevice, "Starting level:", _buttonPosition + new Vector2(-200, -10));
            _levelTextBox._text = "1";
        }
        public void Update()
        {
            if (!Game_Client._isMultiplayer)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _prevState.IsKeyUp(Keys.Enter))
                    _showLevelBox = !_showLevelBox;
                _prevState = Keyboard.GetState();
                if (_showLevelBox)
                    _levelTextBox.Update();
            }

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
            if (_showLevelBox)
                _levelTextBox.Draw(spriteBatch);
        }
        public void ResetGraphics()
        {

        }
        public void Initialize()
        {

        }
    }
}
