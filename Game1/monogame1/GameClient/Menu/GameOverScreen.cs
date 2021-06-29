using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace GameClient
{
    class GameOverScreen
    {
        InventoryManager _InventoryManager;
        //Rectangle settingsRectangle;
        Button _RestartLevel, _exitToMain;
        GraphicsDevice _graphicsDevice;
        private Texture2D _gameOverBackground;
        int _buttonHeight = 50, _buttonWeight = 200;
        Vector2 _buttonPosition;

        public GameOverScreen()
        {

        }
        public void Initialize(ContentManager content, InventoryManager InventoryManager, GraphicsDevice graphics)
        {
            _graphicsDevice = graphics;
            _InventoryManager = InventoryManager;
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 150);
            _RestartLevel = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, _buttonHeight + 2), Color.Green, Color.Gray, "Full screen");
            _exitToMain = new Button(GraphicManager.getRectangleTexture(130, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition + new Vector2(0, _buttonHeight * 4 + 8), Color.DarkRed, Color.Gray, "Exit To Menu");
            _gameOverBackground = GraphicManager._contentManager.Load<Texture2D>("Images/settings_background");
            AudioManager.PlaySong(menu: true);
            //_exitFullScreenButton = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition, Color.Green, Color.Gray, "Exit full Screen");
        }
        public void Update(GameTime gameTime)
        {
                if (_RestartLevel.Update(gameTime))
                {
                    
                }

                if (_exitToMain.Update(gameTime))
                {
                    Game_Client._inMenu = true;
                    AudioManager.PlaySong(menu: true);
                }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            int height = GraphicManager.screenHeight / 2;
            int width = GraphicManager.screenWidth / 2;
            spriteBatch.Draw(_gameOverBackground, new Rectangle(GraphicManager.screenWidth / 4, GraphicManager.screenHeight / 4, width, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.1f);
            _RestartLevel.Draw(spriteBatch);
            _exitToMain.Draw(spriteBatch);
        }
        public void ResetGraphics()
        {
            _buttonPosition = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 150);
            _RestartLevel.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight + 2));
            _exitToMain.ResetGraphics(_buttonPosition + new Vector2(0, _buttonHeight * 4 + 8));
        }
        public bool MouseClick()
        {
            return false;
        }
    }
}
