using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class TextInputBox
    {
        Texture2D _texture;
        SpriteFont _font;
        Color _background;
        Rectangle _rectangle;
        KeyboardState keyboard;
        KeyboardState oldKeyboard;
        char key;
        Vector2 _position;
        public string _text;
        public TextInputBox(Vector2 position)
        {
            keyboard = Keyboard.GetState();
            _position = position;
            _texture = GraphicManager.getRectangleTexture(450, 50, Color.White);
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y - 5, 250, 40);
            _background = new Color(Color.White, 1f);
            _font = GraphicManager.GetBasicFont("basic_22");

        }
        public void Update()
        {
            keyboard = Keyboard.GetState();
            ConvertKeyboardInput(keyboard, oldKeyboard,out key);
            oldKeyboard = keyboard;
            if(key!= (char)0 && key != 'b')
            {
                _text = _text + key;
            }
            else if(key == 'b' && _text.Length > 0)
            {
                _text = _text.Remove(_text.Length-1,1);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, null, _background, 0, Vector2.Zero, SpriteEffects.None, 0.51f);

            if (!string.IsNullOrEmpty(_text))
            {
                spriteBatch.DrawString(_font, _text, _position + new Vector2(3,0), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.6f);
            }
        }
        public static bool ConvertKeyboardInput(KeyboardState keyboard, KeyboardState oldKeyboard, out char key)
        {
            Keys[] keys = keyboard.GetPressedKeys();
            bool shift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);

            if (keys.Length > 0 && !oldKeyboard.IsKeyDown(keys[0]))
            {
                switch (keys[0])
                {
                    //Decimal keys
                    case Keys.D0: if (shift) { key = '0'; } else { key = '0'; } return true;
                    case Keys.D1: if (shift) { key = '1'; } else { key = '1'; } return true;
                    case Keys.D2: if (shift) { key = '2'; } else { key = '2'; } return true;
                    case Keys.D3: if (shift) { key = '3'; } else { key = '3'; } return true;
                    case Keys.D4: if (shift) { key = '4'; } else { key = '4'; } return true;
                    case Keys.D5: if (shift) { key = '5'; } else { key = '5'; } return true;
                    case Keys.D6: if (shift) { key = '6'; } else { key = '6'; } return true;
                    case Keys.D7: if (shift) { key = '7'; } else { key = '7'; } return true;
                    case Keys.D8: if (shift) { key = '8'; } else { key = '8'; } return true;
                    case Keys.D9: if (shift) { key = '9'; } else { key = '9'; } return true;

                    //Decimal numpad keys
                    case Keys.NumPad0: key = '0'; return true;
                    case Keys.NumPad1: key = '1'; return true;
                    case Keys.NumPad2: key = '2'; return true;
                    case Keys.NumPad3: key = '3'; return true;
                    case Keys.NumPad4: key = '4'; return true;
                    case Keys.NumPad5: key = '5'; return true;
                    case Keys.NumPad6: key = '6'; return true;
                    case Keys.NumPad7: key = '7'; return true;
                    case Keys.NumPad8: key = '8'; return true;
                    case Keys.NumPad9: key = '9'; return true;

                    //Special keys
                    case Keys.Decimal: if (shift) { key = '.'; } else { key = '.'; } return true;
                    case Keys.OemPeriod: if (shift) { key = '.'; } else { key = '.'; } return true;
                    case Keys.Back: key = 'b'; return true;
                }
            }

            key = (char)0;
            return false;
        }
        public void ResetGraphics(Vector2 position)
        {
            _position = position;
            _rectangle = new Rectangle((int)_position.X - 4, (int)_position.Y - 5, 250, 40);
        }

    }
}
