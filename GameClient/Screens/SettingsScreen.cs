using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient
{
    public class SettingsScreen
    {
        Game_Client _game_Client;
        ProgressManager _progressManager;
        SettingsDataManager _settingsDataManager;
        Button _settingButton, _returnToGame, _exitToMain, _restartLevel;
        public Button _muteSoundButton, _muteMusicButton, _fullScreenButton;
        GraphicsDevice _graphicsDevice;
        public bool _fullScreenOFF,_soundOFF,_musicOFF;
        public static bool _showSettings;
        private Texture2D _settingsBackground;
        int _buttonHeight = 50 , _buttonWeight = 200;
        Vector2 _buttonPosition;

        public SettingsScreen()
        {

        }
        public void Initialize(Game_Client game_client, ContentManager content, GraphicsDevice graphics,ProgressManager progressManager,SettingsDataManager settingsDataManager)
        {
            _game_Client = game_client;
            _graphicsDevice = graphics;
            _progressManager = progressManager;
            _settingsDataManager = settingsDataManager;
            _settingButton = new Button(content.Load<Texture2D>("etc/settings"), new Vector2(0, 0), Color.White, Color.Gray, null);
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 150);
            _returnToGame = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), _buttonPosition, Color.Blue, Color.Gray, "Return to game");
            _restartLevel = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), _buttonPosition + new Vector2(0, _buttonHeight + 2), Color.Blue, Color.Gray, "Restart Level");
            _fullScreenButton = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), _buttonPosition + new Vector2(0, _buttonHeight * 2 + 4), Color.Green, Color.Gray, "Full screen");
            _muteMusicButton = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), _buttonPosition + new Vector2(0, _buttonHeight*3 + 6), Color.Green, Color.Gray, "Mute music");
            _muteSoundButton = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), _buttonPosition + new Vector2(0, _buttonHeight*4 +8), Color.Green, Color.Gray, "Mute sound");
            _exitToMain = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), _buttonPosition + new Vector2(0, _buttonHeight * 5 + 10), Color.DarkRed, Color.Gray, "Exit To Menu");
            _settingsBackground = GraphicManager._contentManager.Load<Texture2D>("Images/settingBackground");
            AudioManager.PlaySong(menu: true);
            //_exitFullScreenButton = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition, Color.Green, Color.Gray, "Exit full Screen");
        }
        public void Update(GameTime gameTime)
        {
            if (_showSettings)
            {
                if (_fullScreenButton.Update(gameTime))
                {
                    _fullScreenOFF = !_fullScreenOFF;
                    if (_fullScreenOFF)
                    {
                        _fullScreenButton.ChangeText("Exit full Screen");
                    }
                    else
                    {
                        _fullScreenButton.ChangeText("Full screen");
                    }
                    GraphicManager.ChangeToFullScreen(_fullScreenOFF);
                    Game_Client.ResetGraphics();
                    _settingsDataManager.CreateSettingsData();
                }                
                if (_settingButton.Update(gameTime))
                {
                    _showSettings = false;
                }
                if (!Game_Client._inMenu)
                {
                    if (!Game_Client._isMultiplayer)
                    {
                        if (_restartLevel.Update(gameTime))
                        {
                            _showSettings = false;
                            _progressManager.LoadData();
                        }
                    }
                    if (_returnToGame.Update(gameTime))
                    {
                        _showSettings = false;
                    }
                }
                if(_exitToMain.Update(gameTime))
                {
                    _game_Client.ResetGame();
                }
                if (_muteSoundButton.Update(gameTime))
                {
                    if(_soundOFF)
                    {
                        _muteSoundButton.ChangeText("Mute sound");
                        AudioManager.MuteSound(false);
                    }
                    else
                    {
                        _muteSoundButton.ChangeText("Unmute sound");
                        AudioManager.MuteSound(true);
                    }
                    _soundOFF = !_soundOFF;
                    _settingsDataManager.CreateSettingsData();
                }
                if (_muteMusicButton.Update(gameTime))
                {
                    if (_musicOFF)
                    {
                        _muteMusicButton.ChangeText("Mute music");
                        AudioManager.MuteMusic(false);
                    }
                    else
                    {
                        _muteMusicButton.ChangeText("Unmute music");
                        AudioManager.MuteMusic(true);
                    }
                    _musicOFF = !_musicOFF;
                    _settingsDataManager.CreateSettingsData();
                }
            }
            else
            {
                if (_settingButton.Update(gameTime))
                {
                    _showSettings = true;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _settingButton.Draw(spriteBatch);
            if (_showSettings)
            {
                int height = _buttonHeight * 6 + 70;
                int width = _buttonWeight + 60;
                spriteBatch.Draw(_settingsBackground,new Rectangle((int)_buttonPosition.X - 30, (int)_buttonPosition.Y - 30, width, height),null,Color.White,0,Vector2.Zero,SpriteEffects.None,0.1f);
                _fullScreenButton.Draw(spriteBatch);
                if (!Game_Client._inMenu)
                {
                    if (!Game_Client._isMultiplayer)
                        _restartLevel.ChangeColor(Color.Blue);
                    else
                        _restartLevel.ChangeColor(Color.Gray);
                    _restartLevel.Draw(spriteBatch);
                    _returnToGame.Draw(spriteBatch);
                }
                _exitToMain.Draw(spriteBatch);
                _muteMusicButton.Draw(spriteBatch);
                _muteSoundButton.Draw(spriteBatch);
            }
        }
        public void ResetGraphics()
        {
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 150);
            _returnToGame.ResetGraphics(_buttonPosition);
            _restartLevel.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight + 2));
            _fullScreenButton.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight*2 + 4));
            _muteMusicButton.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight * 3 + 6));
            _muteSoundButton.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight * 4 + 8));
            _exitToMain.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight * 5 + 10));
        }
        public bool MouseClick()
        {    
            return false;
        }
    }
}
