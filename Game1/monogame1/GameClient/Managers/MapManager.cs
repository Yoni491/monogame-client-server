using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class MapManager
    {
        static public List<Grave> _graves;
        static public List<MassageBoard> _massageBoards;
        Player _player;
        static public Dictionary<int, Chest> _chests;
        List<NetworkPlayer> _networkPlayers;
        static public Dictionary<int, Box> _boxes;
        static public Dictionary<int, Door> _doors;
        static public List<int> _boxesToSend;
        static public List<int> _doorsToSend;
        static public List<int> _chestsToSend;
        public MapManager()
        {
            _graves = new List<Grave>();
            _massageBoards = new List<MassageBoard>();
            _chests = new Dictionary<int, Chest>();
            _boxes = new Dictionary<int, Box>();
            _doors = new Dictionary<int, Door>();
            _boxesToSend = new List<int>();
            _chestsToSend = new List<int>();
            _doorsToSend = new List<int>();
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
            foreach (var item in _massageBoards)
            {
                if (_player != null)
                    item.Update(_player.RectangleMovement);
            }
            foreach (var grave in _graves)
            {
                if(_player!=null)
                    grave.Update(_player.RectangleMovement);
                else if(_networkPlayers!=null)
                {
                    foreach (var player in _networkPlayers)
                    {
                        grave.Update(player.RectangleMovement);
                    }
                }
            }
            _graves.RemoveAll(item => item._destroy == true);
            if (!Game_Client._IsMultiplayer && !Game_Client._isServer)
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
            foreach (var item in _massageBoards)
            {
                item.Draw(spriteBatch);
            }
        }
        public static void ResetMap()
        {
            _massageBoards.Clear();
            _graves.Clear();
            _chests.Clear();
            _boxes.Clear();
            _doors.Clear();
            _boxesToSend.Clear();
            _chestsToSend.Clear();
        }
    }
}
