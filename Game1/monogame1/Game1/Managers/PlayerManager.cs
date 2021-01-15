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
        private Player _player;
        ItemManager _itemManager;
        CollectionManager _collectionManager;
        InventoryManager _inventoryManager;
        public PlayerManager(List<OtherPlayer> players, CollectionManager collectionManager)
        {
            _players = players;
            _collectionManager = collectionManager;

        }
        public void updateOtherPlayerTexture()
        {
            NetworkManagerClient._updateOtherPlayerTexture = false;
            foreach (var player in _players)
            {
                if (player.updateTexture)
                {
                    player.UpdateTexture(GraphicManager.GetAnimationManager_spriteMovement(10,1.5f));
                }
            }
        }
        public OtherPlayer AddOtherPlayer(int playerNum)
        {
            OtherPlayer otherPlayer = new OtherPlayer(Vector2.Zero, 100, playerNum, _collectionManager.GetGunCopy(3,0.7f,false));
            _players.Add(otherPlayer);
            NetworkManagerClient._updateOtherPlayerTexture = true;
            return otherPlayer;
        }
        public Player AddPlayer(ItemManager itemManager, InventoryManager inventoryManager, GraphicsDevice graphicsDevice)
        {
            _itemManager = itemManager;
            _inventoryManager = inventoryManager;
            Input input = new Input(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space);
            Vector2 position = new Vector2(graphicsDevice.Viewport.Width/2 + 100, graphicsDevice.Viewport.Height / 2 + 100);
            _player = new Player(GraphicManager.GetAnimationManager_spriteMovement(3,1.5f), position, input, 100,this,_itemManager,_inventoryManager);
            _player.EquipGun(_collectionManager.GetGunCopy(3,0.7f,false));
            _player.EquipMeleeWeapon(_collectionManager.GetMeleeWeaponCopy(0,0.7f));
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
                    if (Vector2.Distance(position, player.Position_Feet) < closest_object_distance)
                    {
                        closest_object_distance = Vector2.Distance(position, player.Position_Feet);
                        player_position = player.Position_Feet;
                    }
                }
            if (Vector2.Distance(position, _player.Position_Feet) < closest_object_distance)
            {
                closest_object_distance = Vector2.Distance(position, _player.Position_Feet);
                player_position = _player.Position_Feet;
            }
            return player_position;
        }
        
    }
}
