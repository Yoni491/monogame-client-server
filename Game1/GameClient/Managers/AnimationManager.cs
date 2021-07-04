using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GameClient
{
    public class AnimationManager
    {
        public Dictionary<int, Animation> _animations;
        private Animation _animation;
        private float _timer;
        private int _frameCount;
        public Vector2 _position;
        public float _scale;
        public int _animationID;
        public Animation Animation { get => _animation; set => _animation = value; }

        public AnimationManager(Dictionary<int, Animation> animations,int frameCount, float scale, int animationID)
        {
            _animations = animations;
            _animation = _animations.First().Value;
            _frameCount = frameCount;
            _scale = scale;
            _animationID = animationID;
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
        public AnimationManager Copy()
        {
            AnimationManager animationManager = GraphicManager.GetAnimationManager_spriteMovement(_animationID, _scale);
            return animationManager;
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
        public Vector2 getAnimationPickPosition()
        {
            return new Vector2(_animation._frameWidth / 2, _animation._frameHeight);
        }
        public void SetAnimations(Vector2 _velocity, ref bool _hide_weapon,ref int _moving_direction)
        {
            if (_velocity == Vector2.Zero)
                Stop();
            else
            {
                _hide_weapon = false;
                _moving_direction = -1;
                if (_velocity.X >= Math.Abs(_velocity.Y))
                {
                    _moving_direction = (int)Direction.Right;

                }
                else if (-_velocity.X >= Math.Abs(_velocity.Y))
                {
                    _moving_direction = (int)Direction.Left;
                }
                else if (_velocity.Y > 0)
                {
                    _moving_direction = (int)Direction.Down;
                }
                else if (_velocity.Y < 0)
                {
                    _moving_direction = (int)Direction.Up;
                    _hide_weapon = true;
                }
                else Stop();
                if (_moving_direction != -1)
                    Play(_moving_direction);
            }
        }
    }
}
