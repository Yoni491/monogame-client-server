using GameClient.UI;
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
        private ScreenMessage _loadLevelMessage;
        public static TextInputBox _levelTextBox;
        private ScreenPoint _buttonPosition;
        KeyboardState _prevState;
        bool _showLevelBox;
        LevelManager _levelManager;

        public InGameUI()
        {

        }
        public void Initialize(GraphicsDevice graphicsDevice, InventoryManager inventoryManager, LevelManager levelManager)
        {
            _levelManager = levelManager;
            _graphicsDevice = graphicsDevice;
            _font = GraphicManager.GetBasicFont("basic_22");
            _positionLevel = new Vector2(100, 0);
            _positionGold = new Vector2(200, 0);
            _background = new Color(Color.Black, 0.1f);
            _inventoryManager = inventoryManager;

            _buttonPosition = new ScreenPoint(500, 100);
            _levelTextBox = new TextInputBox(Vector2.Zero, _buttonPosition, true, 50);
            _loadLevelMessage = new ScreenMessage(graphicsDevice, "Load level:", _buttonPosition, new Vector2(-160, -10));
        }
        public void Update(GameTime gameTime)
        {
            if (!Game_Client._isMultiplayer)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _prevState.IsKeyUp(Keys.Enter))
                {
                    if (_showLevelBox)
                    {
                        if (!string.IsNullOrEmpty(_levelTextBox._text))
                            _levelManager.LoadNewLevel(Int32.Parse(_levelTextBox._text));
                    }
                    _showLevelBox = !_showLevelBox;
                    _levelTextBox._text = "";
                }
                _prevState = Keyboard.GetState();
                if (_showLevelBox)
                    _levelTextBox.Update(gameTime);
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
            {
                _levelTextBox.Draw(spriteBatch);
                _loadLevelMessage.Draw(spriteBatch);
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
