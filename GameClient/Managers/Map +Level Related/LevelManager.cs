﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
namespace GameClient
{
    public class LevelManager
    {
        private List<Player> _players;
        ProgressManager _progressManager;
        private readonly TileManager _tileManager;
        Coord _coord_Player;
        static public Vector2 _spawnPoint;
        List<NetworkPlayer> _networkPlayers;
        static public int startingLevel = 1;
        static public int startingLevelServer = 23;
        static public int _currentLevel = -1;
        static public bool _sendNewLevel;
        Game_Client _game_client;
        static private int _maxLevel = 23;
        public LevelManager(Game_Client game_client, TileManager tileManager)
        {
            _game_client = game_client;
            _tileManager = tileManager;
        }
        public void LoadNewLevel(int num = 0)
        {
            if (num > _maxLevel)
                num = 1;
            if (!Game_Client._isServer)
            {
                _game_client.ResetGame(false);
            }
            if (num != 0)
            {
                _currentLevel = num;
                _spawnPoint = _tileManager.LoadMap(_currentLevel);
                _players.ForEach(player => player.PositionPlayerFeetAt(_spawnPoint));
            }
            else
            {
                if (_currentLevel == _maxLevel)
                    _currentLevel = 0;
                _spawnPoint = _tileManager.LoadMap(++_currentLevel);
                _players.ForEach(player => player.PositionPlayerFeetAt(_spawnPoint));
            }
            if (!Game_Client._isServer)
            {
                AudioManager.PlaySong(_currentLevel);
                if (!Game_Client._isMultiplayer)
                    _progressManager.CreateProgressData();
            }
            _sendNewLevel = true;
        }
        public void Initialize(List<Player> players, ProgressManager progressManager)
        {
            _players = players;
            _progressManager = progressManager;
        }
        public void Initialize(List<NetworkPlayer> networkPlayers, ProgressManager progressManager)
        {
            _networkPlayers = networkPlayers;
            _progressManager = progressManager;
        }
        public bool Update()
        {
            if (!Game_Client._isMultiplayer)
            {
                foreach (Player player in _players)
                {
                    _coord_Player = TileManager.GetCoordTile(player.Position_Feet);
                    if (_coord_Player.X + 1 >= TileManager._map.Width)
                    {
                        LoadNewLevel();
                        return true; // return true if load next level
                    }
                }
                if (!_sendNewLevel)
                {
                    if (_networkPlayers != null)
                    {
                        foreach (var player in _networkPlayers)
                        {
                            _coord_Player = TileManager.GetCoordTile(player.Position_Feet);
                            if (_coord_Player.X + 1 >= TileManager._map.Width)
                            {
                                return true;// return true if load next level
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
