using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
namespace GameClient
{
    public class InputManager
    {
        //List<Input> _inputs;
        Dictionary<int, Player> _playerCapabilities;
        Player _mousePlayer;//index = 4

        public InputManager()
        {
            //_inputs = new List<Input>();
            _playerCapabilities = new Dictionary<int, Player>()
            {
                { 0, null},
                { 1, null},
                { 2, null},
                { 3, null}
            };
        }
        //public Input GetMouseInput(GamePadCapabilities capabilities)
        //{
        //}
        public void AssignCapabiltyToPlayer(int capabilityIndex, Player player)
        {
            if (capabilityIndex == 4)
            {
                _mousePlayer = player;
                player._input = new Input(capabilityIndex);
            }
            else if (0 <= capabilityIndex && capabilityIndex <= 3)
            {
                _playerCapabilities[capabilityIndex] = player;
                player._input = new Input(capabilityIndex);
            }
        }
        public int GetCapabilities()
        {
            foreach (KeyValuePair<int, Player> playerCapability in _playerCapabilities)
            {
                if (GamePad.GetCapabilities(playerCapability.Key).IsConnected && playerCapability.Value == null)
                {
                    GamePadState statePad = GamePad.GetState(playerCapability.Key);
                    if (statePad.IsButtonDown(Buttons.A) || statePad.IsButtonDown(Buttons.Start))
                    {
                        return playerCapability.Key;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _mousePlayer == null)
                return 4;
            return -1;
        }
    }
}
