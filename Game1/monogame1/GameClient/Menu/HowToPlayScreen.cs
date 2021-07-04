using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    class HowToPlayScreen
    {
        Button _keyBoardControllerKeys, _returnToMain;
        Vector2 _buttonPosition;
        private GraphicsDevice _graphicsDevice;
        private MainMenuManager _menuManager;
        int _buttonHeight = 60;
        int _buttonWeight = 250;
        Game_Client _gameClient;
        ProgressManager _progressManager;

        public HowToPlayScreen(GraphicsDevice graphicsDevice, MainMenuManager menuManager, ProgressManager progressManager, Game_Client gameClient)
        {
            _gameClient = gameClient;
            _graphicsDevice = graphicsDevice;
            _progressManager = progressManager;
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _keyBoardControllerKeys = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), _buttonPosition, Color.Green, Color.Gray, "Show controller keys");
            _returnToMain = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), _buttonPosition + new Vector2(0, _buttonHeight + 2), Color.Green, Color.Gray, "Return to main menu");
            _menuManager = menuManager;
        }
        public void Update(GameTime gameTime)
        {
            if (_keyBoardControllerKeys.Update(gameTime))
            {
                _menuManager._showChooseCharacterMenu = true;
                _menuManager._showSelectSaveFileMenu = false;
            }
            if (_returnToMain.Update(gameTime))
            {
                _menuManager._showSelectSaveFileMenu = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _keyBoardControllerKeys.Draw(spriteBatch);
            _returnToMain.Draw(spriteBatch);
        }
        public void ResetGraphics()
        {
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _keyBoardControllerKeys.ResetGraphics(_buttonPosition );
            _returnToMain.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight + 2));
        }
    }
}
