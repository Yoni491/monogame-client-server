using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class LevelManager
    {
        private Player _player;
        private readonly TileManager _tileManager;
        Coord _coord_Player;
        static public Vector2 _spawnPoint;
        List<NetworkPlayer> _networkPlayers;
        static public int _currentLevel = 3;
        static public bool _sendNewLevel;

        public LevelManager( TileManager tileManager)
        {
            _tileManager = tileManager;
        }
        public void LoadNewLevel(int num=0)
        {
            EnemyManager.Reset();
            ItemManager.Reset();
            if (num != 0)
            {
                _currentLevel = num;
                _spawnPoint = _tileManager.LoadMap(_currentLevel);
                _player.PositionPlayerFeetAt(_spawnPoint);
            }
            else
            {
                if (_currentLevel == 5)
                    _currentLevel = 0;
                _spawnPoint = _tileManager.LoadMap(++_currentLevel);
                if (_player != null)
                    _player.PositionPlayerFeetAt(_spawnPoint);
            }
            _sendNewLevel = true;

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
            if (!Game_Client._IsMultiplayer)
            {
                if (_player != null)
                {
                    _coord_Player = TileManager.GetCoordTile(_player.Position_Feet);
                    if (_coord_Player.X + 2 >= TileManager._map.Width)
                    {
                        LoadNewLevel();
                    }
                }
                if (!_sendNewLevel)
                {
                    if (_networkPlayers != null)
                    {
                        foreach (var player in _networkPlayers)
                        {
                            _coord_Player = TileManager.GetCoordTile(player.Position_Feet);
                            if (_coord_Player.X + 2 >= TileManager._map.Width)
                            {
                                LoadNewLevel();
                            }
                        }
                    }
                }
            }
        }

    }
}
