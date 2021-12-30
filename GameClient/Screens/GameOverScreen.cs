using GameClient.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
namespace GameClient
{
    class GameOverScreen
    {
        //Rectangle settingsRectangle;
        Button _restartLevel, _exitToMain;
        GraphicsDevice _graphicsDevice;
        private Texture2D _gameOverBackground;
        int _buttonHeight = 50, _buttonWeight = 200;
        ScreenPoint _buttonPosition;
        static public bool _showScreen;
        ProgressManager _progressManager;
        Game_Client _game_Client;
        List<Player> _players;
        public GameOverScreen()
        {
        }
        public void Initialize(Game_Client game_Client, ContentManager content, GraphicsDevice graphics, ProgressManager progressManager, List<Player> players)
        {
            _players = players;
            _game_Client = game_Client;
            _progressManager = progressManager;
            _graphicsDevice = graphics;
            _buttonPosition = new ScreenPoint(_graphicsDevice.Viewport.Bounds.Width / 2 - 120, _graphicsDevice.Viewport.Bounds.Height / 2 - 150);
            _restartLevel = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), new Vector2(0, _buttonHeight + 2), _buttonPosition, Color.Green, Color.Gray, "Restart level");
            _exitToMain = new Button(GraphicManager.getRectangleTexture(130, _buttonHeight, Color.White), new Vector2(0, _buttonHeight * 2 + 2), _buttonPosition, Color.DarkRed, Color.Gray, "Exit To Menu");
            _gameOverBackground = GraphicManager._contentManager.Load<Texture2D>("Images/matrix");
            AudioManager.PlaySong(menu: true);
            //_exitFullScreenButton = new Button(GraphicManager.getRectangleTexture(_buttonWeight, _buttonHeight, Color.White), GraphicManager.GetBasicFont(), _buttonPosition, Color.Green, Color.Gray, "Exit full Screen");
        }
        public void Update(GameTime gameTime)
        {
            if (!Game_Client._isMultiplayer)
            {
                _restartLevel.ChangeText("Restart level");
                if (_restartLevel.Update(gameTime))
                {
                    _showScreen = false;
                    _progressManager.LoadData();
                }
            }
            else
            {
                _restartLevel.ChangeText("Respawn");
                if (_restartLevel.Update(gameTime))
                {
                    _showScreen = false;
                    _players.ForEach(player =>
                    {
                        player._health._health_left = 200;
                        player._dead = false;
                    });
                }
            }
            if (_exitToMain.Update(gameTime))
            {
                _game_Client.ResetGame();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Game_Client._isMultiplayer)
                _restartLevel.ChangeColor(Color.Green);
            else
                _restartLevel.ChangeColor(Color.Gray);
            int height = GraphicManager.screenHeight / 2;
            int width = GraphicManager.screenWidth / 2;
            spriteBatch.Draw(_gameOverBackground, new Rectangle(GraphicManager.screenWidth / 4, GraphicManager.screenHeight / 4, width, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.1f);
            _restartLevel.Draw(spriteBatch);
            _exitToMain.Draw(spriteBatch);
        }
        public void ResetGraphics()
        {
            _buttonPosition.vector2.X = _graphicsDevice.Viewport.Bounds.Width / 2 - 120;
            _buttonPosition.vector2.Y = _graphicsDevice.Viewport.Bounds.Height / 2 - 150;
            _restartLevel.ResetGraphics();
            _exitToMain.ResetGraphics();
        }
        public bool MouseClick()
        {
            return false;
        }
    }
}
