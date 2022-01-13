using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace GameClient
{
    public class Input
    {
        GamePadState _prevGamePadState;
        KeyboardState _prevKeyboardState;
        public Keys _down;
        public Keys _left;
        public Keys _right;
        public Keys _up;
        public Keys _pick;
        public Keys _dash;
        public Vector2 _left_joystick_direction;
        public Vector2 _right_joystick_direction;
        public float _right_trigger;
        public float _left_trigger;
        private bool _buttonA, _buttonB, _buttonX, _buttonY, _buttonRightShoulder, _buttonLeftShoulder;
        public bool _buttonDPadRight, _buttonDPadLeft, _buttonDPadUp, _buttonDPadDown;
        private bool _buttonSpace;
        public bool _isGamePad;
        GamePadCapabilities _capabilities;
        int _capabilityIndex;
        public Input(int capabilityIndex)
        {
            _capabilityIndex = capabilityIndex;
            if (capabilityIndex == 4)
            {
                _isGamePad = false;
                _down = Keys.S;
                _left = Keys.A;
                _right = Keys.D;
                _up = Keys.W;
                _pick = Keys.Space;
                _dash = Keys.LeftShift;
            }
            else
            {
                _isGamePad = true;
                _capabilities = GamePad.GetCapabilities(capabilityIndex);
            }
        }
        public void Update()
        {
            if (_isGamePad && _capabilities.IsConnected)
            {
                GamePadState statePad = GamePad.GetState(_capabilityIndex);
                if (_capabilities.HasLeftXThumbStick)
                {
                    _left_joystick_direction = new Vector2(statePad.ThumbSticks.Left.X, -statePad.ThumbSticks.Left.Y);
                }
                if (_capabilities.HasRightXThumbStick)
                {
                    _right_joystick_direction = new Vector2(statePad.ThumbSticks.Right.X, -statePad.ThumbSticks.Right.Y);
                }
                if (_capabilities.HasRightTrigger)
                {
                    _right_trigger = statePad.Triggers.Right;
                }
                if (_capabilities.HasLeftTrigger)
                {
                    _left_trigger = statePad.Triggers.Left;
                }
                if (_capabilities.HasAButton)
                {
                    if (!_prevGamePadState.IsButtonDown(Buttons.A))
                        _buttonA = statePad.IsButtonDown(Buttons.A);
                }
                if (_capabilities.HasBButton)
                {
                    if (!_prevGamePadState.IsButtonDown(Buttons.B))
                        _buttonB = statePad.IsButtonDown(Buttons.B);
                }
                if (_capabilities.HasXButton)
                {
                    _buttonX = statePad.IsButtonDown(Buttons.X);
                }
                if (_capabilities.HasYButton)
                {
                    if (!_prevGamePadState.IsButtonDown(Buttons.Y))
                        _buttonY = statePad.IsButtonDown(Buttons.Y);
                }
                if (_capabilities.HasRightShoulderButton)
                {
                    if (!_prevGamePadState.IsButtonDown(Buttons.RightShoulder))
                        _buttonRightShoulder = statePad.IsButtonDown(Buttons.RightShoulder);
                }
                if (_capabilities.HasLeftShoulderButton)
                {
                    if (!_prevGamePadState.IsButtonDown(Buttons.LeftShoulder))
                        _buttonLeftShoulder = statePad.IsButtonDown(Buttons.LeftShoulder);
                }
                if (_capabilities.HasDPadRightButton)
                {
                    if (!_prevGamePadState.IsButtonDown(Buttons.DPadRight))
                        _buttonDPadRight = statePad.IsButtonDown(Buttons.DPadRight);
                }
                if (_capabilities.HasDPadLeftButton)
                {
                    if (!_prevGamePadState.IsButtonDown(Buttons.DPadLeft))
                        _buttonDPadLeft = statePad.IsButtonDown(Buttons.DPadLeft);
                }
                if (_capabilities.HasDPadRightButton)
                {
                    if (!_prevGamePadState.IsButtonDown(Buttons.DPadUp))
                        _buttonDPadUp = statePad.IsButtonDown(Buttons.DPadUp);
                }
                if (_capabilities.HasDPadRightButton)
                {
                    if (!_prevGamePadState.IsButtonDown(Buttons.DPadDown))
                        _buttonDPadDown = statePad.IsButtonDown(Buttons.DPadDown);
                }
                _prevGamePadState = statePad;
            }
            else
            {
                KeyboardState stateKeyboard = Keyboard.GetState();
                if (_prevKeyboardState.IsKeyUp(Keys.Space) && stateKeyboard.IsKeyDown(Keys.Space))
                    _buttonSpace = true;
                if (!Game_Client._isMultiplayer)
                {
                    _buttonSpace = stateKeyboard.IsKeyDown(Keys.Space);
                }
                _prevKeyboardState = stateKeyboard;
            }
        }
        public void GetVelocity(ref Vector2 _velocity, float _speed)
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
        public void GetLookingDirection(ref Vector2 _looking_direction, Gun _gun, MeleeWeapon _meleeWeapon)
        {
            if (_isGamePad)
            {
                if (_right_joystick_direction != Vector2.Zero)
                {
                    _isGamePad = true;
                    _looking_direction = _right_joystick_direction;
                }
            }
            else
            {
                if (_gun != null)
                    _looking_direction = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - _gun.Position * GraphicManager.ScreenScale;
                else if (_meleeWeapon != null)
                    _looking_direction = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - _meleeWeapon.Position * GraphicManager.ScreenScale;
            }
        }
        public bool MeleeAttack()
        {
            if (_isGamePad)
            {
                if (_buttonX)
                {
                    _buttonX = false;
                    return true;
                }
            }
            else
            {
                if (Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    return true;
                }
            }
            return false;
        }
        public bool Shot()
        {
            if (_isGamePad)
            {
                if (_right_trigger > 0)
                {
                    return true;
                }
            }
            else
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    return true;
                }
            }
            return false;
        }
        public bool Pick()
        {
            if (_isGamePad)
            {
                if (_buttonB)
                {
                    return true;
                }
            }
            else
            {
                if (_buttonSpace)
                {
                    _buttonSpace = false;
                    return true;
                }
            }
            return false;
        }
        public bool Dash()
        {
            if (_isGamePad)
            {
                if (_buttonLeftShoulder)
                {
                    _buttonLeftShoulder = false;
                    return true;
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(_dash))
                {
                    return true;
                }
            }
            return false;
        }
        public bool MoveInventoryPointerRight()
        {
            if (_isGamePad)
            {
                if (_buttonDPadRight)
                {
                    _buttonRightShoulder = false;
                    return true;
                }
            }
            return false;
        }
        public bool MoveInventoryPointerLeft()
        {
            if (_isGamePad)
            {
                if (_buttonDPadLeft)
                {
                    _buttonDPadLeft = false;
                    return true;
                }
            }
            return false;
        }
        public bool ClickInventoryItemGamePad()
        {
            if (_isGamePad)
            {
                if (_buttonA)
                {
                    _buttonA = false;
                    return true;
                }
            }
            return false;
        }
        public bool DropInventoryItemGamePad()
        {
            if (_isGamePad)
            {
                if (_buttonY)
                {
                    _buttonY = false;
                    return true;
                }
            }
            return false;
        }

    }
}