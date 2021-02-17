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
        Button _settingButton, _fullScreenButton, _exitFullScreenButton,_returnToGame,_exitToMain;
        private Game_Client _game_client;
        GraphicsDeviceManager _graphics;
        private Player _player;
        bool fullScreen;
        public static bool _showSettings;
        private Texture2D _settingsBackground;
        int _buttonHeight = 30;
        Vector2 _buttonsPosition;
        public UIManager()
        {

        }
        public void Initialize(ContentManager content, InventoryManager InventoryManager, GraphicsDeviceManager graphics, Player player, Game_Client game_client)
        {
            _game_client = game_client;
            _graphics = graphics;
            _player = player;
            _InventoryManager = InventoryManager;
            _settingButton = new Button(content.Load<Texture2D>("etc/settings"), null, new Vector2(0, 0), Color.White, Color.Gray, null);
            _buttonsPosition = new Vector2(GraphicManager.screenWidth / 3, GraphicManager.screenHeight / 3);
            _fullScreenButton = new Button(GraphicManager.getRectangleTexture(100, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonsPosition, Color.Green, Color.Gray, "Full Screen");
            _exitFullScreenButton = new Button(GraphicManager.getRectangleTexture(110, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonsPosition, Color.Green, Color.Gray, "Exit full Screen");
            _returnToGame = new Button(GraphicManager.getRectangleTexture(130, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonsPosition * 1.9f, Color.Blue, Color.Gray, "Return to game");
            _exitToMain = new Button(GraphicManager.getRectangleTexture(130, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonsPosition + new Vector2(0,GraphicManager.screenHeight / 4), Color.DarkRed, Color.Gray, "Exit To Menu");
            _settingsBackground = GraphicManager._contentManager.Load<Texture2D>("Images/settings_background");
        }
        public void Update(GameTime gameTime)
        {
            _InventoryManager.MouseClick();
            if (_showSettings)
            {
                if (!fullScreen)
                {
                    if (_fullScreenButton.Update(gameTime))
                    {

                        _player._clickedOnUi = true;
                        fullScreen = true;
                        GraphicManager.ChangeToFullScreen(true);
                        _game_client.ResetGraphics();
                    }
                }
                else
                {
                    if (_exitFullScreenButton.Update(gameTime))
                    {

                        _player._clickedOnUi = true;
                        fullScreen = false;
                        GraphicManager.ChangeToFullScreen(false);
                        _game_client.ResetGraphics();
                    }
                }
                
                if (_settingButton.Update(gameTime))
                {
                    _showSettings = false;
                    _player._clickedOnUi = true;
                }
                if(_returnToGame.Update(gameTime))
                {
                    _showSettings = false;
                    _player._clickedOnUi = true;
                }
                if(_exitToMain.Update(gameTime))
                {
                    _game_client._inMenu = true;
                }
            }
            else
            {
                if (_settingButton.Update(gameTime))
                {
                    _showSettings = true;
                    _player._clickedOnUi = true;
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
                if(fullScreen)
                {
                    _exitFullScreenButton.Draw(spriteBatch);
                }
                else
                {
                    _fullScreenButton.Draw(spriteBatch);
                }
                _returnToGame.Draw(spriteBatch);
                _exitToMain.Draw(spriteBatch);
            }
        }
        public void ResetGraphics()
        {
            _buttonsPosition = new Vector2(GraphicManager.screenWidth / 3, GraphicManager.screenHeight / 3);
            _fullScreenButton.ResetGraphics(_buttonsPosition);
            _exitFullScreenButton.ResetGraphics(_buttonsPosition);
            _returnToGame.ResetGraphics(_buttonsPosition * 1.9f);
            
        }
    }
}
