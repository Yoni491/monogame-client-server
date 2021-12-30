using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
namespace GameClient
{
    public class InputManager
    {
        List<Input> _inputs;
        List<GamePadCapabilities> _capabilities;
        Dictionary<GamePadCapabilities, Player> _playerCapabilities;
        Player _mousePlayer;
        public InputManager()
        {
            _inputs = new List<Input>();
            _playerCapabilities.Add(GamePad.GetCapabilities(PlayerIndex.One), null);
            _playerCapabilities.Add(GamePad.GetCapabilities(PlayerIndex.Two), null);
            _playerCapabilities.Add(GamePad.GetCapabilities(PlayerIndex.Three), null);
            _playerCapabilities.Add(GamePad.GetCapabilities(PlayerIndex.Four), null);
        }
        //public Input GetMouseInput(GamePadCapabilities capabilities)
        //{
        //}
        public void AssignCapabiltyToPlayer(int capabilityIndex, Player player)
        {
            if (capabilityIndex == 0)
            {
                _mousePlayer = player;
                player._input = new Input(capabilityIndex);
            }
            else if (1 <= capabilityIndex && capabilityIndex <= 4)
            {
                _playerCapabilities[GamePad.GetCapabilities(capabilityIndex)] = player;
                player._input = new Input(capabilityIndex);
            }
        }
        public int GetCapabilities()
        {
            int index = 1;
            foreach (KeyValuePair<GamePadCapabilities, Player> playerCapability in _playerCapabilities)
            {
                if (playerCapability.Key.IsConnected && playerCapability.Value == null)
                {
                    GamePadState statePad = GamePad.GetState(index++);
                    if (statePad.IsButtonDown(Buttons.A) || statePad.IsButtonDown(Buttons.Start))
                    {
                        return index;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _mousePlayer == null)
                return 0;
            return -1;
        }
    }
}
