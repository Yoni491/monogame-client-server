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
        private Gun _gun;
        private Bullet _bullet;
        public Vector2 _reachablePlayerPos=Vector2.Zero;
        public BulletReach(int id, Player player, List<NetworkPlayer> networkPlayers, Gun gun)
        {
            _id = id;
            _player = player;
            _networkPlayers = networkPlayers;
            _gun = gun;
            _bullet = _gun._bullet;
        }
        virtual public void FindReachablePlayer()
        {
            if (_networkPlayers != null)
            {
                try //try because if the server drops a player and bullet reach runs on another thread
                {
                    foreach (var player in _networkPlayers)
                    {
                        if (player._health._health_left > 0)
                        {
                            if (CheckIfReachable(player.Position_Head))
                            {
                                _reachablePlayerPos = player.Position_Head;
                                return;
                            }
                            if (CheckIfReachable(player.Position_Feet))
                            {
                                _reachablePlayerPos = player.Position_Feet;
                                return;
                            }
                        }
                    }
                }
                catch
                {
                    return;
                }
            }
            if(_player!=null)
            {
                if (CheckIfReachable(_player.Position_Head))
                {
                    _reachablePlayerPos = _player.Position_Head;
                    return;
                }
                if (CheckIfReachable(_player.Position_Feet))
                    _reachablePlayerPos = _player.Position_Feet;
            }
        }
        public bool CheckIfReachable(Vector2 _direction)
        {
            Vector2 tempPos;
            tempPos =  _gun.GetTipOfTheGun(_direction);
            Vector2 direction = Vector2.Normalize(_direction- tempPos);
            Rectangle tempRec;
            while (true)
            {
                if (tempPos.X < 2000 && tempPos.X > 0 && tempPos.Y < 2000 && tempPos.Y > 0)
                {
                    tempRec = new Rectangle((int)tempPos.X, (int)tempPos.Y, _bullet.Rectangle.Width, _bullet.Rectangle.Height);
                    if (CollisionManager.isColidedWithPlayer(tempRec, Vector2.Zero, 0))
                    {
                        _gun._MaxPointBulletReach = tempPos;
                        return true;
                    }
                    else if (CollisionManager.isColidedWithNetworkPlayers(tempRec, Vector2.Zero, 0))
                    {
                        _gun._MaxPointBulletReach = tempPos;
                        return true;
                    }
                    if (CollisionManager.isCollidingWalls(tempRec, direction * _bullet._speed))
                    {

                        //_gun._MaxPointBulletReach = tempPos;
                        return false;
                    }
                    tempPos += direction * _bullet._speed;
                }
                else
                {
                    _gun._MaxPointBulletReach = tempPos;
                    return false;
                }

            }
        }
    }
}
