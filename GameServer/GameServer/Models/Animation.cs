using Microsoft.Xna.Framework.Graphics;

namespace GameServer
{
    public class Animation
    {
        public int _currentFrame;
        public int _frameHeight;
        public float _frameSpeed;
        public int _frameWidth;
        public bool _isLooping;
        public int _frameCount;

        public Texture2D[] _textures;

        public Animation(Texture2D []textures)
        {
            _textures = textures;
            _isLooping = true;
            _frameSpeed = 0.2f;
            _frameWidth = textures[0].Width;
            _frameHeight = textures[0].Height;
        }
        public Animation Copy()
        {
            return new Animation(_textures);
        }
    }
}
