using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Player
    {
        private Gun _gun;

        private Boolean _hide_gun = false;

        private Input _input;

        private float _speed = 2f;

        private Vector2 _velocity;

        private AnimationManager _animationManager;

        private Dictionary<string, Animation> _animations;

        private Vector2 _position;

        private HealthManager _health;

        private Vector2 _looking_direction;

        public Vector2 Position { get => _position; set => _position = value; }

        public Player(Dictionary<string, Animation> i_animations, Vector2 position,Input input , int health)
        {
            _animations = i_animations;
            _animationManager = new AnimationManager(_animations.First().Value);
            _position = position;
            _input = input;
            _health = new HealthManager(health,position +new Vector2(8,10));
            _velocity = Vector2.Zero;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_hide_gun && _gun != null)
                _gun.Draw(spriteBatch, _position);
            _animationManager.Draw(spriteBatch);
            if (_gun != null && !_hide_gun)
                _gun.Draw(spriteBatch, _position);
            _health.Draw(spriteBatch);
        }

        public void InputReader()
        {
            if (Keyboard.GetState().IsKeyDown(_input.Up))
                _velocity.Y = -1;
            else if (Keyboard.GetState().IsKeyDown(_input.Down))
                _velocity.Y = 1;
            if (Keyboard.GetState().IsKeyDown(_input.Left))
                _velocity.X = -1;
            else if (Keyboard.GetState().IsKeyDown(_input.Right))
                _velocity.X = 1;
            if(_input._left_joystick_direction!= Vector2.Zero)
                _velocity = _input._left_joystick_direction;

            if(_velocity!= Vector2.Zero)
                _velocity = Vector2.Normalize(_velocity) * _speed;

            if (_input._right_joystick_direction != Vector2.Zero)
                _looking_direction = _input._right_joystick_direction;
            else
                _looking_direction = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - _gun.Position;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                _gun.Shot();
            if(_input._right_trigger>0)
                _gun.Shot();
        }
        protected void SetAnimations()
        {
            
            if (_velocity == Vector2.Zero)
                _animationManager.Stop();
            else
            {
                _hide_gun = false;
                if (_velocity.X >= Math.Abs(_velocity.Y))
                {
                    _animationManager.Play(_animations["WalkRight"]);

                }
                else if (-_velocity.X >= Math.Abs(_velocity.Y))
                {
                    _animationManager.Play(_animations["WalkLeft"]);
                }
                else if (_velocity.Y > 0)
                {
                    _animationManager.Play(_animations["WalkDown"]);
                }
                else if (_velocity.Y < 0)
                {
                    _animationManager.Play(_animations["WalkUp"]);
                    _hide_gun = true;
                }
                else _animationManager.Stop();
            }
        }


        public void EquipGun(Gun gun)
        {
            _gun = gun;
        }
        public void Update(GameTime gameTime,List<Simple_Enemy> enemies)
        {
            InputReader();

            SetAnimations();

            _position += _velocity;
            
            _animationManager.Position = _position;

            _animationManager.Update(gameTime);

            

            if (_gun != null)
            {
                _gun.Update(gameTime, enemies,_looking_direction);
            }
            _velocity = Vector2.Zero;

            _health._position = _position + new Vector2(8, 10);

            _input.Update();
        }

    }
}

