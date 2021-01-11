using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
namespace GameClient
{
    public class AnimationManager
    {
        private Dictionary<int, Animation> _animations;
        private Animation _animation;
        private float _timer;
        private int _frameCount;
        public Vector2 _position;
        public float _scale;
        public Animation Animation { get => _animation; set => _animation = value; }

        public AnimationManager(Dictionary<int, Animation> animations, int frameCount)
        {
            _animations = animations;
            _animation = _animations.First().Value;
            _frameCount = frameCount;
        }
        public AnimationManager(Dictionary<int, Animation> animations,int frameCount, float scale)
        {
            _animations = animations;
            _animation = _animations.First().Value;
            _frameCount = frameCount;
            _scale = scale;
        }
        public void Update(GameTime gameTime,Vector2 position)
        {
            _position = position;
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > _animation._frameSpeed)
            {
                _timer = 0f;
                _animation._currentFrame++;
                if (_animation._currentFrame >= _frameCount)
                {
                    _animation._currentFrame = 0;
                    if(_animation._textures!=null)
                    {
                        Stop();
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, float layer)
        {
            spriteBatch.Draw(_animation._textures[_animation._currentFrame], _position,
                null,
                Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None,
                layer);
        }
        public AnimationManager Copy(float scale)
        {
            return new AnimationManager(_animations, _frameCount , scale);
        }
        public void Play(int animation_number)
        {
            if (_animation == _animations[animation_number])
                return;
            _animation = _animations[animation_number];
            _animation._currentFrame = 0;
            _animation._frameSpeed = 0.25f;
        }
        public void Stop()
        {
            _timer = 0;
            _animation._currentFrame = 0;
        }
        public void Hide()
        {

        }
        public Vector2 getAnimationPickPosition()
        {
            return new Vector2(_animation._frameWidth / 2, _animation._frameHeight);
            //return new Vector2(_animation.Texture.Width / 2, _animation.Texture.Height);
        }

    }
}
