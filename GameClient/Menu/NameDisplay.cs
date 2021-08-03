using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient
{
    public class NameDisplay
    {
        SpriteFont _font;
        //Texture2D _texture;
        //Rectangle _rectangle;
        Vector2 _position;
        public string _text { get; set; }
        Color _background;
        GraphicsDevice _graphicsDevice;
        
        public NameDisplay(GraphicsDevice graphicDevice, string text,Vector2? positon = null,int positionOffsetY = 0)
        {
            _graphicsDevice = graphicDevice;
            //_texture = GraphicManager.getRectangleTexture(50, 20, Color.Black);
            _font = GraphicManager.GetBasicFont("basic_12");
            if (positon != null)
            {
                _position = (Vector2)positon;
            }
            _text = text;
            //_rectangle = new Rectangle((int)_position.X, (int)_position.Y, ((int)_font.MeasureString(_text).X + 20), ((int)_font.MeasureString(_text).Y + 20) );
            _background = new Color(Color.Black, 0.1f);

        }
       public void Update(Vector2 position)
        {
            _position = position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_texture, _rectangle, null, _background, 0, Vector2.Zero, SpriteEffects.None, 0.51f);

            if (!string.IsNullOrEmpty(_text))
            {
                int x = (int)(_font.MeasureString(_text).X)/2;
                spriteBatch.DrawString(_font, _text, _position + new Vector2(-x, 0), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.99f);
            }
        }
        public void ResetGraphics()
        {
            _position = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 4, _graphicsDevice.Viewport.Bounds.Height / 4); ;
            //_rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }
    }
}
