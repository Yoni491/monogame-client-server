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
        public Vector2 _position, _origin;
        public float _scale;
        private float _rotation, _holderScale;
        private SpriteEffects _flipEffect;
        private bool _letAnimationFinish;
        public int _animationID;

        public Animation Animation { get => _animation; set => _animation = value; }

        public AnimationManager(Dictionary<int, Animation> animations, int frameCount, float scale, int animationID)
        {
            _animations = animations;
            _animation = _animations.First().Value;
            _frameCount = frameCount;
            _scale = scale;
            _animationID = animationID;
        }
        public void Update(GameTime gameTime, Vector2 position)
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
                    _letAnimationFinish = false;
                }
            }
        }
        public void DrawCharacter(SpriteBatch spriteBatch, float layer)
        {
            spriteBatch.Draw(_animation._textures[_animation._currentFrame], _position,
                null,
                Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None,
                layer);
        }
        public void DrawGun(SpriteBatch spriteBatch, float layer)
        {
            spriteBatch.Draw(_animation._textures[_animation._currentFrame], _position,
                null,
                Color.White, _rotation, _origin, _holderScale, _flipEffect,
                layer);
        }
        public AnimationManager Copy()
        {
            Dictionary<int, Animation> _animationsCopy;
            _animationsCopy = new Dictionary<int, Animation>();
            foreach (var item in _animations)
            {
                _animationsCopy.Add(item.Key, item.Value.Copy());
            }
            return new AnimationManager(_animationsCopy, _frameCount, _scale, _animationID);
        }
        public void Play(int animation_number)
        {
            if (_animation == _animations[animation_number])
                return;
            _animation = _animations[animation_number];
            _animation._currentFrame = 0;
        }
        public void Stop()
        {
            _timer = 0;
            _animation._currentFrame = 0;
        }
        public Vector2 getAnimationPickPositionForCharacter()
        {
            return new Vector2(_animation._frameWidth / 2, _animation._frameHeight);
        }
        public void SetAnimationsCharacter(Vector2 velocity, ref bool hide_weapon, ref int moving_direction)
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
        public void SetAnimationsGun(bool currentlyShooting, float rotation, Vector2 origin, float holderScale, SpriteEffects flipEffect)
        {
            _rotation = rotation;
            _holderScale = holderScale;
            _flipEffect = flipEffect;
            _origin = origin;
            if (currentlyShooting)
            {
                Play(0);
                _letAnimationFinish = true;
            }
            else if (_letAnimationFinish)
            {
                Play(0);
            }
            else
            {
                Stop();
            }
        }
        public void SetAnimationsFromServerCharacter(Vector2 velocity, ref bool hide_weapon, ref int moving_direction)
        {
            if (velocity == Vector2.Zero)
            {
                Play(moving_direction);
                if (moving_direction == (int)Direction.Up)
                    hide_weapon = true;
                else
                    hide_weapon = false;
                Stop();
            }
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
    }
}
