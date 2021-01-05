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
        public Vector2 Position { get; set; }
        public Animation Animation { get => _animation; set => _animation = value; }

        
        public AnimationManager(Dictionary<int, Animation> animations,int frameCount)
        {
            _animations = animations;
            _animation = _animations.First().Value;
            _frameCount = frameCount;
        }
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;
                _animation.CurrentFrame++;
                if (_animation.CurrentFrame >= _frameCount)
                {
                    _animation.CurrentFrame = 0;
                    if(_animation._textures!=null)
                    {
                        Stop();
                    }
                }
            }
        }
        public void DrawEachAnimationLine(SpriteBatch spriteBatch,float layer)
        {
            spriteBatch.Draw(_animation.Texture,Position,
                new Rectangle(_animation.CurrentFrame * _animation._frameWidth, 0,_animation._frameWidth,_animation.FrameHeight),
                Color.White,0,Vector2.Zero,1,SpriteEffects.None,
                layer);
        }
        public void DrawAnimationByOrder(SpriteBatch spriteBatch, float layer)
        {
            spriteBatch.Draw(_animation._textures[_animation.CurrentFrame], Position,
                null,
                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None,
                layer);
        }
        public AnimationManager Copy()
        {
            return new AnimationManager(_animations, _frameCount);
        }
        public void Play(int animation_number)
        {
            if (_animation == _animations[animation_number])
                return;
            _animation = _animations[animation_number];
            _animation.CurrentFrame = 0;
            _animation.FrameSpeed = 0.25f;
        }
        public void Stop()
        {
            _timer = 0;
            _animation.CurrentFrame = 0;
        }
        public void Hide()
        {

        }
        public Vector2 getAnimationPickPosition()
        {
            return new Vector2(_animation._frameWidth / 2, _animation.FrameHeight);
            //return new Vector2(_animation.Texture.Width / 2, _animation.Texture.Height);
        }

    }
}
