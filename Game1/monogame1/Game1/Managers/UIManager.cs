using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient
{
    public class UIManager
    {
        Texture2D _SettingsButton;
        InventoryManager _InventoryManager;
        Rectangle settingsRectangle;
        Texture2D _menuBackgroundImage;
        Button _fullScreenButton;
        GraphicsDeviceManager _graphics;
        bool ShowSettings;
        public UIManager(ContentManager content, InventoryManager InventoryManager,GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
            _SettingsButton = content.Load<Texture2D>("etc/settings");
            _InventoryManager = InventoryManager;
            settingsRectangle = new Rectangle(0, 0, _SettingsButton.Width, _SettingsButton.Height);
            _fullScreenButton = new Button(GraphicManager.getRectangleTexture(100, 200, Color.White), GraphicManager.GetBasicFont(), GraphicManager._ScreenMiddle, Color.Green, Color.Gray, "Full Screen");
        }
        public void Update(GameTime gameTime)
        {
            if (ShowSettings)
                if (_fullScreenButton.Update(gameTime))
                {
                    _graphics.IsFullScreen = true;
                    _graphics.ApplyChanges();
                }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_SettingsButton, settingsRectangle, Color.White);
            if(ShowSettings)
                _fullScreenButton.Draw(spriteBatch);
        }
        public bool MouseClick()
        {
            if (CollisionManager.isMouseCollidingRectangle(settingsRectangle))
            {
                ShowSettings = true;
                return true;
            }

            if (_InventoryManager.MouseClick())
                return true;
            return false;
        }
    }
}
