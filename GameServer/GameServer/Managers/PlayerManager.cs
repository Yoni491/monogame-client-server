using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;


namespace Game1
{
    public class PlayerManager
    {
        private List<Player> _players;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;
        private List<Simple_Enemy> _enemies;
        private Texture2D _bullet_texture; 
        public PlayerManager(List<Player> players,ContentManager contentManager,GraphicsDevice graphicsDevice, List<Simple_Enemy> enemies)
        {
            _players = players;
            _enemies = enemies;
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
            _bullet_texture = _contentManager.Load<Texture2D>("etc/bullet");
            AddPlayers(4);
        }
        public void AddPlayers(int i_amount)
        {
            
            for (int i = 0; i < i_amount; i++)
            {
                AddPlayer(i);
            }
        }
        public void AddPlayer(int player)
        {
            if (player == 0)
            {
                Texture2D texture = _contentManager.Load<Texture2D>("Patreon sprites 1/3");
                Input input = new Input()
                {
                    Up = Keys.W,
                    Down = Keys.S,
                    Left = Keys.A,
                    Right = Keys.D,
                };
                Vector2 position = new Vector2(200, 200);
                AddPlayerSprite(texture, input, position);
                Texture2D gun_texture = _contentManager.Load<Texture2D>("Weapons/2");
                Gun gun = new Gun(gun_texture, new Vector2(0, 0), _bullet_texture, _enemies);
                _players[0].EquipGun(gun);
            }
            else if (player == 1)
            {
                Texture2D texture = _contentManager.Load<Texture2D>("Patreon sprites 1/7");
                Input input = new Input()
                {
                    Up = Keys.Up,
                    Down = Keys.Down,
                    Left = Keys.Left,
                    Right = Keys.Right,
                };
                Vector2 position = new Vector2(150, 150);
                AddPlayerSprite(texture, input, position);
                Texture2D gun_texture = _contentManager.Load<Texture2D>("Weapons/3");
                Gun gun = new Gun(gun_texture, new Vector2(0, 0), _bullet_texture, _enemies);
                _players[1].EquipGun(gun);
            }
            else if (player == 2)
            {
                Texture2D texture = _contentManager.Load<Texture2D>("Patreon sprites 1/5");
                Input input = new Input()
                {
                    Up = Keys.NumPad8,
                    Down = Keys.NumPad5,
                    Left = Keys.NumPad4,
                    Right = Keys.NumPad6,
                };
                Vector2 position = new Vector2(200, 200);
                AddPlayerSprite(texture, input, position);
                Texture2D gun_texture = _contentManager.Load<Texture2D>("Weapons/4");
                Gun gun = new Gun(gun_texture, new Vector2(0, 0), _bullet_texture, _enemies);
                _players[2].EquipGun(gun);
            }
            else if (player == 3)
            {
                Texture2D texture = _contentManager.Load<Texture2D>("Patreon sprites 1/6");
                Input input = new Input()
                {
                    Up = Keys.I,
                    Down = Keys.K,
                    Left = Keys.J,
                    Right = Keys.L,
                };
                Vector2 position = new Vector2(50, 50);
                AddPlayerSprite(texture, input, position);
                Texture2D gun_texture = _contentManager.Load<Texture2D>("Weapons/5");
                Gun gun = new Gun(gun_texture, new Vector2(0, 0), _bullet_texture, _enemies);
                _players[3].EquipGun(gun);
            }
        }
        public void AddPlayerSprite(Texture2D i_texture, Input i_input, Vector2 i_position)
        {
            _players.Add(new Player(new Dictionary<string, Animation>()
            {
            { "WalkDown", new Animation(SpriteManager.Resize4x4Sprite(i_texture,0,_graphicsDevice), 4) },
            { "WalkLeft", new Animation(SpriteManager.Resize4x4Sprite(i_texture,1,_graphicsDevice), 4) },
            { "WalkRight", new Animation(SpriteManager.Resize4x4Sprite(i_texture,2,_graphicsDevice), 4) },
            { "WalkUp", new Animation(SpriteManager.Resize4x4Sprite(i_texture,3,_graphicsDevice), 4) },
            },
            i_position,
            i_input,
            100
            ));


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var sprite in _players)
                sprite.Draw(spriteBatch);
        }

    }
}
