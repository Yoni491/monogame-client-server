using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
namespace GameClient
{
    public class PlayerManager
    {
        private List<NetworkPlayer> _networkPlayers;
        public List<Player> _players;
        ItemManager _itemManager;
        CollectionManager _collectionManager;
        InventoryManager _inventoryManager;
        SettingsScreen _UImanager;
        int _playerStartingHealth = 200;
        GraphicsDevice _graphicsDevice;
        public PlayerManager(GraphicsDevice graphicsDevice, List<NetworkPlayer> networkPlayers, CollectionManager collectionManager, List<Player> players)
        {
            _graphicsDevice = graphicsDevice;
            _networkPlayers = networkPlayers;
            _collectionManager = collectionManager;
            _players = players;
        }
        public void Initialize(ItemManager itemManager, InventoryManager inventoryManager, SettingsScreen uIManager)
        {
            _UImanager = uIManager;
            _itemManager = itemManager;
            _inventoryManager = inventoryManager;
        }
        public NetworkPlayer AddnetworkPlayer(int playerNum)
        {
            NetworkPlayer networkPlayer = new NetworkPlayer(Vector2.Zero, CollectionManager.GetAnimationManagerCopy(2, 1.5f),
                _playerStartingHealth, playerNum, CollectionManager.GetGunCopy(3, false, false, null), new NameDisplay(_graphicsDevice, ""));
            _networkPlayers.Add(networkPlayer);
            return networkPlayer;
        }
        public Player AddPlayer()
        {
            Vector2 position = Vector2.Zero;
            int animationNum = 3;
            NameDisplay nameDisplay = new NameDisplay(_graphicsDevice, "");
            Player player = new Player(GraphicManager.GetAnimationManager_spriteMovement(animationNum, 1.5f),
                animationNum, position, _playerStartingHealth, this, _itemManager, _inventoryManager.AddInventory(), _UImanager, nameDisplay);
            player._inventory.EquippedGun = _collectionManager.GetItem(7).DropAndCopy(true);
            player.EquipGun(player._inventory.EquippedGun._gun);
            //_player.EquipMeleeWeapon(CollectionManager.GetMeleeWeaponCopy(1,false,true,_inventoryManager));
            _players.Add(player);
            return player;
        }
        public void Reset(bool resetPlayer)
        {
            _networkPlayers.Clear();
            if (_players.Count > 0 && resetPlayer)
                _players.ForEach(player => ResetAndSetPlayer(3, "", player));
        }
        public void ResetAndSetPlayer(int animationNum, string playerName, Player player)
        {
            player._animationManager = CollectionManager.GetAnimationManagerCopy(animationNum, 1.5f);
            player._animationNum = animationNum;
            player._inventory.ResetInventory();
            player._health._health_left = _playerStartingHealth;
            player._health._total_health = _playerStartingHealth;
            player._inventory.EquippedGun = _collectionManager.GetItem(7).DropAndCopy(true);
            player.EquipGun(player._inventory.EquippedGun._gun);
            player._dead = false;
            player._nameDisplay._text = playerName;
        }
        public void AddPlayerFromData(ProgressDataPlayer progressData)
        {
            //_player._animationManager = CollectionManager.GetAnimationManagerCopy(progressData._animationNum, 1.5f);
            //_player._animationNum = progressData._animationNum;
            //_player._health._health_left = progressData._Health._health_left;
            //_player._health._total_health = progressData._Health._total_health;
            //_inventoryManager.EquippedGun = _collectionManager.GetItem(progressData._gunNum + 5).DropAndCopy(true);
            //_player.EquipGun(_inventoryManager.EquippedGun._gun);
            //_player._dead = false;
            //_player._nameDisplay._text = progressData._playerName;
        }
        public void Update(GameTime gameTime)
        {
            _players.ForEach(player =>
            {
                if (player._dead)
                {
                    GameOverScreen._showScreen = true;
                }
                player.Update(gameTime);
            });
            foreach (var player in _networkPlayers)
                player.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _players.ForEach(player => player.Draw(spriteBatch));
            foreach (var player in _networkPlayers)
            {
                if (!IsNetworkPlayerLocalPlayer(player._playerNum))
                    player.Draw(spriteBatch);
            }
        }
        public bool IsNetworkPlayerLocalPlayer(int networkNum)
        {
            bool res = false;
            _players.ForEach(player =>
            {
                if (player._playerNum == networkNum)
                    res = true;
            });
            return res;
        }
        public Vector2 getClosestPlayerToPosition(Vector2 position)
        {
            float closest_object_distance = float.MaxValue;
            Vector2 player_position = position;
            if (_networkPlayers != null && Game_Client._isServer)
            {
                foreach (var player in _networkPlayers)
                {
                    if (player._health._health_left > 0)
                    {
                        if (Vector2.Distance(position, player.Position_Feet) < closest_object_distance)
                        {
                            closest_object_distance = Vector2.Distance(position, player.Position_Feet);
                            player_position = player.Position_Feet;
                        }
                    }
                }
            }
            _players.ForEach(player =>
            {
                if (Vector2.Distance(position, player.Position_Feet) < closest_object_distance)
                {
                    closest_object_distance = Vector2.Distance(position, player.Position_Feet);
                    player_position = player.Position_Feet;
                }
            }
            );
            return player_position;
        }
    }
}
