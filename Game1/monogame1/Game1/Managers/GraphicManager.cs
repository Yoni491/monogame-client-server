using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameClient
{

    public class GraphicManager
    {
        public static GraphicsDevice _graphicsDevice;
        static ContentManager _contentManager;
        public static SpriteFont _font;
        public GraphicManager(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            _graphicsDevice = graphicsDevice;
            _contentManager = contentManager;
            _font = contentManager.Load<SpriteFont>("Fonts/basic");
        }
        static public SpriteFont GetBasicFont()
        {
            SpriteFont font = _contentManager.Load<SpriteFont>("Fonts/basic");
            return font;
        }
        static public Texture2D Resize4x4Sprite(Texture2D texture, int x)
        {
            Rectangle newBounds = texture.Bounds;
            newBounds.Y = (texture.Height / 4) * x;
            newBounds.Height = (texture.Height / 4);
            Texture2D croppedTexture = new Texture2D(_graphicsDevice, newBounds.Width, newBounds.Height);
            Color[] data = new Color[newBounds.Width * newBounds.Height];
            texture.GetData(0, newBounds, data, 0, newBounds.Width * newBounds.Height);
            croppedTexture.SetData(data);
            return croppedTexture;
        }
        static public Texture2D GetTextureSqaure(Texture2D texture, int height, int width, int row, int column)
        {
            Rectangle newBounds = texture.Bounds;
            newBounds.Height = (texture.Height / height);
            newBounds.Width = (texture.Width / width);
            newBounds.Y = (texture.Height / height) * row;
            newBounds.X = (texture.Width / width) * column;
            Texture2D croppedTexture = new Texture2D(_graphicsDevice, newBounds.Width, newBounds.Height);
            Color[] data = new Color[newBounds.Width * newBounds.Height];
            texture.GetData(0, newBounds, data, 0, newBounds.Width * newBounds.Height);
            croppedTexture.SetData(data);
            return croppedTexture;
        }
        static public Animation MakeAnimationFromRow(Texture2D row,int width)
        {
            Texture2D[] _textures;
            _textures = new Texture2D[width];
            for (int i = 0; i < width; i++)
            {
                _textures[i] = (GetTextureSqaure(row, 1, width, 0, i));
            }
            return new Animation(_textures);
        }
        static private Dictionary<int, Animation> GetAnimation4x4Dictionary_spritesMovement(Texture2D i_texture)
        {
            return new Dictionary<int, Animation>()
            {
            { (int)Direction.Down, MakeAnimationFromRow(Resize4x4Sprite(i_texture,0),4) },
            { (int)Direction.Left, MakeAnimationFromRow(Resize4x4Sprite(i_texture,1),4) },
            { (int)Direction.Right, MakeAnimationFromRow(Resize4x4Sprite(i_texture,2),4) },
            { (int)Direction.Up, MakeAnimationFromRow(Resize4x4Sprite(i_texture,3),4) },
            };
        }
        static public AnimationManager GetAnimationManager_spriteMovement(int spriteNum,float scale)
        {
            return new AnimationManager(GetAnimation4x4Dictionary_spritesMovement(_contentManager.Load<Texture2D>("Patreon sprites 1/" + spriteNum)), 4 , scale);
        }
        static public void DrawLine(Vector2 start, Vector2 end, SpriteBatch spriteBatch)
        {
            Texture2D _line_texture = _contentManager.Load<Texture2D>("etc/lineSprite");
            spriteBatch.Draw(_line_texture, start, null, Color.White,
                             (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                             new Vector2(0f, (float)_line_texture.Height / 2),
                             new Vector2(Vector2.Distance(start, end), 0.005f),
                             SpriteEffects.None, 0.5f);

        }
        static public Texture2D getRectangleTexture(int width,int height, Color color)
        {
            Texture2D _healthbar = new Texture2D(_graphicsDevice, width, height);
            Color[] data = new Color[width* height];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            _healthbar.SetData(data);
            return _healthbar;
        }
        static public Texture2D getImage(string image)
        {
            return _contentManager.Load<Texture2D>("Images/"+ image);
        }
        static public void DrawRectangle(SpriteBatch spriteBatch,Rectangle rectangle,float layer)
        {
            Texture2D texture = new Texture2D(GraphicManager._graphicsDevice, rectangle.Width, rectangle.Height);
            Color[] data = new Color[rectangle.Width*rectangle.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Green;
            texture.SetData(data);
            spriteBatch.Draw(texture, rectangle, null ,Color.Black,0,Vector2.Zero,SpriteEffects.None,layer);

        }
    }
}
