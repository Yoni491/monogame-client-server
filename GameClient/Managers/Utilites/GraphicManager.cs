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
        public static ContentManager _contentManager;
        private float _timer_update_grpahics;
        public static GraphicsDeviceManager _graphics;
        public static Vector2 ScreenScale;
        public static float screenHeightScaled;
        public static float screenWidthScaled;
        public static int screenHeight;
        public static int screenWidth;
        public static Vector2 _ScreenMiddle;
        public static Texture2D _deadPlayerTexture;
        public static int _maxScreenHeight;
        public static int _maxScreenWidth;
        public GraphicManager(GraphicsDevice graphicsDevice, ContentManager contentManager, GraphicsDeviceManager graphics)
        {
            _graphicsDevice = graphicsDevice;
            _contentManager = contentManager;
            _graphics = graphics;
            _maxScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _maxScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            ScreenScale = new Vector2((float)1280 / 1920, (float)720 / 1080);
            screenHeight = 720;
            screenWidth = 1280;
            screenHeightScaled = ScreenScale.Y * graphicsDevice.Viewport.Height;
            screenWidthScaled = ScreenScale.X * graphicsDevice.Viewport.Width;
            _ScreenMiddle = new Vector2(screenWidthScaled, screenHeightScaled);
            _deadPlayerTexture = GetTextureSqaure(_contentManager.Load<Texture2D>("resources/Dungeon_Tileset"), 10, 10, 9, 3);
            Button._font = GetBasicFont("basic_16");
            Inventory._font = GetBasicFont("basic_12");
        }
        public void Update(GameTime gameTime)
        {
            _timer_update_grpahics += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer_update_grpahics >= 1)
            {
                _timer_update_grpahics = 0;
                Game_Client.ResetGraphics();
            }
        }
        static public SpriteFont GetBasicFont(string fontName)
        {
            SpriteFont font = _contentManager.Load<SpriteFont>("Fonts/" + fontName);
            return font;
        }
        static public void ChangeToFullScreen(bool fullScreen)
        {
            if (fullScreen)
            {
                ChangeScreenSize(_maxScreenWidth, _maxScreenHeight);
                _graphics.PreferredBackBufferWidth = _maxScreenWidth;  // set this value to the desired width of your window
                _graphics.PreferredBackBufferHeight = _maxScreenHeight;   // set this value to the desired height of your window
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
        static public void ChangeScreenSize(int width, int height)
        {
            screenHeight = height;
            screenWidth = width;
            ScreenScale = new Vector2((float)width / 1920, (float)height / 1080);
            screenHeightScaled = ScreenScale.Y * height;
            screenWidthScaled = ScreenScale.X * width;
            _ScreenMiddle = new Vector2(screenWidthScaled, screenHeightScaled);
        }
        static public Texture2D GetRowFrom4x4Sprite(Texture2D texture, int x)
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
        static public Animation MakeAnimationFromRow(Texture2D row, int width)
        {
            Texture2D[] _textures;
            _textures = new Texture2D[width];
            for (int i = 0; i < width; i++)
            {
                _textures[i] = (GetTextureSqaure(row, 1, width, 0, i));
            }
            return new Animation(_textures);
        }
        static public Animation MakeAnimationFromImage(Texture2D img, int width, int height, int startingIndex=0,int lastIndex = 0)
        {
            Texture2D[] _textures;
            _textures = new Texture2D[width];
            int index = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (startingIndex <= index++)
                    {
                        if(lastIndex!=0 || index <= lastIndex)
                        {
                            _textures[i] = (GetTextureSqaure(img, height, width, j, i));
                        }
                    }

                }

            }
            return new Animation(_textures);
        }
        static public AnimationManager GetAnimationManager_Gun(int spriteNum, int width, int height, int startingIndex = 0, int lastIndex = -1, float scale=1f)
        {
            Texture2D texture = _contentManager.Load<Texture2D>("Weapons/Animations/"+spriteNum);
            return new AnimationManager(new Dictionary<int, Animation>()
            {
                { 0,MakeAnimationFromImage(texture,width,height,startingIndex,lastIndex) },//shot animation
            },10,scale, spriteNum);
        }
        static private Dictionary<int, Animation> GetAnimation4x4Dictionary_spritesMovement(Texture2D i_texture)
        {
            return new Dictionary<int, Animation>()
            {
            { (int)Direction.Down, MakeAnimationFromRow(GetRowFrom4x4Sprite(i_texture,0),4) },
            { (int)Direction.Left, MakeAnimationFromRow(GetRowFrom4x4Sprite(i_texture,1),4) },
            { (int)Direction.Right, MakeAnimationFromRow(GetRowFrom4x4Sprite(i_texture,2),4) },
            { (int)Direction.Up, MakeAnimationFromRow(GetRowFrom4x4Sprite(i_texture,3),4) },
            };
        }
        static public AnimationManager GetAnimationManager_spriteMovement(int spriteNum, float scale)
        {
            return new AnimationManager(GetAnimation4x4Dictionary_spritesMovement(_contentManager.Load<Texture2D>("Patreon sprites 1/" + spriteNum)), 4, scale, spriteNum);
        }
        static public void DrawLine(Vector2 start, Vector2 end, SpriteBatch spriteBatch)
        {
            float angle = (float)Math.Atan2(start.Y - end.Y, start.X - end.X);
            float distance = Vector2.Distance(start, end);
            Texture2D _line_texture = _contentManager.Load<Texture2D>("etc/lineSprite");
            spriteBatch.Draw(_line_texture, new Rectangle((int)end.X, (int)end.Y, (int)distance, 2), null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0.5f);
        }
        static public Texture2D getRectangleTexture(int width, int height, Color color)
        {
            Texture2D _healthbar = new Texture2D(_graphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            _healthbar.SetData(data);
            return _healthbar;
        }
        static public Texture2D getImage(string image)
        {
            return _contentManager.Load<Texture2D>("Images/" + image);
        }
        static public void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, float layer)
        {
            Texture2D texture = new Texture2D(GraphicManager._graphicsDevice, rectangle.Width, rectangle.Height);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Green;
            texture.SetData(data);
            spriteBatch.Draw(texture, rectangle, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, layer);
        }
        static public void DrawSmallSquareAtPosition(SpriteBatch spriteBatch, Vector2 position, float layer)
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
