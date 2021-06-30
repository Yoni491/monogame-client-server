using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class SelectSaveFileScreen
    {
        Button _newGame, _continueGame, _returnToMain;
        Vector2 _buttonPosition;
        private GraphicsDevice _graphicsDevice;
        private MainMenuManager _menuManager;
        int _buttonHeight = 60;
        int _buttonWeight = 250;
        Game_Client _gameClient;
        ProgressManager _progressManager;

        public SelectSaveFileScreen(GraphicsDevice graphicsDevice, MainMenuManager menuManager, ProgressManager progressManager,Game_Client gameClient)
        {
            _gameClient = gameClient;
            _graphicsDevice = graphicsDevice;
            _progressManager = progressManager;
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _continueGame = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, 0), Color.Green, Color.Gray, "Continue game");
            _newGame = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, _buttonHeight + 2), Color.Green, Color.Gray, "New game");
            _returnToMain = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, _buttonHeight*2 + 4), Color.Green, Color.Gray, "Return to main menu");
            _menuManager = menuManager;
        }
        public void Update(GameTime gameTime)
        {
            if (ProgressManager._saveFileAvailable)
            {
                if (_continueGame.Update(gameTime))
                {
                    _progressManager.LoadData();
                    Game_Client._inMenu = false;
                    _menuManager._showSelectSaveFileMenu = false;
                }
            }
            if (_newGame.Update(gameTime))
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
            if(ProgressManager._saveFileAvailable)
                _continueGame.ChangeColor(Color.Green);
            else
                _continueGame.ChangeColor(Color.Gray);
            _continueGame.Draw(spriteBatch);
            _newGame.Draw(spriteBatch);
            _returnToMain.Draw(spriteBatch);
        }
        public void ResetGraphics()
        {
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _continueGame.ResetGraphics(_buttonPosition);
            _newGame.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight + 2));
            _returnToMain.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight*2 + 4));
        }
    }
}
