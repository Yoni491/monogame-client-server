using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameClient
{

    public class GraphicManager
    {
        static GraphicsDevice _graphicsDevice;
        static ContentManager _contentManager;
        public static SpriteFont _font;
        public GraphicManager(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            _graphicsDevice = graphicsDevice;
            _contentManager = contentManager;
            _font = contentManager.Load<SpriteFont>("Fonts/basic");
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
        static public Dictionary<string, Animation> GetAnimation4x4Dictionary(Texture2D i_texture)
        {
            return new Dictionary<string, Animation>()
            {
            { "WalkDown", new Animation(Resize4x4Sprite(i_texture,0), 4) },
            { "WalkLeft", new Animation(Resize4x4Sprite(i_texture,1), 4) },
            { "WalkRight", new Animation(Resize4x4Sprite(i_texture,2), 4) },
            { "WalkUp", new Animation(Resize4x4Sprite(i_texture,3), 4) },
            };
        }
    }
}
