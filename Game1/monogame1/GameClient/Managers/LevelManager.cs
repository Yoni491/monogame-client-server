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
        Vector2 _spawnPoint;
        List<NetworkPlayer> _networkPlayers;

        public LevelManager( TileManager tileManager)
        {
            _tileManager = tileManager;
        }
        public void LoadMap(int num)
        {
            _spawnPoint = _tileManager.LoadMap(num);
        }
        public void Initialize(Player player,List<NetworkPlayer> networkPlayers)
        {
            _player = player;
            if (_player!=null)
                _player.PositionPlayerFeetAt(_spawnPoint);
            _networkPlayers = networkPlayers;
        }
        public void Update()
        {
            if (_player != null)
            {
                _coord_Player = TileManager.GetCoordTile(_player.Position_Feet);
                if (_coord_Player.X + 2 >= TileManager._map.Width)
                {
                    _player.PositionPlayerFeetAt(_tileManager.LoadMap(1));
                    PathFindingManager.UseAStar = true;
                    EnemyManager.Reset();
                }
            }
        }
        public void Draw()
        {

        }
    }
}
