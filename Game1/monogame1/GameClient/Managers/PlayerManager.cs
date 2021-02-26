using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace GameClient
{
    public class PlayerManager
    {
        private List<NetworkPlayer> _players;
        public Player _player;
        ItemManager _itemManager;
        CollectionManager _collectionManager;
        InventoryManager _inventoryManager;
        UIManager _UImanager;
        public PlayerManager(List<NetworkPlayer> players, CollectionManager collectionManager)
        {
            _players = players;
            _collectionManager = collectionManager;
            
        }
        public NetworkPlayer AddnetworkPlayer(int playerNum)
        {
            NetworkPlayer networkPlayer = new NetworkPlayer(Vector2.Zero,CollectionManager.GetAnimationManagerCopy(2,1.5f), 100, playerNum, CollectionManager.GetGunCopy(3,false,false, null));
            _players.Add(networkPlayer);
            return networkPlayer;
        }
        public Player AddPlayer(ItemManager itemManager, InventoryManager inventoryManager, GraphicsDevice graphicsDevice, UIManager uIManager)
        {
            _UImanager = uIManager;
            _itemManager = itemManager;
            _inventoryManager = inventoryManager;
            Input input = new Input(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space);
            Vector2 position = new Vector2(graphicsDevice.Viewport.Width/2 + -300, graphicsDevice.Viewport.Height / 2 +200);
            int animationNum = 3;
            _player = new Player(GraphicManager.GetAnimationManager_spriteMovement(animationNum, 1.5f), animationNum, position, input, 100,this,_itemManager,_inventoryManager,_UImanager);
            _player.EquipGun(CollectionManager.GetGunCopy(1,false,true,_player._inventoryManager));
            //_player.EquipMeleeWeapon(CollectionManager.GetMeleeWeaponCopy(1,false,true,_inventoryManager));
            return _player;
        }
        public void Update(GameTime gameTime)
        {
            if(_player!=null)
                _player.Update(gameTime);
            foreach (var player in _players)
                    player.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _player.Draw(spriteBatch);
            foreach (var player in _players)
            {
                if (player._playerNum != _player._playerNum)
                    player.Draw(spriteBatch);
            }
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
                    if (Vector2.Distance(position, player._position) < closest_object_distance)
                    {
                        closest_object_distance = Vector2.Distance(position, player._position);
                        player_position = player._position;
                    }
                }
            if (_player != null)
            {
                if (Vector2.Distance(position, _player.Position_Feet) < closest_object_distance)
                {
                    closest_object_distance = Vector2.Distance(position, _player.Position_Feet);
                    player_position = _player.Position_Feet;
                }
                if (Vector2.Distance(position, _player.Position_Feet) < closest_object_distance)
                {
                    closest_object_distance = Vector2.Distance(position, _player._position);
                    player_position = _player.Position_Feet;
                }
            }
            return player_position;
        }
        
    }
}
