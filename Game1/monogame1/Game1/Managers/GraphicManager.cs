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
        private float _timer_update_grpahics;
        private Game_Client _gameClient;
        public static GraphicsDeviceManager _graphics;
        public static Vector2 ScreenScale;
        public static float screenHeightScaled;
        public static float screenWidthScaled;
        public static int screenHeight;
        public static int screenWidth;
        public static Vector2 _ScreenMiddle;
        public GraphicManager(GraphicsDevice graphicsDevice, ContentManager contentManager, Game_Client gameClient, GraphicsDeviceManager graphics)
        {
            _graphicsDevice = graphicsDevice;
            _contentManager = contentManager;
            _font = contentManager.Load<SpriteFont>("Fonts/basic");
            _gameClient = gameClient;
            _graphics = graphics;
            ScreenScale = new Vector2((float)1280 / 1920, (float)720 / 1080);
            screenHeight = 720;
            screenWidth = 1280;
            screenHeightScaled = ScreenScale.Y * graphicsDevice.Viewport.Height;
            screenWidthScaled = ScreenScale.X * graphicsDevice.Viewport.Width;
            _ScreenMiddle = new Vector2(screenWidthScaled, screenHeightScaled);
        }
        public void Update(GameTime gameTime)
        {
            _timer_update_grpahics += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(_timer_update_grpahics >=1)
            {
                _timer_update_grpahics = 0;
                _gameClient.ResetGraphics();
            }
        }
        static public SpriteFont GetBasicFont()
        {
            SpriteFont font = _contentManager.Load<SpriteFont>("Fonts/basic");
            return font;
        }
        static public void ChangeToFullScreen(bool fullScreen)
        {
            if (fullScreen)
            {
                ChangeScreenSize(1920, 1080);
                _graphics.PreferredBackBufferWidth = 1920;  // set this value to the desired width of your window
                _graphics.PreferredBackBufferHeight = 1080;   // set this value to the desired height of your window
                _graphics.ApplyChanges();
                _graphics.IsFullScreen = true;
                _graphics.ApplyChanges();
            }
            else
            {
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();
                ChangeScreenSize(1280, 720);
                _graphics.PreferredBackBufferWidth = 1280;  // set this value to the desired width of your window
                _graphics.PreferredBackBufferHeight = 720;   // set this value to the desired height of your window
                _graphics.ApplyChanges();
                
            }
        }
        static public void ChangeScreenSize(int width,int height)
        {
            screenHeight = height;
            screenWidth = width;
            ScreenScale = new Vector2((float)width / 1920, (float)height / 1080);
            screenHeightScaled = ScreenScale.Y * height;
            screenWidthScaled = ScreenScale.X * width;
            _ScreenMiddle = new Vector2(screenWidthScaled, screenHeightScaled);
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
        static public void DrawSmallSquareAtPosition(SpriteBatch spriteBatch,Vector2 position, float layer)
        {
            DrawRectangle(spriteBatch, new Rectangle((int)position.X, (int)position.Y, 5, 5), layer);
        }
        static public Matrix GetSpriteBatchMatrix()
        {
            var scaleX = (float)screenWidth / 1920;
            var scaleY = (float)screenHeight / 1080;
            return Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }
    }
}
