using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient
{
    public class MultiplayerMenu
    {

        Button _returnToMain, _connectButton;
        Vector2 _buttonPosition;
        private GraphicsDevice _graphicsDevice;
        private Game_Client _game_Client;
        private MainMenuManager _menuManager;
        private bool _connecting;
        int _buttonHeight = 60;
        int _buttonWeight = 250;
        
        public MultiplayerMenu(GraphicsDevice graphicsDevice, Game_Client game_Client, MainMenuManager menuManager)
        {
            _graphicsDevice = graphicsDevice;
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _connectButton = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition, Color.Green, Color.Gray, "Connect to server");
            _returnToMain = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, _buttonHeight + 2), Color.Green, Color.Gray, "Return to main menu");
            _game_Client = game_Client;
            _menuManager = menuManager;
        }
        public void Update(GameTime gameTime)
        {
            if (_returnToMain.Update(gameTime))
            {
                _menuManager._showMultiplayerMenu = false;
                _game_Client._networkManager.CloseConnection();
            }
            if (_connectButton.Update(gameTime) && !_connecting)
            {
                _connecting = true;
                _game_Client._networkManager.Initialize_connection();
            }
            if (MainMenuManager._connected)
            {
                _connecting = false;
                _menuManager._showMultiplayerMenu = false;
                _menuManager._showChooseCharacterMenu = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _connectButton.Draw(spriteBatch);
            _returnToMain.Draw(spriteBatch);
        }
        public void ResetGraphics()
        {
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _connectButton.ResetGraphics(_buttonPosition);
            _returnToMain.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight + 2));
        }
    }
}
