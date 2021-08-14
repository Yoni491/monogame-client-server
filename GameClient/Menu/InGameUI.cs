using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    class InGameUI
    {
        SpriteFont _font;
        Vector2 _position;
        int _levelShowing;
        public string _text { get; set; }
        Color _background;
        GraphicsDevice _graphicsDevice;

        public InGameUI(GraphicsDevice graphicDevice)
        {
            _graphicsDevice = graphicDevice;
            _font = GraphicManager.GetBasicFont("basic_22");
            _position = new Vector2(100,0);
            _background = new Color(Color.Black, 0.1f);

        }
        public void Update()
        {
            if (_levelShowing != LevelManager._currentLevel)
            {
                _levelShowing = LevelManager._currentLevel;
                _text = "Level: " + _levelShowing.ToString();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!string.IsNullOrEmpty(_text))
            {
                spriteBatch.DrawString(_font, _text, _position, Color.White, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0.99f);
            }
        }
        public void ResetGraphics()
        {

        }
        public void Initialize()
        {

        }
    }
}
