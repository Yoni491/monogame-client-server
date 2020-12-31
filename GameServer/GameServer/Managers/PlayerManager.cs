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
        private List<Simple_Enemy> _enemies;
        public PlayerManager(List<Player> players,ContentManager contentManager,GraphicsDevice graphicsDevice, List<Simple_Enemy> enemies)
        {
            _players = players;
            _enemies = enemies;
        }
    }
}
