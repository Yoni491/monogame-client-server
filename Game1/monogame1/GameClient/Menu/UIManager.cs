using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient
{
    public class UIManager
    {
        InventoryManager _InventoryManager;
        //Rectangle settingsRectangle;
        Button _settingButton, _fullScreenButton,_returnToGame,_exitToMain,_muteSoundButton,_muteMusicButton;
        GraphicsDevice _graphicsDevice;
        bool _fullScreen,_soundMuted,_musicMuted;
        public static bool _showSettings;
        private Texture2D _settingsBackground;
        int _buttonHeight = 50 , _buttonWeight = 200;
        Vector2 _buttonPosition;

        public UIManager()
        {

        }
        public void Initialize(ContentManager content, InventoryManager InventoryManager, GraphicsDevice graphics)
        {
            _graphicsDevice = graphics;
            _InventoryManager = InventoryManager;
            _settingButton = new Button(content.Load<Texture2D>("etc/settings"), null, new Vector2(0, 0), Color.White, Color.Gray, null);
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _returnToGame = new Button(GraphicManager.getRectangleTexture(130, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition, Color.Blue, Color.Gray, "Return to game");
            _fullScreenButton = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0,_buttonHeight + 2), Color.Green, Color.Gray, "Full Screen");
            //_exitFullScreenButton = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition, Color.Green, Color.Gray, "Exit full Screen");
            _muteMusicButton = new Button(GraphicManager.getRectangleTexture(130, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, _buttonHeight*2 + 4), Color.DarkRed, Color.Gray, "Mute music");
            _muteSoundButton = new Button(GraphicManager.getRectangleTexture(130, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, _buttonHeight*3 + 6), Color.DarkRed, Color.Gray, "Mute sound");
            _exitToMain = new Button(GraphicManager.getRectangleTexture(130, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, _buttonHeight * 4 + 8), Color.DarkRed, Color.Gray, "Exit To Menu");
            _settingsBackground = GraphicManager._contentManager.Load<Texture2D>("Images/settings_background");
            AudioManager.PlaySong(menu: true);
        }
        public void Update(GameTime gameTime)
        {
            _InventoryManager.MouseClick();
            _InventoryManager.MouseRightClick();
            if (_showSettings)
            {
                if (!_fullScreen)
                {
                    if (_fullScreenButton.Update(gameTime))
                    {
                        _fullScreen = true;
                        GraphicManager.ChangeToFullScreen(true);
                        Game_Client.ResetGraphics();
                    }
                }
                else
                {
                    //if (_exitFullScreenButton.Update(gameTime))
                    //{
                    //    _fullScreen = false;
                    //    GraphicManager.ChangeToFullScreen(false);
                    //    Game_Client.ResetGraphics();
                    //}
                }
                
                if (_settingButton.Update(gameTime))
                {
                    _showSettings = false;
                }
                if(_returnToGame.Update(gameTime))
                {
                    _showSettings = false;
                }
                if(_exitToMain.Update(gameTime))
                {
                    Game_Client._inMenu = true;
                    AudioManager.PlaySong(menu: true);
                }
                if (_muteSoundButton.Update(gameTime))
                {
                    if(_soundMuted)
                    {
                        AudioManager.MuteSound(false);
                    }
                    else
                    {
                        AudioManager.MuteSound(true);
                    }
                    _soundMuted = !_soundMuted;
                }
                if (_muteMusicButton.Update(gameTime))
                {
                    if (_musicMuted)
                    {
                        AudioManager.MuteMusic(false);
                    }
                    else
                    {
                        AudioManager.MuteMusic(true);
                    }
                    _musicMuted = !_musicMuted;
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
                int height = GraphicManager.screenHeight / 2;
                int width = GraphicManager.screenWidth / 2;
                spriteBatch.Draw(_settingsBackground,new Rectangle(GraphicManager.screenWidth/4, GraphicManager.screenHeight/4, width, height),null,Color.White,0,Vector2.Zero,SpriteEffects.None,0.1f);
                if(_fullScreen)
                {
                    //_exitFullScreenButton.Draw(spriteBatch);
                }
                else
                {
                    _fullScreenButton.Draw(spriteBatch);
                }
                _returnToGame.Draw(spriteBatch);
                _exitToMain.Draw(spriteBatch);
                _muteMusicButton.Draw(spriteBatch);
                _muteSoundButton.Draw(spriteBatch);
            }
        }
        public void ResetGraphics()
        {
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 30);
            _returnToGame.ResetGraphics(_buttonPosition);
            _fullScreenButton.ResetGraphics(_buttonPosition);
            _muteMusicButton.ResetGraphics(_buttonPosition);
            _muteSoundButton.ResetGraphics(_buttonPosition);
            _exitToMain.ResetGraphics(_buttonPosition);
        }    
        public bool MouseClick()
        {    
            return false;
        }
    }
}
