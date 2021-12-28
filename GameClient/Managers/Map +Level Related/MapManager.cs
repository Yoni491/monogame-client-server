using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class MapManager
    {
        static public List<Grave> _graves;
        static public List<MessageBoard> _messageBoards;
        Player _player;
        static public Dictionary<int, Chest> _chests;
        List<NetworkPlayer> _networkPlayers;
        static public Dictionary<int, Box> _boxes;
        static public Dictionary<int, Door> _doors;
        static public List<int> _boxesToSend;
        static public List<int> _doorsToSend;
        static public List<int> _chestsToSend;
        static public List<int> _boxesDestroyed;
        static public List<int> _doorsDestroyed;
        static public List<int> _chestsDestroyed;
        public MapManager()
        {
            _graves = new List<Grave>();
            _messageBoards = new List<MessageBoard>();
            _chests = new Dictionary<int, Chest>();
            _boxes = new Dictionary<int, Box>();
            _doors = new Dictionary<int, Door>();
            _boxesToSend = new List<int>();
            _chestsToSend = new List<int>();
            _doorsToSend = new List<int>();
            _boxesDestroyed = new List<int>();
            _doorsDestroyed = new List<int>();
            _chestsDestroyed = new List<int>();
        }
        public void Initialize(Player player)
        {
            _player = player;
        }
        public void Initialize(List<NetworkPlayer> networkPlayers)
        {
            _networkPlayers = networkPlayers;
        }
        public void Update()
        {
            if (!Game_Client._isServer)
            {
                foreach (var item in _messageBoards)
                {
                    if (_player != null)
                        item.Update(_player.RectangleMovement);
                }
            }
            foreach (var grave in _graves)
            {
                if (_player != null)
                    grave.Update(_player.RectangleMovement);
                else if (_networkPlayers != null)
                {
                    foreach (var player in _networkPlayers)
                    {
                        grave.Update(player.RectangleMovement);
                    }
                }
            }
            _graves.RemoveAll(item => item._destroy);
            if (!Game_Client._isMultiplayer && !Game_Client._isServer)
            {
                foreach (var item in _boxesToSend)
                {
                    AudioManager.PlaySound("DestroyBox");
                    _boxes.Remove(item);
                }
                _boxesToSend.Clear();
                foreach (var item in _doorsToSend)
                {
                    _doors.Remove(item);
                }
                _doorsToSend.Clear();
                foreach (var item in _chestsToSend)
                {
                    _chests.Remove(item);
                }
                _chestsToSend.Clear();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in _messageBoards)
            {
                item.Draw(spriteBatch, _player._input._isGamePad);
            }
        }
        public static void ResetMap()
        {
            _messageBoards.Clear();
            _graves.Clear();
            _chests.Clear();
            _boxes.Clear();
            _doors.Clear();
            _boxesToSend.Clear();
            _chestsToSend.Clear();
            _doorsToSend.Clear();
            _boxesDestroyed.Clear();
            _doorsDestroyed.Clear();
            _chestsDestroyed.Clear();
        }
        public void ResetGraphics()
        {
            _messageBoards.ForEach(x => x.ResetGraphics());
        }
    }
}
