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
        private Player _player;
        ItemManager _itemManager;
        CollectionManager _collectionManager;
        InventoryManager _inventoryManager;
        public PlayerManager(List<OtherPlayer> players, ContentManager contentManager, GraphicsDevice graphicsDevice, CollectionManager collectionManager)
        {
            _players = players;
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
                    player.UpdateTexture(GraphicManager.GetAnimationManager_spriteMovement(10));
                }
            }
        }
        public OtherPlayer AddOtherPlayer(int playerNum)
        {
            OtherPlayer otherPlayer = new OtherPlayer(Vector2.Zero, 100, playerNum, _collectionManager.GetGunCopy(3));
            _players.Add(otherPlayer);
            Game_Client._updateOtherPlayerTexture = true;
            return otherPlayer;
        }
        public Player AddPlayer(ItemManager itemManager, InventoryManager inventoryManager)
        {
            _itemManager = itemManager;
            _inventoryManager = inventoryManager;
            Input input = new Input(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space);
            Vector2 position = new Vector2(200, 200);
            _player = new Player(GraphicManager.GetAnimationManager_spriteMovement(3), position, input, 100,this,_itemManager,_inventoryManager);
            _player.EquipGun(_collectionManager.GetGunCopy(1));
            _player.EquipMeleeWeapon(_collectionManager.GetMeleeWeaponCopy(0));
            return _player;
        }
        public void Update(GameTime gameTime, List<Simple_Enemy> enemies)
        {
            _player.Update(gameTime, enemies);
            foreach (var player in _players)
                player.Update(gameTime, enemies);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _player.Draw(spriteBatch);
            foreach (var sprite in _players)
                sprite.Draw(spriteBatch);
        }
        public Vector2 getClosestPlayerToPosition(Vector2 position)
        {
            float closest_object_distance = float.MaxValue;
            Vector2 player_position = position;
            if (_players != null)
                foreach (var player in _players)
                {
                    if (Vector2.Distance(position, player.Position) < closest_object_distance)
                    {
                        closest_object_distance = Vector2.Distance(position, player.Position);
                        player_position = player.Position;
                    }
                }
            if (Vector2.Distance(position, _player.Position) < closest_object_distance)
            {
                closest_object_distance = Vector2.Distance(position, _player.Position);
                player_position = _player.Position;
            }
            return player_position;
        }
        
    }
}
