using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{

    public class BulletReach
    {
        public int _id;
        private Player _player;
        private List<NetworkPlayer> _networkPlayers;
        public Vector2 _reachablePlayerPos=Vector2.Zero;
        private Vector2 _position;
        public BulletReach(int id, Player player, List<NetworkPlayer> networkPlayers)
        {
            _id = id;
            _player = player;
            _networkPlayers = networkPlayers;
        }
        public void Update(Vector2 position)
        {
            _position = position;
        }
        public bool FindReachablePlayer()
        {
            if (_networkPlayers != null)
            {
                foreach (var player in _networkPlayers)
                {
                    Vector2 direction = player.Position_Feet - _position;
                    if(CheckIfReachable(player._gun._tipOfTheGun, player._gun._bullet, direction, player._gun))
                        _reachablePlayerPos = _player.Position_Feet;

                    direction = player.Position_Head - _position;
                    if(CheckIfReachable(player._gun._tipOfTheGun, player._gun._bullet, direction, player._gun))
                        _reachablePlayerPos = _player.Position_Feet;
                }
            }
            if(_player!=null)
            {
                Vector2 direction = Vector2.Normalize(_player.Position_Feet - _position);
                if (CheckIfReachable(_player._gun._tipOfTheGun, _player._gun._bullet, direction, _player._gun))
                    _reachablePlayerPos = _player.Position_Feet;
                direction = Vector2.Normalize(_player.Position_Head - _position);
                if(CheckIfReachable(_player._gun._tipOfTheGun, _player._gun._bullet, direction, _player._gun))
                    _reachablePlayerPos = _player.Position_Feet;
            }
            return false;
        }
        public bool CheckIfReachable(Vector2 _tipOfTheGun,Bullet _bullet,Vector2 _direction,Gun gun)
        {
            Vector2 tempPos;
            tempPos = _tipOfTheGun;
            Rectangle tempRec;

            while (true)
            {
                if (tempPos.X < 2000 && tempPos.X > 0 && tempPos.Y < 2000 && tempPos.Y > 0)
                {
                    tempRec = new Rectangle((int)tempPos.X, (int)tempPos.Y, _bullet.Rectangle.Width, _bullet.Rectangle.Height);
                    if (CollisionManager.isColidedWithPlayer(tempRec, Vector2.Zero, 0))
                    {
                        gun._MaxPointBulletReach = tempPos;
                        return true;
                    }
                    else if (CollisionManager.isColidedWithNetworkPlayers(tempRec, Vector2.Zero, 0))
                    {
                        gun._MaxPointBulletReach = tempPos;
                        return true;
                    }
                    if (CollisionManager.isCollidingWalls(tempRec, _direction * _bullet._speed))
                    {

                        gun._MaxPointBulletReach = tempPos;
                        return false;
                    }
                    tempPos += _direction * _bullet._speed;
                }
                else
                {
                    gun._MaxPointBulletReach = tempPos;
                    return false;
                }

            }
        }
    }
}
