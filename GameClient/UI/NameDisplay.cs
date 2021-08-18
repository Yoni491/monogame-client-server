using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient
{
    public class NameDisplay
    {
        SpriteFont _font;
        Vector2 _position;
        public string _text { get; set; }
        Color _background;
        GraphicsDevice _graphicsDevice;
        
        public NameDisplay(GraphicsDevice graphicDevice, string text,Vector2? positon = null,int positionOffsetY = 0)
        {
            _graphicsDevice = graphicDevice;
            _font = GraphicManager.GetBasicFont("basic_16");
            if (positon != null)
            {
                _position = (Vector2)positon;
            }
            _text = text;
            _background = new Color(Color.Black, 0.1f);

        }
       public void Update(Vector2 position)
        {
            _position = new Vector2((int)position.X, (int)position.Y);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!string.IsNullOrEmpty(_text))
            {
                int x = (int)(_font.MeasureString(_text).X * 0.7f)/2;
                spriteBatch.DrawString(_font, _text, _position + new Vector2(-x, 0), Color.White, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.99f);
            }
        }
        public void ResetGraphics()
        {
            _position = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 4, _graphicsDevice.Viewport.Bounds.Height / 4);
        }
    }
}
