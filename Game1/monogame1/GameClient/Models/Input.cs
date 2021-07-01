using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameClient
{
    public class Input
    {
        GamePadState _prevState;
        public Keys _down;
        public Keys _left;
        public Keys _right;
        public Keys _up;
        public Keys _pick;
        public Vector2 _left_joystick_direction;
        public Vector2 _right_joystick_direction;
        public float _right_trigger;
        public float _left_trigger;
        private bool _buttonA, _buttonB,_buttonX,_buttonY, _buttonRightShoulder, _buttonLeftShoulder;

        public bool _isGamePad;

        GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);

        public Input(Keys up, Keys down,Keys left, Keys right, Keys pick)
        {
            _down = down;
            _left = left;
            _right = right;
            _up = up;
            _pick = pick;
        }
        public void Update()
        {

            if (capabilities.IsConnected)
            {
                GamePadState state = GamePad.GetState(PlayerIndex.One);
                if (capabilities.HasLeftXThumbStick)
                {
                    _left_joystick_direction = new Vector2(state.ThumbSticks.Left.X, -state.ThumbSticks.Left.Y);
                }
                if (capabilities.HasRightXThumbStick)
                {
                    _right_joystick_direction = new Vector2(state.ThumbSticks.Right.X, -state.ThumbSticks.Right.Y);
                }
                if (capabilities.HasRightTrigger)
                {
                    _right_trigger = state.Triggers.Right;
                }
                if (capabilities.HasLeftTrigger)
                {
                    _left_trigger = state.Triggers.Left;
                }
                if (capabilities.HasAButton)
                {
                    _buttonA = state.IsButtonDown(Buttons.A);
                }
                if (capabilities.HasBButton)
                {
                    if (!_prevState.IsButtonDown(Buttons.B))
                        _buttonB = state.IsButtonDown(Buttons.B);
                }
                if (capabilities.HasXButton)
                {
                    _buttonX = state.IsButtonDown(Buttons.X);
                }
                if (capabilities.HasYButton)
                {
                    if (!_prevState.IsButtonDown(Buttons.Y))
                        _buttonY = state.IsButtonDown(Buttons.Y);
                }
                if (capabilities.HasRightShoulderButton)
                {
                    if (!_prevState.IsButtonDown(Buttons.RightShoulder))
                        _buttonRightShoulder = state.IsButtonDown(Buttons.RightShoulder);
                }
                if (capabilities.HasLeftShoulderButton)
                {
                    if (!_prevState.IsButtonDown(Buttons.LeftShoulder))
                        _buttonLeftShoulder = state.IsButtonDown(Buttons.LeftShoulder);
                }
                _prevState = state;
            }
        }
        public void GetVelocity(ref Vector2 _velocity,float _speed)
        {
            if (Keyboard.GetState().IsKeyDown(_up))
            {
                _isGamePad = false;
                _velocity.Y = -1;

            }
            else if (Keyboard.GetState().IsKeyDown(_down))
            {
                _isGamePad = false;
                _velocity.Y = 1;
            }
            if (Keyboard.GetState().IsKeyDown(_left))
            {
                _isGamePad = false;
                _velocity.X = -1;
            }
            else if (Keyboard.GetState().IsKeyDown(_right))
            {
                _isGamePad = false;
                _velocity.X = 1;
            }
            if (_left_joystick_direction != Vector2.Zero)
            {
                _isGamePad = true;
                _velocity = _left_joystick_direction;
            }
            if (_velocity != Vector2.Zero)
            {
                _velocity = Vector2.Normalize(_velocity) * _speed;
            }
        }
        public void GetLookingDirection(ref Vector2 _looking_direction, Gun _gun,MeleeWeapon _meleeWeapon)
        {
            if (_right_joystick_direction != Vector2.Zero)
            {
                _isGamePad = true;
                _looking_direction = _right_joystick_direction;
            }
            if (!_isGamePad)
            {
                if (_gun != null)
                    _looking_direction = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - _gun.Position * GraphicManager.ScreenScale;
                else if (_meleeWeapon != null)
                    _looking_direction = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - _meleeWeapon.Position * GraphicManager.ScreenScale;
            }
        }
        public bool MeleeAttack()
        {
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                _isGamePad = false;
                return true;
            }
            else if (_buttonA)
            {
                _isGamePad = true;
                return true;
            }
            return false;
        }
        public bool Shot()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _isGamePad = false;
                return true;
            }
            else if(_right_trigger > 0)
            {
                _isGamePad = true;
                return true;
            }
            return false;
        }
        public bool Pick()
        {
            if (Keyboard.GetState().IsKeyDown(_pick))
            {
                return true;
            }
            else if (_buttonX)
            {
                _isGamePad = true;
                return true;
            }
            return false;
        }
        public bool MoveInventoryPointerRight()
        {
            if (_buttonRightShoulder)
            {
                _isGamePad = true;
                _buttonRightShoulder = false;
                return true;
            }
            return false;
        }
        public bool MoveInventoryPointerLeft()
        {
            if (_buttonLeftShoulder)
            {
                _isGamePad = true;
                _buttonLeftShoulder = false;
                return true;
            }
            return false;
        }
        public bool ClickInventoryItemGamePad()
        {
            if (_buttonB)
            {
                _isGamePad = true;
                _buttonB = false;
                return true;
            }
            return false;
        }
        public bool DropInventoryItemGamePad()
        {
            if (_buttonY)
            {
                _isGamePad = true;
                _buttonY = false;
                return true;
            }
            return false;
        }
    }
}