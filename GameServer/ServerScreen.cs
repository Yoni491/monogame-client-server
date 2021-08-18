using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using GameClient;
using Microsoft.Xna.Framework;
using System.Net;

namespace GameServer
{
    public class ServerScreen
    {
        ScreenMessage _headLine,_waitingMessage,_ip;
        private Texture2D _background;

        public ServerScreen(GraphicsDevice graphicsDevice)
        {
            _headLine = new ScreenMessage(graphicsDevice, "GAME SERVER");
            _waitingMessage = new ScreenMessage(graphicsDevice, "Waiting for connection...",positionOffsetY: 50);
            _background = GraphicManager._contentManager.Load<Texture2D>("Images/matrix");
            string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
            var externalIp = IPAddress.Parse(externalIpString);
            _ip = new ScreenMessage(graphicsDevice, "IP:" + externalIp + " Port:"+"1994", positionOffsetY: 100);

        }
        public void Update()
        {

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
        }
    }
}
