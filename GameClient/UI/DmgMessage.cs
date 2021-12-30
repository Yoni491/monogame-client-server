using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GameClient
{
    public class DmgMessage
    {
        SpriteFont _font;
        Vector2 _position;
        public string _text { get; set; }
        Color _color;
        GraphicsDevice _graphicsDevice;
        float _timeDisplayed, _timer = 0, _scale;
        public bool _destroy;
        public DmgMessage(GraphicsDevice graphicDevice, int dmg, Vector2 positon, float timeDisplayed, Color color, float scale)
        {
            _graphicsDevice = graphicDevice;
            _font = GraphicManager.GetBasicFont("basic_16");
            _scale = scale;
            if (Color.Gold == color)
            {
                _text = "+" + dmg.ToString();
            }
            else
            {
                _text = dmg.ToString();
            }
            _position = positon;
            _timeDisplayed = timeDisplayed;
            _color = color;
        }
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= _timeDisplayed)
            {
                _destroy = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!string.IsNullOrEmpty(_text))
            {
                int x = (int)(_font.MeasureString(_text).X) / 2;
                spriteBatch.DrawString(_font, _text, _position + new Vector2(-x, 0), _color, 0, Vector2.Zero, _scale, SpriteEffects.None, 0.99f);
            }
        }
    }
}
