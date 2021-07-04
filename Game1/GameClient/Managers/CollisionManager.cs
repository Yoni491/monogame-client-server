using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameClient
{
    public class CollisionManager
    {
        static private List<NetworkPlayer> _networkPlayers;
        static private Player _player;
        static private List<SimpleEnemy> _enemies;

        public CollisionManager()
        {
        }
        public void Initialize(List<NetworkPlayer> other_players, Player player, List<SimpleEnemy> enemies)
        {
            _networkPlayers = other_players;
            _player = player;
            _enemies = enemies;
        }
        static public bool isMouseCollidingRectangle(Rectangle rectangle)
        {
            Vector2 mouse = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            if (mouse.X > rectangle.X && mouse.X < rectangle.X + rectangle.Width)
                if (mouse.Y > rectangle.Y && mouse.Y < rectangle.Y + rectangle.Height)
                    return true;
            return false;
        }
        static public bool isColidedWithPlayer(Rectangle rectangle, Vector2 velocity, int dmg)
        {
            if (_player == null)
                return false;
            if (IsCollidingLeft(rectangle, _player.Rectangle, velocity) || IsCollidingRight(rectangle, _player.Rectangle, velocity) || 
                IsCollidingTop(rectangle, _player.Rectangle, velocity) || IsCollidingBottom(rectangle, _player.Rectangle, velocity))
            {
                _player._health._health_left -= dmg;
                if (_player._health._health_left <= 0)
                    _player._dead = true;
                return true;
            }

            if (IsCollidingLeft(rectangle, _player.RectangleMovement, velocity) || IsCollidingRight(rectangle, _player.RectangleMovement, velocity) || 
                IsCollidingTop(rectangle, _player.RectangleMovement, velocity) || IsCollidingBottom(rectangle, _player.RectangleMovement, velocity))
            {
                _player._health._health_left -= dmg;
                return true;
            }
            return false;
        }
        static public bool isColidedWithNetworkPlayers(Rectangle rectangle, Vector2 velocity, int dmg)
        {
            if (_networkPlayers == null)
                return false;
            foreach (var player in _networkPlayers)
            {
                if (IsCollidingLeft(rectangle, player.Rectangle, velocity) || IsCollidingRight(rectangle, player.Rectangle, velocity) || 
                    IsCollidingTop(rectangle, player.Rectangle, velocity) || IsCollidingBottom(rectangle, player.Rectangle, velocity))
                {
                    return true;
                }
                if (IsCollidingLeft(rectangle, player.RectangleMovement, velocity) || IsCollidingRight(rectangle, player.RectangleMovement, velocity) || 
                    IsCollidingTop(rectangle, player.RectangleMovement, velocity) || IsCollidingBottom(rectangle, player.RectangleMovement, velocity))
                {
                    return true;
                }
            }
            return false;
        }
        static public bool isColidedWithEnemies(Rectangle rectangle, Vector2 velocity, int dmg)
        {
            foreach (var enemy in _enemies)
            {
                if (IsCollidingLeft(rectangle, enemy.Rectangle, velocity) || IsCollidingRight(rectangle, enemy.Rectangle, velocity) || 
                    IsCollidingTop(rectangle, enemy.Rectangle, velocity) || IsCollidingBottom(rectangle, enemy.Rectangle, velocity))
                {
                    enemy.DealDamage(dmg);
                    return true;
                }
            }
            return false;
        }
        static public bool isCollidingWalls(Rectangle rectangle, Vector2 velocity)
        {

            foreach (var wall in TileManager._walls)
            {
                Rectangle Wallrectangle = wall.Value._rectangle;
                if (IsCollidingLeft(rectangle, Wallrectangle, velocity) || IsCollidingRight(rectangle, Wallrectangle, velocity)
                    || IsCollidingTop(rectangle, Wallrectangle, velocity) || IsCollidingBottom(rectangle, Wallrectangle, velocity))
                    return true;
            }
            foreach (var wall in TileManager._destroyableWalls)
            {
                Rectangle Wallrectangle = wall.Value._rectangle;
                if (IsCollidingLeft(rectangle, Wallrectangle, velocity) || IsCollidingRight(rectangle, Wallrectangle, velocity)
                    || IsCollidingTop(rectangle, Wallrectangle, velocity) || IsCollidingBottom(rectangle, Wallrectangle, velocity))
                    return true;
            }
            return false;
        }
        static public Chest isCollidingChests(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var item in MapManager._chests)
            {
                Chest chest = item.Value;
                if (IsCollidingLeft(rectangle, chest.Rectangle, velocity) || IsCollidingRight(rectangle, chest.Rectangle, velocity) || 
                    IsCollidingTop(rectangle, chest.Rectangle, velocity) || IsCollidingBottom(rectangle, chest.Rectangle, velocity))
                {
                    return chest;
                }
            }
            return null;
        }
        static public bool isCollidingBoxes(Rectangle rectangle, Vector2 velocity, int dmg)
        {
            foreach (var item in MapManager._boxes)
            {
                Box box = item.Value;
                if (!box._destroy)
                {
                    if (IsCollidingLeft(rectangle, box.Rectangle, velocity) || IsCollidingRight(rectangle, box.Rectangle, velocity) || 
                        IsCollidingTop(rectangle, box.Rectangle, velocity) || IsCollidingBottom(rectangle, box.Rectangle, velocity))
                    {
                        if(dmg>0)
                            box.Destroy();
                        return true;
                    }
                }
            }
            return false;
        }
        static public Door IsCollidingDoors(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var item in MapManager._doors)
            {
                Door door = item.Value;
                if (!door._destroy)
                {
                    if (IsCollidingLeft(rectangle, door.Rectangle, velocity) || IsCollidingRight(rectangle, door.Rectangle, velocity) || 
                        IsCollidingTop(rectangle, door.Rectangle, velocity) || IsCollidingBottom(rectangle, door.Rectangle, velocity))
                    {
                        return door;
                    }
                }
            }
            return null;
        }
        static public bool IsCollidingLeft(Rectangle rectangle, Rectangle wall, Vector2 velocity)
        {
            if (wall.Right > rectangle.Left + velocity.X &&
                wall.Left < rectangle.Left &&
                wall.Bottom > rectangle.Top &&
                wall.Top < rectangle.Bottom)
                return true;
            return false;
        }

        static public bool IsCollidingRight(Rectangle rectangle, Rectangle wall, Vector2 velocity)
        {
            if (wall.Left < rectangle.Right + velocity.X &&
                wall.Right > rectangle.Right &&
                wall.Bottom > rectangle.Top &&
                wall.Top < rectangle.Bottom)
                return true;
            return false;
        }

        static public bool IsCollidingTop(Rectangle rectangle, Rectangle wall, Vector2 velocity)
        {
            if (wall.Bottom > rectangle.Top + velocity.Y &&
                wall.Top < rectangle.Top &&
                wall.Right > rectangle.Left &&
                wall.Left < rectangle.Right)
                return true;
            return false;
        }

        static public bool IsCollidingBottom(Rectangle rectangle, Rectangle wall, Vector2 velocity)
        {
            if (wall.Top < rectangle.Bottom + velocity.Y &&
                wall.Bottom > rectangle.Bottom &&
                wall.Right > rectangle.Left &&
                wall.Left < rectangle.Right)
                return true;

            return false;
        }
        static public bool IsCollidingLeftWalls(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var wall in TileManager._walls)
            {
                Rectangle Wallrectangle = wall.Value._rectangle;
                if (Wallrectangle.Right > rectangle.Left + velocity.X &&
                    Wallrectangle.Left < rectangle.Left &&
                    Wallrectangle.Bottom > rectangle.Top &&
                    Wallrectangle.Top < rectangle.Bottom)
                    return true;
            }
            foreach (var wall in TileManager._destroyableWalls)
            {
                Rectangle Wallrectangle = wall.Value._rectangle;
                if (Wallrectangle.Right > rectangle.Left + velocity.X &&
                    Wallrectangle.Left < rectangle.Left &&
                    Wallrectangle.Bottom > rectangle.Top &&
                    Wallrectangle.Top < rectangle.Bottom)
                    return true;
            }
            return false;
        }

        static public bool IsCollidingRightWalls(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var wall in TileManager._walls)
            {
                Rectangle Wallrectangle = wall.Value._rectangle;
                if (Wallrectangle.Left < rectangle.Right + velocity.X &&
                    Wallrectangle.Right > rectangle.Right &&
                    Wallrectangle.Bottom > rectangle.Top &&
                    Wallrectangle.Top < rectangle.Bottom)
                    return true;
            }
            foreach (var wall in TileManager._destroyableWalls)
            {
                Rectangle Wallrectangle = wall.Value._rectangle;
                if (Wallrectangle.Left < rectangle.Right + velocity.X &&
                    Wallrectangle.Right > rectangle.Right &&
                    Wallrectangle.Bottom > rectangle.Top &&
                    Wallrectangle.Top < rectangle.Bottom)
                    return true;
            }
            return false;
        }

        static public bool IsCollidingTopWalls(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var wall in TileManager._walls)
            {
                Rectangle Wallrectangle = wall.Value._rectangle;
                if (Wallrectangle.Bottom > rectangle.Top + velocity.Y &&
                    Wallrectangle.Top < rectangle.Top &&
                    Wallrectangle.Right > rectangle.Left &&
                    Wallrectangle.Left < rectangle.Right)
                    return true;
            }
            foreach (var wall in TileManager._destroyableWalls)
            {
                Rectangle Wallrectangle = wall.Value._rectangle;
                if (Wallrectangle.Bottom > rectangle.Top + velocity.Y &&
                    Wallrectangle.Top < rectangle.Top &&
                    Wallrectangle.Right > rectangle.Left &&
                    Wallrectangle.Left < rectangle.Right)
                    return true;
            }
            return false;
        }

        static public bool IsCollidingBottomWalls(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var wall in TileManager._walls)
            {
                Rectangle Wallrectangle = wall.Value._rectangle;
                if (Wallrectangle.Top < rectangle.Bottom + velocity.Y &&
                    Wallrectangle.Bottom > rectangle.Bottom &&
                    Wallrectangle.Right > rectangle.Left &&
                    Wallrectangle.Left < rectangle.Right)
                    return true;
            }
            foreach (var wall in TileManager._destroyableWalls)
            {
                Rectangle Wallrectangle = wall.Value._rectangle;
                if (Wallrectangle.Top < rectangle.Bottom + velocity.Y &&
                    Wallrectangle.Bottom > rectangle.Bottom &&
                    Wallrectangle.Right > rectangle.Left &&
                    Wallrectangle.Left < rectangle.Right)
                    return true;
            }
            return false;
        }

    }
}
