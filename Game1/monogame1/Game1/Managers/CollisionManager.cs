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
        static private List<NetworkPlayer> _other_players;
        static private Player _player;
        static private List<Simple_Enemy> _enemies;

        public CollisionManager()
        {
        }
        public void Initialize(List<NetworkPlayer> other_players, Player player, List<Simple_Enemy> enemies)
        {
            _other_players = other_players;
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
            if (IsCollidingLeft(rectangle, _player.Rectangle, velocity) || IsCollidingRight(rectangle, _player.Rectangle, velocity) || IsCollidingTop(rectangle, _player.Rectangle, velocity) || IsCollidingBottom(_player.Rectangle, _player.Rectangle, velocity))
            {
                _player._health._health_left -= dmg;
                return true;
            }
            return false;
        }
        static public bool isColidedWithEnemies(Rectangle rectangle, Vector2 velocity, int dmg)
        {
            foreach (var enemy in _enemies)
            {
                if (IsCollidingLeft(rectangle, enemy.Rectangle, velocity) || IsCollidingRight(rectangle, enemy.Rectangle, velocity) || IsCollidingTop(rectangle, enemy.Rectangle, velocity) || IsCollidingBottom(rectangle, enemy.Rectangle, velocity))
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
                if (IsCollidingLeft(rectangle, wall, velocity) || IsCollidingRight(rectangle, wall, velocity) || IsCollidingTop(rectangle, wall, velocity) || IsCollidingBottom(rectangle, wall, velocity))
                    return true;
            }
            return false;
        }
        static public bool isCollidingChests(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var chest in MapManager._chests)
            {
                if (IsCollidingLeft(rectangle, chest.Rectangle, velocity) || IsCollidingRight(rectangle, chest.Rectangle, velocity) || IsCollidingTop(rectangle, chest.Rectangle, velocity) || IsCollidingBottom(chest.Rectangle, chest.Rectangle, velocity))
                {
                    chest.Open();
                    return true;
                }
            }
            return false;
        }
        static public bool isCollidingBoxes(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var box in MapManager._boxes)
            {
                if (IsCollidingLeft(rectangle, box.Rectangle, velocity) || IsCollidingRight(rectangle, box.Rectangle, velocity) || IsCollidingTop(rectangle, box.Rectangle, velocity) || IsCollidingBottom(box.Rectangle, box.Rectangle, velocity))
                {
                    box.Destroy();
                    return true;
                }
            }
            return false;
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
                if (wall.Right > rectangle.Left + velocity.X &&
                    wall.Left < rectangle.Left &&
                    wall.Bottom > rectangle.Top &&
                    wall.Top < rectangle.Bottom)
                    return true;
            }
            return false;
        }

        static public bool IsCollidingRightWalls(Rectangle rectangle, Vector2 velocity)
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

        static public bool IsCollidingTopWalls(Rectangle rectangle, Vector2 velocity)
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

        static public bool IsCollidingBottomWalls(Rectangle rectangle, Vector2 velocity)
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
