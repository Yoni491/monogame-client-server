using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient
{
    public class MultiplayerScreen
    {
        SettingsDataManager _settingsDataManager;
        Button _returnToMain, _connectButton;
        Vector2 _buttonPosition;
        private GraphicsDevice _graphicsDevice;
        private Game_Client _game_Client;
        private MainMenuScreen _menuManager;
        public TextInputBox _IPtextBox;
        private ScreenMessage _enterIPMessage;
        public bool _connecting;
        int _buttonHeight = 60;
        int _buttonWeight = 250;
        
        public MultiplayerScreen(GraphicsDevice graphicsDevice, Game_Client game_Client, MainMenuScreen menuManager, SettingsDataManager settingsDataManager)
        {
            _settingsDataManager = settingsDataManager;
            _graphicsDevice = graphicsDevice;
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _IPtextBox = new TextInputBox(_buttonPosition, true);
            _enterIPMessage = new ScreenMessage(graphicsDevice,"Enter IP:",_buttonPosition + new Vector2(-130, -10));
            _connectButton = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), _buttonPosition + new Vector2(0, _buttonHeight + 2), Color.Green, Color.Gray, "Connect to server");
            _returnToMain = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), _buttonPosition + new Vector2(0, _buttonHeight*2 + 4), Color.Green, Color.Gray, "Return to main menu");
            _game_Client = game_Client;
            _menuManager = menuManager;
        }
        public void Update(GameTime gameTime)
        {
            _IPtextBox.Update();
            if (_returnToMain.Update(gameTime))
            {
                _settingsDataManager.CreateSettingsData();
                _menuManager._showMultiplayerMenu = false;
                _game_Client._networkManager.CloseConnection();
            }
            if (_connectButton.Update(gameTime) && !_connecting)
            {
                _settingsDataManager.CreateSettingsData();
                _connecting = true;
                _game_Client._networkManager.Initialize_connection(_IPtextBox._text);
            }
            if (MainMenuScreen._connected)
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
            _enterIPMessage.Draw(spriteBatch);
            _IPtextBox.Draw(spriteBatch);
            _connectButton.Draw(spriteBatch);
            _returnToMain.Draw(spriteBatch);
        }
        public void ResetGraphics()
        {
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _IPtextBox.ResetGraphics(_buttonPosition);
            _connectButton.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight + 2));
            _returnToMain.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight*2 + 4));
            _enterIPMessage.ResetGraphics( _buttonPosition + new Vector2(-130, -10));
        }
    }
}
