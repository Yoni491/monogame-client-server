using Microsoft.Xna.Framework.Graphics;

namespace GameClient
{
    public class Animation
    {
        public int CurrentFrame { get; set; }
        public int FrameHeight { get { return Texture.Height; } }
        public float FrameSpeed { get; set; }
        public int FrameWidth { get { return Texture.Width / _frameCount; } }
        public bool isLooping { get; set; }

        public int _frameCount;

        public Texture2D[] _textures;


        public Texture2D Texture { get; private set; }
        public Animation(Texture2D texture, int frameCount)
        {
            Texture = texture;
            isLooping = true;
            FrameSpeed = 0.2f;
            _frameCount = frameCount;
        }
        public Animation(Texture2D []textures, int frameCount)
        {
            _textures = textures;
            isLooping = true;
            FrameSpeed = 0.2f;
            _frameCount = frameCount;
        }
    }
}
