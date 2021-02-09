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
        static private List<OtherPlayer> _other_players;
        static private Player _player;
        static private List<Simple_Enemy> _enemies;

        public CollisionManager()
        {
        }
        public void Initialize(List<OtherPlayer> other_players, Player player, List<Simple_Enemy> enemies)
        {
            _other_players = other_players;
            _player = player;
            _enemies = enemies;
        }

        static public bool isColidedWithPlayer(Rectangle rectangle, int dmg)
        {

            if (rectangle.X > _player.Rectangle.X && rectangle.X < _player.Rectangle.X + _player.Rectangle.Width)
                if (rectangle.Y > _player.Rectangle.Y && rectangle.Y < _player.Rectangle.Y + _player.Rectangle.Height)
                {
                    _player._health._health_left -= dmg;
                    //_player.DealDamage(dmg);
                    return true;
                }


            return false;
        }
        static public bool isColidedWithEnemies(Rectangle rectangle, int dmg)
        {
            foreach (var enemy in _enemies)
            {
                if (rectangle.X > enemy.Rectangle.X && rectangle.X < enemy.Rectangle.X + enemy.Rectangle.Width)
                    if (rectangle.Y > enemy.Rectangle.Y && rectangle.Y < enemy.Rectangle.Y + enemy.Rectangle.Height)
                    {
                        enemy.DealDamage(dmg);
                        //if (_player._health._health_left <= 0)
                        //{
                        //    _player._destroy = true;
                        //}
                        return true;
                    }
            }
            return false;
        }
        static public bool isMouseCollidingRectangle(Rectangle rectangle)
        {
            Vector2 mouse = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            if (mouse.X > rectangle.X && mouse.X < rectangle.X + rectangle.Width)
                if (mouse.Y > rectangle.Y && mouse.Y < rectangle.Y + rectangle.Height)
                    return true;
            return false;
        }
        static public bool isCollidingWalls(Rectangle rectangle,Vector2 velocity)
        {
            return IsCollidingLeft(rectangle, velocity) || IsCollidingRight(rectangle, velocity) || IsCollidingTop(rectangle, velocity) || IsCollidingBottom(rectangle, velocity);
        }
        static public bool isCollidingChests(Rectangle rectangle)
        {
            foreach (var chest in MapManager._chests)
            {
                if (rectangle.X > chest.Rectangle.X && rectangle.X < chest.Rectangle.X + chest.Rectangle.Width)
                    if (rectangle.Y > chest.Rectangle.Y && rectangle.Y < chest.Rectangle.Y + chest.Rectangle.Height)
                    {
                        chest.Open();
                        return true;
                    }
            }
            return false;
        }
        static public bool isCollidingBoxes(Rectangle rectangle)
        {
            foreach (var box in MapManager._boxes)
            {
                if (rectangle.X > box.Rectangle.X && rectangle.X < box.Rectangle.X + box.Rectangle.Width)
                    if (rectangle.Y > box.Rectangle.Y && rectangle.Y < box.Rectangle.Y + box.Rectangle.Height)
                    {
                        box.Destroy();
                        return true;
                    }
            }
            return false;
        }

        static public bool IsCollidingLeft(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var wall in TileManager._walls)
            {
                if (wall.Right > rectangle.Left + velocity.X &&
                    wall.Left < rectangle.Left &&
                    wall.Bottom > rectangle.Top &&
                    wall.Top < rectangle.Bottom)
                    return true;
            }
            return false;
        }

        static public bool IsCollidingRight(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var wall in TileManager._walls)
            {
                if (wall.Left < rectangle.Right + velocity.X &&
                    wall.Right > rectangle.Right &&
                    wall.Bottom > rectangle.Top &&
                    wall.Top < rectangle.Bottom)
                    return true;
            }
            return false;
        }

        static public bool IsCollidingTop(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var wall in TileManager._walls)
            {
                if (wall.Bottom > rectangle.Top + velocity.Y &&
                    wall.Top < rectangle.Top &&
                    wall.Right > rectangle.Left &&
                    wall.Left < rectangle.Right)
                    return true;
            }
            return false;
        }

        static public bool IsCollidingBottom(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var wall in TileManager._walls)
            {
                if (wall.Top < rectangle.Bottom + velocity.Y &&
                    wall.Bottom > rectangle.Bottom &&
                    wall.Right > rectangle.Left &&
                    wall.Left < rectangle.Right)
                    return true;
            }
            return false;
        }

    }
}
