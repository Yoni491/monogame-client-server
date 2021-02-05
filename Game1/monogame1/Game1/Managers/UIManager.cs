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
        Texture2D _menuBackgroundImage;
        Button _settingButton, _fullScreenButton, _exitFullScreenButton;
        GraphicsDeviceManager _graphics;
        private Player _player;
        bool fullScreen;
        bool ShowSettings;

        public UIManager()
        {
            
        }
        public void Initialize(ContentManager content, InventoryManager InventoryManager, GraphicsDeviceManager graphics, Player player)
        {
            _graphics = graphics;
            _player = player;
            _InventoryManager = InventoryManager;
            _settingButton = new Button(content.Load<Texture2D>("etc/settings"), null, new Vector2(0, 0), Color.White, Color.Gray, null);
            _fullScreenButton = new Button(GraphicManager.getRectangleTexture(100, 100, Color.White), GraphicManager.GetBasicFont(), GraphicManager._ScreenMiddle, Color.Green, Color.Gray, "Full Screen");
            _exitFullScreenButton = new Button(GraphicManager.getRectangleTexture(100, 100, Color.White), GraphicManager.GetBasicFont(), GraphicManager._ScreenMiddle, Color.Green, Color.Gray, "Exit full Screen");
        }
        public void Update(GameTime gameTime)
        {
            _InventoryManager.MouseClick();
            if (ShowSettings)
            {
                if (!fullScreen)
                {
                    if (_fullScreenButton.Update(gameTime))
                    {

                        _player._clickedOnUi = true;
                        fullScreen = true;
                        GraphicManager.ChangeToFullScreen(true);
                        _InventoryManager.ResetGraphics();
                    }
                }
                else
                {
                    if (_exitFullScreenButton.Update(gameTime))
                    {

                        _player._clickedOnUi = true;
                        fullScreen = false;
                        GraphicManager.ChangeToFullScreen(false);
                        _InventoryManager.ResetGraphics();
                    }
                }
                
                if (_settingButton.Update(gameTime))
                {
                    ShowSettings = false;
                    _player._clickedOnUi = true;
                }
            }
            else
            {
                if (_settingButton.Update(gameTime))
                {
                    ShowSettings = true;
                    _player._clickedOnUi = true;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _settingButton.Draw(spriteBatch);
            if (ShowSettings)
            {
                if(fullScreen)
                {
                    _exitFullScreenButton.Draw(spriteBatch);
                }
                else
                {
                    _fullScreenButton.Draw(spriteBatch);
                }
            }
        }
    }
}
