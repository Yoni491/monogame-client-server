using GameClient.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameClient
{
    class HowToPlayScreen
    {
        Button _keyBoardControllerKeys, _returnToMain;
        ScreenPoint _buttonPosition;
        private GraphicsDevice _graphicsDevice;
        private MainMenuScreen _menuManager;
        int _buttonHeight = 60;
        int _buttonWeight = 300;
        Game_Client _gameClient;
        ProgressManager _progressManager;
        Texture2D _keyboardMouse, _gamePad;
        bool _showKeyBoardKeys = true;
        Rectangle _keysRectangle;

        public HowToPlayScreen(GraphicsDevice graphicsDevice, MainMenuScreen menuManager, ProgressManager progressManager, Game_Client gameClient)
        {
            _gameClient = gameClient;
            _graphicsDevice = graphicsDevice;
            _progressManager = progressManager;
            _buttonPosition = new ScreenPoint(_graphicsDevice.Viewport.Bounds.Width / 1.4f, _graphicsDevice.Viewport.Bounds.Height / 2f);
            _keyBoardControllerKeys = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), Vector2.Zero, _buttonPosition, Color.Green, Color.Gray, "Show controller keys");
            _returnToMain = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), new Vector2(0, _buttonHeight + 2), _buttonPosition, Color.Green, Color.Gray, "Return to main menu");
            _menuManager = menuManager;
            _keyboardMouse = GraphicManager.getImage("‏‏‫KeyboardMouseKeys");
            _gamePad = GraphicManager.getImage("GamePadKeys");
            _keysRectangle = new Rectangle(50, (int)(_graphicsDevice.Viewport.Bounds.Height / 4f), 800, 500);
        }
        public void Update(GameTime gameTime)
        {
            if (_keyBoardControllerKeys.Update(gameTime))
            {
                _showKeyBoardKeys = !_showKeyBoardKeys;
                if (_showKeyBoardKeys)
                {
                    _keyBoardControllerKeys.ChangeText("controller keys");
                }
                else
                {
                    _keyBoardControllerKeys.ChangeText("keyboard and mouse keys");
                }
            }
            if (_returnToMain.Update(gameTime))
            {
                _menuManager._showHowToPlay = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            if (_showKeyBoardKeys)
                spriteBatch.Draw(_keyboardMouse, _keysRectangle, Color.White);
            else
                spriteBatch.Draw(_gamePad, _keysRectangle, Color.White);

            _keyBoardControllerKeys.Draw(spriteBatch);
            _returnToMain.Draw(spriteBatch);
        }
        public void ResetGraphics()
        {
            _keysRectangle = new Rectangle(50, (int)(_graphicsDevice.Viewport.Bounds.Height / 4f), 800, 500);
            _buttonPosition.vector2.X = (int)(_graphicsDevice.Viewport.Bounds.Width / 1.4f);
            _buttonPosition.vector2.Y = (int)(_graphicsDevice.Viewport.Bounds.Height / 2f);
            _keyBoardControllerKeys.ResetGraphics();
            _returnToMain.ResetGraphics();
        }
    }
}
