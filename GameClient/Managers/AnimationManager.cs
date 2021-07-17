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
        public void SetAnimations(Vector2 velocity, ref bool hide_weapon,ref int moving_direction)
        {
            if (velocity == Vector2.Zero)
                Stop();
            else
            {
                hide_weapon = false;
                moving_direction = -1;
                if (velocity.X >= Math.Abs(velocity.Y))
                {
                    moving_direction = (int)Direction.Right;

                }
                else if (-velocity.X >= Math.Abs(velocity.Y))
                {
                    moving_direction = (int)Direction.Left;
                }
                else if (velocity.Y > 0)
                {
                    moving_direction = (int)Direction.Down;
                }
                else if (velocity.Y < 0)
                {
                    moving_direction = (int)Direction.Up;
                    hide_weapon = true;
                }
                else Stop();
                if (moving_direction != -1)
                    Play(moving_direction);
            }
        }
        public void SetAnimationsFromServer(int moving_direction)
        {
            if (moving_direction != -1)
                Play(moving_direction);
            else
                Stop();
        }
    }
}
