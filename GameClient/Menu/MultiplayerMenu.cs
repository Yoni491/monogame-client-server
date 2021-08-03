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
        private TextInputBox _textInputBox;
        private ScreenMassage _enterIPMassage;
        public bool _connecting;
        int _buttonHeight = 60;
        int _buttonWeight = 250;
        
        public MultiplayerMenu(GraphicsDevice graphicsDevice, Game_Client game_Client, MainMenuManager menuManager)
        {
            _graphicsDevice = graphicsDevice;
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _textInputBox = new TextInputBox(_buttonPosition, true);
            _enterIPMassage = new ScreenMassage(graphicsDevice,"Enter IP:",_buttonPosition + new Vector2(-130, -10));
            _connectButton = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), _buttonPosition + new Vector2(0, _buttonHeight + 2), Color.Green, Color.Gray, "Connect to server");
            _returnToMain = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), _buttonPosition + new Vector2(0, _buttonHeight*2 + 4), Color.Green, Color.Gray, "Return to main menu");
            _game_Client = game_Client;
            _menuManager = menuManager;
        }
        public void Update(GameTime gameTime)
        {
            _textInputBox.Update();
            if (_returnToMain.Update(gameTime))
            {
                _menuManager._showMultiplayerMenu = false;
                _game_Client._networkManager.CloseConnection();
            }
            if (_connectButton.Update(gameTime) && !_connecting)
            {
                _connecting = true;
                _game_Client._networkManager.Initialize_connection(_textInputBox._text);
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
            if (_connecting)
            {
                _connectButton.ChangeColor(Color.Orange);
                _connectButton.ChangeText("Trying to connect...");
            }
            else
            {
                _connectButton.ChangeText("Connect to server");
                _connectButton.ChangeColor(Color.Green);
            }
            _enterIPMassage.Draw(spriteBatch);
            _textInputBox.Draw(spriteBatch);
            _connectButton.Draw(spriteBatch);
            _returnToMain.Draw(spriteBatch);
        }
        public void ResetGraphics()
        {
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _textInputBox.ResetGraphics(_buttonPosition);
            _connectButton.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight + 2));
            _returnToMain.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight*2 + 4));
        }
    }
}
