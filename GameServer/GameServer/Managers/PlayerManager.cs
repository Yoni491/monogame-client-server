using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;


namespace GameServer
{
    public class PlayerManager
    {
        private List<Player> _players;
        //private ContentManager _contentManager;
        //private GraphicsDevice _graphicsDevice;
        private List<Simple_Enemy> _enemies;
        //private Texture2D _bullet_texture; 
        public PlayerManager(List<Player> players,ContentManager contentManager,GraphicsDevice graphicsDevice, List<Simple_Enemy> enemies)
        {
            _players = players;
            _enemies = enemies;
            //_contentManager = contentManager;
            //_graphicsDevice = graphicsDevice;
            //_bullet_texture = _contentManager.Load<Texture2D>("etc/bullet");
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
                //Texture2D texture = _contentManager.Load<Texture2D>("Patreon sprites 1/3");
                
                Vector2 position = new Vector2(200, 200);
                AddPlayerSprite(position);
                //Texture2D gun_texture = _contentManager.Load<Texture2D>("Weapons/2");
                Gun gun = new Gun(new Vector2(0, 0), _enemies);
                //_players[0].EquipGun(gun);
            }
            
        }
        public void AddPlayerSprite(Vector2 i_position)
        {
            _players.Add(new Player(
            i_position,
            100
            ));


        }

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    foreach (var sprite in _players)
        //        sprite.Draw(spriteBatch);
        //}

    }
}
