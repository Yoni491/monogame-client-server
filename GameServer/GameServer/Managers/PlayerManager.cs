using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace GameServer
{
    public class PlayerManager
    {
        private List<NetworkPlayer> _NetworkPlayers;
        public NetworkPlayer _player;
        ItemManager _itemManager;
        CollectionManager _collectionManager;
        public PlayerManager(List<NetworkPlayer> players, CollectionManager collectionManager)
        {
            _NetworkPlayers = players;
            _collectionManager = collectionManager;
            
        }
        //public NetworkPlayer AddnetworkPlayer(int playerNum)
        //{
        //    NetworkPlayer networkPlayer = new NetworkPlayer(Vector2.Zero, 100, playerNum, CollectionManager.GetGunCopy(3,0.7f,false));
        //    _players.Add(networkPlayer);
        //    NetworkManagerClient._updatenetworkPlayerTexture = true;
        //    return networkPlayer;
        //}
        //public Player AddPlayer(ItemManager itemManager, InventoryManager inventoryManager, GraphicsDevice graphicsDevice, UIManager uIManager)
        //{
        //    _UImanager = uIManager;
        //    _itemManager = itemManager;
        //    _inventoryManager = inventoryManager;
        //    Input input = new Input(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space);
        //    Vector2 position = new Vector2(graphicsDevice.Viewport.Width/2 + -300, graphicsDevice.Viewport.Height / 2 +200);
        //    int animationNum = 3;
        //    _player = new Player(GraphicManager.GetAnimationManager_spriteMovement(animationNum, 1.5f), animationNum, position, input, 100,this,_itemManager,_inventoryManager,_UImanager);
        //    _player.EquipGun(CollectionManager.GetGunCopy(1,0.7f,false));
        //    _player.EquipMeleeWeapon(CollectionManager.GetMeleeWeaponCopy(1,0.7f));
        //    return _player;
        //}
        //public void Update(GameTime gameTime, List<Simple_Enemy> enemies)
        //{
        //    _player.Update(gameTime);
        //    foreach (var player in _players)
        //        player.Update(gameTime, enemies);
        //}
        public Vector2 getClosestPlayerToPosition(Vector2 position)
        {
            float closest_object_distance = float.MaxValue;
            Vector2 player_position = position;
            if (_NetworkPlayers != null)
                foreach (var player in _NetworkPlayers)
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
