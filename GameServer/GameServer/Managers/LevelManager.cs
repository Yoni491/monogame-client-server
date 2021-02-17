using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class LevelManager
    {
        private Player _player;
        private readonly TileManager _tileManager;
        Coord _coord_Player;
        Vector2 _spawnPoint;

        public LevelManager( TileManager tileManager)
        {
            this._tileManager = tileManager;
            _spawnPoint = _tileManager.LoadMap(13);
        }
        public void Initialize(Player player)
        {
            _player = player;
            _player.Position = _spawnPoint;
        }
        //public void Update()
        //{
        //    _coord_Player = TileManager.GetCoordTile(_player.Position_Feet);
        //    if (_coord_Player.X + 2 >=TileManager._map.Width)
        //    {
        //        _player.PositionPlayerFeetAt(_tileManager.LoadMap(1));
        //        PathFindingManager.UseAStar = true;
        //        //EnemyManager.Reset();
        //    }
        //}
        public void Draw()
        {

        }
    }
}
