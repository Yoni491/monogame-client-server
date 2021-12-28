using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using GameClient;
using Microsoft.Xna.Framework;
using System.Net;
using Microsoft.Xna.Framework.Input;
using GameClient.UI;

namespace GameServer
{
    public class ServerScreen
    {
        ScreenMessage _headLine, _waitingMessage, _ip;
        private Texture2D _background;
        private ScreenMessage _startingLevelMessage;
        public static TextInputBox _startingLevelTextBox;
        private ScreenPoint _buttonPosition;

        private ScreenMessage _loadLevelMessage;
        public static TextInputBox _loadLevelTextBox;

        KeyboardState _prevState;
        bool _showLevelBox;
        LevelManager _levelManager;
        Game_Server _game_Server;
        public ServerScreen(GraphicsDevice graphicsDevice, LevelManager levelManager, Game_Server game_Server)
        {
            _game_Server = game_Server;
            _levelManager = levelManager;
            _buttonPosition = new ScreenPoint(graphicsDevice.Viewport.Width / 4, graphicsDevice.Viewport.Height / 5);

            _headLine = new ScreenMessage(graphicsDevice, "GAME SERVER", _buttonPosition, Vector2.Zero);
            _waitingMessage = new ScreenMessage(graphicsDevice, "Waiting for connection...", _buttonPosition, new Vector2(0, 50));
            _background = GraphicManager._contentManager.Load<Texture2D>("Images/matrix");
            string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
            var externalIp = IPAddress.Parse(externalIpString);
            _ip = new ScreenMessage(graphicsDevice, "IP:" + externalIp + " Port:" + "1994", _buttonPosition, new Vector2(0, 100));
            _startingLevelTextBox = new TextInputBox(new Vector2(200, 150), _buttonPosition, true, 50);
            _startingLevelMessage = new ScreenMessage(graphicsDevice, "Starting level:", _buttonPosition, new Vector2(0, 150));
            _loadLevelTextBox = new TextInputBox(new Vector2(190, 200), _buttonPosition, true, 50);
            _loadLevelMessage = new ScreenMessage(graphicsDevice, "Load level:", _buttonPosition, new Vector2(0, 200));
            _startingLevelTextBox._text = "1";
        }
        public void Update()
        {
            if (NetworkManagerServer._Connected)
            {
                _loadLevelTextBox.Update();
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _prevState.IsKeyDown(Keys.Enter))
                {
                    if (!string.IsNullOrEmpty(_loadLevelTextBox._text))
                    {
                        NetworkManagerServer._sendNames = true;
                        _game_Server.ResetGame(false);
                        _levelManager.LoadNewLevel(Int32.Parse(_loadLevelTextBox._text));
                    }
                    _loadLevelTextBox._text = "";
                }
            }
            else
            {
                _startingLevelTextBox.Update();
            }
            _prevState = Keyboard.GetState();

        }
        public void UpdateMessage(string text)
        {
            _waitingMessage.Text(text);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            int height = GraphicManager.screenHeight;
            int width = GraphicManager.screenWidth;
            spriteBatch.Draw(_background, new Rectangle(0, 0, width, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.1f);

            _headLine.Draw(spriteBatch);
            _waitingMessage.Draw(spriteBatch);
            _ip.Draw(spriteBatch);
            if (NetworkManagerServer._Connected)
            {
                _loadLevelMessage.Draw(spriteBatch);
                _loadLevelTextBox.Draw(spriteBatch);
            }
            else
            {
                _startingLevelMessage.Draw(spriteBatch);
                _startingLevelTextBox.Draw(spriteBatch);
            }
        }
    }
}
