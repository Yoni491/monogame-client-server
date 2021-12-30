using GameClient.UI;
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
        ScreenPoint _buttonPosition;
        private GraphicsDevice _graphicsDevice;
        private MainMenuScreen _menuManager;
        int _buttonHeight = 60;
        int _buttonWeight = 250;
        Game_Client _gameClient;
        ProgressManager _progressManager;
        public SelectSaveFileScreen(GraphicsDevice graphicsDevice, MainMenuScreen menuManager, ProgressManager progressManager, Game_Client gameClient)
        {
            _gameClient = gameClient;
            _graphicsDevice = graphicsDevice;
            _progressManager = progressManager;
            _buttonPosition = new ScreenPoint(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _continueGame = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), Vector2.Zero, _buttonPosition, Color.Green, Color.Gray, "Continue game");
            _newGame = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), new Vector2(0, _buttonHeight + 2), _buttonPosition, Color.Green, Color.Gray, "New game");
            _returnToMain = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), new Vector2(0, _buttonHeight * 2 + 4), _buttonPosition, Color.Green, Color.Gray, "Return to main menu");
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
                    _menuManager._showSelectSaveFileScreen = false;
                }
            }
            if (_newGame.Update(gameTime))
            {
                _menuManager._showChooseCharacterScreen = true;
                _menuManager._showSelectSaveFileScreen = false;
            }
            if (_returnToMain.Update(gameTime))
            {
                _menuManager._showSelectSaveFileScreen = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (ProgressManager._saveFileAvailable)
                _continueGame.ChangeColor(Color.Green);
            else
                _continueGame.ChangeColor(Color.Gray);
            _continueGame.Draw(spriteBatch);
            _newGame.Draw(spriteBatch);
            _returnToMain.Draw(spriteBatch);
        }
        public void ResetGraphics()
        {
            _buttonPosition.vector2.X = _graphicsDevice.Viewport.Bounds.Width / 2 - 120;
            _buttonPosition.vector2.Y = _graphicsDevice.Viewport.Bounds.Height / 2 - 30;
            _continueGame.ResetGraphics();
            _newGame.ResetGraphics();
            _returnToMain.ResetGraphics();
        }
    }
}
