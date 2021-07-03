using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace GameClient
{
    public class CollectionManager
    {
        List<SimpleEnemy> _enemies;
        ContentManager _contentManager;
        public static List<Gun> _guns;
        public static string[,] _massagesArray;
        public static string[,] _massagesArrayGamePad;
        public static int[,] _chestsArray;
        static List<MeleeWeapon> _meleeWeapons;
        static List<Bullet> _bullets;
        static List<Item> _items;
        static List<SimpleEnemy> _simple_enemies;
        public static List<AnimationManager> _playerAnimationManager;
        PlayerManager _playerManager;
        ItemManager _itemManager;

        //public static int[] allItems = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public static int[] allWeapons = new int[] { 5, 6, 7, 8, 9 };
        public static int[] allConsumables = new int[] { 2, 4 };
        public static int[] basicDrops = new int[] { 2, 10 };
        public static int[] keyDrop = new int[] { 11 };
        public CollectionManager()
        {

        }
        public void Initialize(List<SimpleEnemy> enemies, ContentManager contentManager, PlayerManager playerManager, ItemManager itemManager)
        {
            _enemies = enemies;
            _contentManager = contentManager;
            _playerManager = playerManager;
            _itemManager = itemManager;
            _massagesArray = new string[30, 10];
            _massagesArrayGamePad = new string[30, 10];
            _chestsArray = new int[30, 10];
            InitializeBullets();
            InitializeGuns();
            InitializeMeleeWeapons();
            InitializeItems();
            InitializeSimpleEnemies();
            InitializePlayerTextures();
            InitializeMassagesArray();
            InitializeChestArray();
        }
        private void InitializeMeleeWeapons()
        {
            int id = 0;
            _meleeWeapons = new List<MeleeWeapon>();
            _meleeWeapons.Add(new MeleeWeapon(id++, GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("Weapons/swords-sheet"), 1, 13, 0, 6),
                Vector2.Zero, 0.5f, 1, false, false, null));
            _meleeWeapons.Add(new MeleeWeapon(id++, GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("Weapons/swords-sheet"), 1, 13, 0, 4),
                Vector2.Zero, 0.5f, 1, false, false, null));
        }
        private void InitializeSimpleEnemies()
        {
            int id = 0;
            _simple_enemies = new List<SimpleEnemy>();

            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(1, 1.5f), id++, Vector2.Zero, 1f, _playerManager,
                _itemManager, 30, basicDrops, null, _guns[0]));//M16 GID=137
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(8, 1.5f), id++, Vector2.Zero, 8, _playerManager,
                _itemManager, 10, basicDrops, GetMeleeWeaponCopy(0, true, true, null), null));//runner KNIFE GID=138
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(10, 1.5f), id++, Vector2.Zero, 1, _playerManager,
                _itemManager, 20, basicDrops, null, _guns[2]));//RIFLE GID=139
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(21, 1.5f), id++, Vector2.Zero, 1, _playerManager,
                _itemManager, 20, basicDrops, null, _guns[1]));//sniper GID=140
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(7, 1.5f), id++, Vector2.Zero, 1, _playerManager,
                _itemManager, 20, basicDrops, null, _guns[3]));//machine-gun GID=141
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(9, 1.5f), id++, Vector2.Zero, 1, _playerManager,
                _itemManager, 20, basicDrops, null, _guns[4]));//UZI GID=142
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(21, 3), id++, Vector2.Zero, 1, _playerManager,
                _itemManager, 400, keyDrop, null, _guns[1], summonEnemyID: 1));//sniper Boss GID=143


        }
        private void InitializeItems()
        {
            int id = 0;
            _items = new List<Item>();
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 4),//0 - 4 consumables
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 4),
                id++, 1, "Ore", 0.02f, 1, false, false, false, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 0),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 0)
                , id++, 1, "Wood", 0.02f, 1, false, false, false, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 6, 2),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 6, 2),
                id++, 1, "Apple", 0.01f, 1, false, false, false, null, 10, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 3),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 3),
                id++, 1, "Bone", 0.08f, 1, false, false, false, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 10, 1),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 10, 1),
                id++, 1.5f, "Potion", 0.003f, 1, false, false, false, null, 10, 50));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/1"), null,//5 - 9 weapons
                id++, 0.7f, "M16", 0.01f, 1, false, false, false, _guns[0], 1));//rating 4
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/2"), null,
                id++, 0.7f, "Sniper", 0.03f, 1, false, false, false, _guns[1], 1));//rating 3
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/3"), null,
                id++, 0.7f, "Rifle", 0.1f, 1, false, false, false, _guns[2], 1));//rating 1
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/4"), null,
                id++, 0.7f, "MachineGun", 0.005f, 1, false, false, false, _guns[3], 1));//rating 5
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/5"), null,
                id++, 0.7f, "Uzi", 0.05f, 1, false, false, false, _guns[4], 1));//rating 2
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/Dungeon_Tileset"), 10, 10, 8, 6), null,//10,11 gold,key
                id++, 1.5f, "Gold", 0.02f, 1, false, false, false, null, 1000));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/Dungeon_Tileset"), 10, 10, 9, 9), null,
                id++, 2f, "Key", 1, 1, false, false, false, null, 1000));

        }
        private void InitializeBullets()
        {
            int id = 0;
            _bullets = new List<Bullet>();
            Texture2D _bullet_texture = _contentManager.Load<Texture2D>("etc/bullet");
            _bullets.Add(new Bullet(id++, _bullet_texture, 20, 0.08f, 2, 500));//M16
            _bullets.Add(new Bullet(id++, _bullet_texture, 25, 0.45f, 40, 2000));//Sniper
            _bullets.Add(new Bullet(id++, _bullet_texture, 20, 0.2f, 5, 600));//Rifle
            _bullets.Add(new Bullet(id++, _bullet_texture, 20, 0.02f, 4, 400));//MachineGun
            _bullets.Add(new Bullet(id++, _bullet_texture, 20, 0.015f, 1, 300));//Uzi

        }
        private void InitializeGuns()
        {
            int id = 0;
            _guns = new List<Gun>();
            Gun M16 = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/1"), new Vector2(0, 0), _enemies, _bullets[0], false, 0.1f, true, true);
            Gun Sniper = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/2"), new Vector2(0, 0), _enemies, _bullets[1], true, 0, true, true);
            Gun Rifle = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/3"), new Vector2(0, 0), _enemies, _bullets[2], false, 0, true, true);
            Gun MachineGun = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/4"), new Vector2(0, 0), _enemies, _bullets[3], false, 0.4f, true, true);
            Gun Uzi = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/5"), new Vector2(0, 0), _enemies, _bullets[4], false, 0.55f, true, true);
            _guns.Add(M16);
            _guns.Add(Sniper);
            _guns.Add(Rifle);
            _guns.Add(MachineGun);
            _guns.Add(Uzi);
        }
        private void InitializeMassagesArray()
        {
            _massagesArray[1, 0] = "Welcome to UNBOXINGRAVE\nUse 'W' 'A' 'S' 'D' keys to move around";
            _massagesArray[1, 1] = "Press left mouse button to shot";
            _massagesArray[1, 2] = "Press right mouse button to melee attack";
            _massagesArray[1, 3] = "Press spacebar to pick items near you";
            _massagesArray[1, 4] = "Your inventory is at the bottom of the screen, click on inventory items to use them";
            _massagesArray[1, 5] = "Move to the right side of the map to progress";

            _massagesArrayGamePad[1, 0] = "Welcome to UNBOXINGRAVE\nUse the left joystick to move around";
            _massagesArrayGamePad[1, 1] = "Press R2 to shot, Use the right jotstick to aim";
            _massagesArrayGamePad[1, 2] = "Press square to melee attack";
            _massagesArrayGamePad[1, 3] = "Press O to pick items near you";
            _massagesArrayGamePad[1, 4] = "Your inventory is at the bottom of the screen, press R1 / L1 to choose an item and press X to use it";

            _massagesArray[2, 0] = "use melee attack or shooting to destroy the boxes";
            _massagesArray[2, 1] = "boxes may drop items for you to use";
            _massagesArray[2, 2] = "pick up the key using spacebar";
            _massagesArray[2, 3] = "open the door with melee attack";
            _massagesArray[2, 4] = "good job";

            _massagesArray[3, 0] = "WARNING!!\nenemies attack when they can reach you";
            _massagesArray[3, 1] = "once your destroy the boxes prepare to fight!";
            _massagesArray[3, 2] = "well done";

            _massagesArray[4, 0] = "Warning! graves spawn enemies when you are getting closer to them!";
            _massagesArray[4, 1] = "Use melee attack to destroy the boxes faster";
            _massagesArray[4, 2] = "Watch out from those boxes.. something could be behind them";

            _massagesArray[5, 0] = "You need a key to open the door.. I wonder where you could find one";
            _massagesArray[5, 1] = "Hit the chest with melee attack to open it";

            _massagesArray[6, 0] = "Watch out from the sniper";

            _massagesArray[7, 0] = "This guys looks scary.. watch out";

            _massagesArray[8, 0] = "It's christmas! your present is in the chest!";
            _massagesArray[8, 1] = "This weapon is basically cheating, use it wisely";

            _massagesArray[9, 0] = "The sniper is probably not so good here";

            _massagesArray[10, 0] = "It's better to avoid some fights";
            _massagesArray[10, 1] = "RUN!!";
            _massagesArray[10, 2] = "Slow down";
            _massagesArray[10, 3] = "Just kidding, run faster";
            _massagesArray[10, 4] = "WHO IS SPEED?";
            _massagesArray[10, 5] = "YOU ARE SPEED!!!";

            _massagesArray[11, 0] = "It's christmas again? your present is in the chest!";
            _massagesArray[11, 1] = "say this out loud: 'say hello to my little friend'";

            _massagesArray[12, 0] = "if you can't kill them, join them - Isaac Newton";

            _massagesArray[16, 0] = "I think you deserve it";
        }
        private void InitializeChestArray()
        {
            _chestsArray[8, 0] = 6;
            _chestsArray[11, 0] = 9;
            _chestsArray[16, 0] = 5;
        }
        private void InitializePlayerTextures()
        {
            _playerAnimationManager = new List<AnimationManager>();
            for (int i = 1; i < 25; i++)
            {
                _playerAnimationManager.Add(GraphicManager.GetAnimationManager_spriteMovement(i, 1.5f));
            }
        }
        static public Gun GetGunCopy(int id, bool hitPlayers, bool dealDmg, InventoryManager inventoryManager)
        {
            if (id == -1)
            {
                Random x = new Random();
                id = x.Next(0, 5);
            }
            return _guns[id].Copy(hitPlayers, dealDmg, inventoryManager);
        }
        static public MeleeWeapon GetMeleeWeaponCopy(int id, bool hitPlayers, bool dealDmg, InventoryManager inventoryManager)
        {
            if (id == -1)
            {
                Random x = new Random();
                id = x.Next(0, 1);
            }
            return _meleeWeapons[id].Copy(hitPlayers, dealDmg, inventoryManager);
        }
        public SimpleEnemy GetSimpleEnemyCopy(int id, bool useAstar, bool waitForDestroyedWall)
        {
            return _simple_enemies[id].Copy(useAstar, waitForDestroyedWall);
        }
        public Item GetItem(int id)
        {
            return _items[id];
        }
        static public AnimationManager GetAnimationManagerCopy(int id, float scale)
        {
            return _playerAnimationManager[id - 1].Copy();
        }
        static public Tuple<string, string> GetMassageFromMassageArray(int level, int massageNum)
        {

            return new Tuple<string, string>(_massagesArray[level, massageNum], _massagesArrayGamePad[level, massageNum]);
        }
        static public int GetItemIDFromChestArray(int level, int chestNum)
        {
            return _chestsArray[level, chestNum];
        }
    }
}