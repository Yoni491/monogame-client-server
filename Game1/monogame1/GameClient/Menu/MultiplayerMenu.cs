using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient
{
    public class MultiplayerMenu
    {

        Button _returnToMain;
        Button _connect;
        Vector2 _buttonPosition;
        private GraphicsDevice _graphicsDevice;
        private Game_Client _game_Client;
        private MainMenuManager _menuManager;
        private bool connecting;
        
        public MultiplayerMenu(GraphicsDevice graphicsDevice, Game_Client game_Client, MainMenuManager menuManager)
        {
            _buttonPosition = new Vector2(graphicsDevice.Viewport.Bounds.Width / 2, graphicsDevice.Viewport.Bounds.Height / 2);
            _connect = new Button(GraphicManager.getRectangleTexture(160, 30, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, 12), Color.Green, Color.Gray, "Connect to server");
            _returnToMain = new Button(GraphicManager.getRectangleTexture(160, 30, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, 100), Color.Green, Color.Gray, "Return to main menu");
            _graphicsDevice = graphicsDevice;
            _game_Client = game_Client;
            _menuManager = menuManager;
        }
        public void Update(GameTime gameTime)
        {
            if (!UIManager._showSettings)
            {
                if (_returnToMain.Update(gameTime))
                {
                    _menuManager._showMultiplayerMenu = false;
                    _menuManager._showChooseCharacterMenu = false;
                    _game_Client._networkManager.CloseConnection();
                }
                if (_connect.Update(gameTime) && !connecting)
                {
                    connecting = true;
                    _game_Client._networkManager.Initialize_connection();
                }
            }
            if (MainMenuManager._connected)
            {
                connecting = false;
                _menuManager._showMultiplayerMenu = false;
                _menuManager._showChooseCharacterMenu = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _connect.Draw(spriteBatch);
            _returnToMain.Draw(spriteBatch);
        }
        public void ResetGraphics()
        {
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2, _graphicsDevice.Viewport.Bounds.Height / 2);
            _connect.ResetGraphics(_buttonPosition + new Vector2(0, 12));
            _returnToMain.ResetGraphics(_buttonPosition + new Vector2(0, 100));
        }
    }
}
