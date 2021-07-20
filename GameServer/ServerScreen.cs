using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using GameClient;
using Microsoft.Xna.Framework;

namespace GameServer
{
    public class ServerScreen
    {
        ScreenMassage _headLine,_waitingMassage;
        private Texture2D _background;

        public ServerScreen(GraphicsDevice graphicsDevice)
        {
            _headLine = new ScreenMassage(graphicsDevice, "GAME SERVER");
            _waitingMassage = new ScreenMassage(graphicsDevice, "Waiting for connection...",100);
            _background = GraphicManager._contentManager.Load<Texture2D>("Images/matrix");

        }
        public void Update()
        {

        }
        public void UpdateMassage(string text)
        {
            _waitingMassage.Text(text);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            int height = GraphicManager.screenHeight;
            int width = GraphicManager.screenWidth;
            spriteBatch.Draw(_background, new Rectangle(0, 0, width, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.1f);

            _headLine.Draw(spriteBatch);
            _waitingMassage.Draw(spriteBatch);
        }
    }
}
