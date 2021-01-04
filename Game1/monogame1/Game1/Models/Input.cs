using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameClient
{
    public class Input
    {
        public Keys Down { get; set; }

        public Keys Left { get; set; }

        public Keys Right { get; set; }

        public Keys Up { get; set; }

        public Vector2 _left_joystick_direction;

        public Vector2 _right_joystick_direction;

        public float _right_trigger;


        GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);

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

                if (capabilities.HasAButton)
                {
                    state.IsButtonDown(Buttons.A);

                }
            }
        }
    }
}