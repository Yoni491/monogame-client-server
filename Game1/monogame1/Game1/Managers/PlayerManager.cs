using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace GameClient
{
    public class PlayerManager
    {
        private List<OtherPlayer> _players;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;
        private List<Simple_Enemy> _enemies;
        private Texture2D _bullet_texture;
        private Player _player;
        CollectionManager _collectionManager;
        public PlayerManager(List<OtherPlayer> players, ContentManager contentManager, GraphicsDevice graphicsDevice, List<Simple_Enemy> enemies, CollectionManager collectionManager)
        {
            _players = players;
            _enemies = enemies;
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
            _collectionManager = collectionManager;
        }
        public void updateOtherPlayerTexture()
        {
            Game_Client._updateOtherPlayerTexture = false;
            foreach (var player in _players)
            {
                if (player.updateTexture)
                {
                    Texture2D texture = _contentManager.Load<Texture2D>("Patreon sprites 1/3");
                    player.UpdateTexture(SpriteManager.GetAnimation4x4Dictionary(texture, _graphicsDevice));
                }
            }
        }
        public OtherPlayer AddOtherPlayer(int playerNum)
        {
            OtherPlayer otherPlayer = new OtherPlayer(Vector2.Zero, 100, playerNum, _collectionManager.GetGun(3));
            _players.Add(otherPlayer);
            Game_Client._updateOtherPlayerTexture = true;
            return otherPlayer;
        }
        public Player AddPlayer()
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
            _player = new Player(SpriteManager.GetAnimation4x4Dictionary(texture, _graphicsDevice), position, input, 100);
            _player.EquipGun(_collectionManager.GetGun(0));
            return _player;
        }
        public void Update(GameTime gameTime, List<Simple_Enemy> enemies)
        {
            _player.Update(gameTime, enemies);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _player.Draw(spriteBatch);
            foreach (var sprite in _players)
                sprite.Draw(spriteBatch);
        }

    }
}
