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
        public static string[,] _messagesArray;
        public static string[,] _messagesArrayGamePad;
        public static int[,] _chestsArray;
        static List<MeleeWeapon> _meleeWeapons;
        static List<Bullet> _bullets;
        static List<Item> _items;
        static List<SimpleEnemy> _simple_enemies;
        public static List<AnimationManager> _playerAnimationManager;
        static List<AnimationManager> _gunAnimations;
        static List<Texture2D> _gunSprites;
        PlayerManager _playerManager;
        ItemManager _itemManager;
        //public static int[] allItems = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public static int[] allWeapons = new int[] { 7, 8, 9,10,11,12,13,14,15,16 };
        public static int[] allConsumables = new int[] { 2, 4 };
        public static int[] basicDrops = new int[] { 2, 5 };
        public static int[] keyDrop = new int[] { 6 };
        public CollectionManager()
        {
        }
        public void Initialize(List<SimpleEnemy> enemies, ContentManager contentManager, PlayerManager playerManager, ItemManager itemManager)
        {
            _enemies = enemies;
            _contentManager = contentManager;
            _playerManager = playerManager;
            _itemManager = itemManager;
            _messagesArray = new string[30, 10];
            _messagesArrayGamePad = new string[30, 10];
            _chestsArray = new int[30, 10];
            InitializeBullets();
            InitializeGuns();
            InitializeMeleeWeapons();
            InitializeItems();
            InitializeSimpleEnemies();
            InitializePlayerTextures();
            InitializeMessagesArray();
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
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(1, 1.7f), id++, Vector2.Zero, 1f, _playerManager,
                _itemManager, 30, basicDrops, null, _guns[1]));//M16 GID=137
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(8, 1.2f), id++, Vector2.Zero, 8, _playerManager,
                _itemManager, 10, basicDrops, GetMeleeWeaponCopy(0, true, true, null), null));//runner KNIFE GID=138
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(10, 1.5f), id++, Vector2.Zero, 1, _playerManager,
                _itemManager, 20, basicDrops, null, _guns[0]));//RIFLE GID=139
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(21, 1.4f), id++, Vector2.Zero, 1, _playerManager,
                _itemManager, 20, basicDrops, null, _guns[2]));//sniper GID=140
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(7, 1.7f), id++, Vector2.Zero, 1, _playerManager,
                _itemManager, 120, basicDrops, null, _guns[3]));//machine-gun GID=141
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(9, 1.5f), id++, Vector2.Zero, 1, _playerManager,
                _itemManager, 20, basicDrops, null, _guns[6]));//UZI GID=142
            _simple_enemies.Add(new SimpleEnemy(GraphicManager.GetAnimationManager_spriteMovement(11, 3), id++, Vector2.Zero, 1, _playerManager,
                _itemManager, 400, keyDrop, null, _guns[2], summonEnemyID: 1));//sniper Boss GID=143
        }
        private void InitializeItems()
        {
            int id = 0;
            _items = new List<Item>();
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 4),//0 - 4 consumables
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 4),
                id++, 1, "Ore", 0.02f, 1, false, false, false, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 0),//1 Wood
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 0)
                , id++, 1, "Wood", 0.02f, 1, false, false, false, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 6, 2),//2 Apple
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 6, 2),
                id++, 1, "Apple", 0.01f, 1, false, false, false, null, 10, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 3),//3 Bone
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 3),
                id++, 1, "Bone", 0.08f, 1, false, false, false, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 10, 1),//4 potion
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 10, 1),
                id++, 1.5f, "Potion", 0.003f, 1, false, false, false, null, 10, 50));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/Dungeon_Tileset"), 10, 10, 8, 6), null,//5 gold
                id++, 1.5f, "Gold", 0.02f, 1, false, false, false, null, 1000));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/Dungeon_Tileset"), 10, 10, 9, 9), null,//6 key
                id++, 2f, "Key", 1, 1, false, false, false, null, 1000));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/sprites/1"), null,//7-16 guns
                id++, 0.7f, "DesertEagle", 0.1f, 1, false, false, false, _guns[0], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/sprites/2"), null,//8
                id++, 0.7f, "M16", 0.01f, 1, false, false, false, _guns[1], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/sprites/3"), null,
                id++, 0.7f, "Sniper", 0.03f, 1, false, false, false, _guns[2], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/sprites/4"), null,//10
                id++, 0.7f, "Famas", 0.005f, 1, false, false, false, _guns[3], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/sprites/5"), null,
                id++, 0.7f, "P90", 0.05f, 1, false, false, false, _guns[4], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/sprites/6"), null,//12
                id++, 0.7f, "417", 0.05f, 1, false, false, false, _guns[5], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/sprites/7"), null,
                id++, 0.7f, "Uzi", 0.05f, 1, false, false, false, _guns[6], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/sprites/8"), null,//14
                id++, 0.7f, "P12", 0.05f, 1, false, false, false, _guns[7], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/sprites/9"), null,
                id++, 0.7f, "FlareGun", 0.05f, 1, false, false, false, _guns[8], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/sprites/10"), null,//16
                id++, 0.7f, "AK47", 0.05f, 1, false, false, false, _guns[9], 1));

        }
        private void InitializeBullets()
        {
            int id = 0;
            _bullets = new List<Bullet>();
            Texture2D _bullet_texture = _contentManager.Load<Texture2D>("etc/bullet");
            _bullets.Add(new Bullet(id++, _bullet_texture,isSniper:false, speed:20, shootingTimer:0.4f, dmg:5, travelDistance:700,spread:0));//Desert-eagle
            _bullets.Add(new Bullet(id++, _bullet_texture,false, 20, 0.08f, 2, 700,0.2f));//M16
            _bullets.Add(new Bullet(id++, _bullet_texture,true, 25, 0.45f, 40, 2000,0));//Sniper
            _bullets.Add(new Bullet(id++, _bullet_texture,false, 20, 0.13f, 4, 700,0.3f));//Famas
            _bullets.Add(new Bullet(id++, _bullet_texture, false, 20, 0.2f, 3, 700,0.2f));//P90
            _bullets.Add(new Bullet(id++, _bullet_texture, false, 20, 0.15f, 1, 700,0.2f));//gun417
            _bullets.Add(new Bullet(id++, _bullet_texture, false, 20, 0.02f, 1, 700,0.55f));//Uzi
            _bullets.Add(new Bullet(id++, _bullet_texture, false, 20, 0.4f, 10, 700,0.2f));//P12
            _bullets.Add(new Bullet(id++, _bullet_texture, false, 10, 2, 100, 1000,0));//FlareGun
            _bullets.Add(new Bullet(id++, _bullet_texture, false, 20, 0.075f, 3, 700,0.3f));//AK47
        }
        private void InitializeGuns()
        {
            int id = 0;
            _guns = new List<Gun>();
            _gunAnimations = new List<AnimationManager>();
            _gunAnimations.Add(GraphicManager.GetAnimationManager_Gun(1, 1, 2, 0.2f));
            _gunAnimations.Add(GraphicManager.GetAnimationManager_Gun(2, 2, 7, 0.05f, 8, lastIndex:9));
            _gunAnimations.Add(GraphicManager.GetAnimationManager_Gun(3, 3, 10,0.1f,24,29));
            _gunAnimations.Add(GraphicManager.GetAnimationManager_Gun(4, 2, 5));
            _gunAnimations.Add(GraphicManager.GetAnimationManager_Gun(5, 2, 6));
            _gunAnimations.Add(GraphicManager.GetAnimationManager_Gun(6, 1, 4));
            _gunAnimations.Add(GraphicManager.GetAnimationManager_Gun(7, 1, 12));
            _gunAnimations.Add(GraphicManager.GetAnimationManager_Gun(8, 1, 8));
            _gunAnimations.Add(GraphicManager.GetAnimationManager_Gun(9, 1, 20));
            _gunAnimations.Add(GraphicManager.GetAnimationManager_Gun(10, 1, 20));

            _gunSprites = new List<Texture2D>();
            for (int i = 1; i < 11; i++)
            {
                _gunSprites.Add(_contentManager.Load<Texture2D>("Weapons/sprites/"+i));
            }
            for (int i = 0; i < 10; i++)
            {
                _guns.Add(new Gun(id++, _gunSprites[i], _gunAnimations[i], new Vector2(0, 0), _enemies, _bullets[i], true, true));
            }
            //Gun DesertEagle = new Gun(id++, _gunSprites[0], _gunAnimations[0], new Vector2(0, 0), _enemies, _bullets[0], true, true);
            //Gun M16 = new Gun(id++, _gunSprites[1], _gunAnimations[1], new Vector2(0, 0), _enemies, _bullets[1], true, true);
            //Gun Sniper = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/sprites/3"), _gunAnimations[2], new Vector2(0, 0), _enemies, _bullets[2], true, true);
            //Gun Famas = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/sprites/4"), _gunAnimations[3], new Vector2(0, 0), _enemies, _bullets[3], true, true);
            //Gun P90 = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/sprites/5"), _gunAnimations[4], new Vector2(0, 0), _enemies, _bullets[4], true, true);
            //Gun gun417 = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/sprites/6"), _gunAnimations[5], new Vector2(0, 0), _enemies, _bullets[5], true, true);
            //Gun Uzi = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/sprites/7"), _gunAnimations[6], new Vector2(0, 0), _enemies, _bullets[6], true, true);
            //Gun P12 = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/sprites/8"), _gunAnimations[7], new Vector2(0, 0), _enemies, _bullets[7], true, true);
            //Gun FlareGun = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/sprites/9"), _gunAnimations[8], new Vector2(0, 0), _enemies, _bullets[8], true, true);
            //Gun AK47 = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/sprites/10"), _gunAnimations[9], new Vector2(0, 0), _enemies, _bullets[9], true, true);

            //_guns.Add(DesertEagle);
            //_guns.Add(M16);
            //_guns.Add(Sniper);
            //_guns.Add(Famas);
            //_guns.Add(P90);
            //_guns.Add(gun417);
            //_guns.Add(Uzi);
            //_guns.Add(P12);
            //_guns.Add(FlareGun);
            //_guns.Add(AK47);

        }
        private void InitializeMessagesArray()
        {
            _messagesArray[1, 0] = "Welcome to UNBOXINGRAVE\nUse 'W' 'A' 'S' 'D' keys to move around";
            _messagesArray[1, 1] = "Press left mouse button to shot";
            _messagesArray[1, 2] = "Press right mouse button to melee attack";
            _messagesArray[1, 3] = "Press spacebar to pick items near you";
            _messagesArray[1, 4] = "Your inventory is at the bottom of the screen,\nclick on inventory items to use them";
            _messagesArray[1, 5] = "Move to the right side of the map to progress";
            _messagesArrayGamePad[1, 0] = "Welcome to UNBOXINGRAVE\nUse the left joystick to move around";
            _messagesArrayGamePad[1, 1] = "Press R2 to shot, Use the right jotstick to aim";
            _messagesArrayGamePad[1, 2] = "Press square to use melee attack";
            _messagesArrayGamePad[1, 3] = "Press O to pick items near you";
            _messagesArrayGamePad[1, 4] = "Your inventory is at the bottom of the screen,\npress R1 / L1 to choose an item and press X to use it";
            _messagesArray[2, 0] = "Use melee attack or shooting to destroy the boxes";
            _messagesArray[2, 1] = "Boxes may drop items for you to use";
            _messagesArray[2, 2] = "Pick up the key using spacebar";
            _messagesArray[2, 3] = "Open the door with melee attack";
            _messagesArray[2, 4] = "Good job";
            _messagesArray[3, 0] = "WARNING!!\nEnemies attack when they can reach you";
            _messagesArray[3, 1] = "Once your destroy the boxes prepare to fight!";
            _messagesArray[3, 2] = "Well done";
            _messagesArray[4, 0] = "Warning! graves spawn enemies when\nyou are getting closer to them!";
            _messagesArray[4, 1] = "Use melee attack to destroy the boxes faster";
            _messagesArray[4, 2] = "Click on an item from the inventory to use";
            _messagesArray[4, 3] = "Watch out from those boxes.. something could be behind them";
            _messagesArrayGamePad[4, 2] = "To use an item\npress R1 and L1 to select it,\npress X to use it";
            _messagesArray[5, 0] = "You need a key to open the door..\nI wonder where you could find one";
            _messagesArray[5, 1] = "Hit the chest with melee attack to open it";
            _messagesArray[6, 0] = "Beware of the sniper";
            _messagesArray[7, 0] = "This guys looks scary.. watch out";
            _messagesArray[8, 0] = "It's christmas! your present is in the chest!";
            _messagesArray[8, 1] = "To equip a weapon click on it on the inventory";
            _messagesArray[8, 2] = "This weapon is basically cheating, use it wisely";
            _messagesArrayGamePad[8, 1] = "To equip a weapon press R1 and L1 to select it,\npress X to equip it";
            _messagesArray[9, 0] = "Those enemies are coming fast, be ready.";
            _messagesArray[10, 0] = "It's better to avoid some fights";
            _messagesArray[10, 1] = "RUN!!";
            _messagesArray[10, 2] = "Slow down";
            _messagesArray[10, 3] = "Just kidding, run faster";
            _messagesArray[10, 4] = "WHO IS SPEED?";
            _messagesArray[10, 5] = "YOU ARE SPEED!!!";
            _messagesArray[11, 0] = "It's christmas again?\nyour present is in the chest!";
            _messagesArray[11, 1] = "Spread the love";
            _messagesArray[12, 0] = "I shot a man with a paintball gun...\njust to watch him dye.";
            _messagesArray[16, 0] = "You were made for this!";
        }
        private void InitializeChestArray()
        {
            _chestsArray[8, 0] = 9;
            _chestsArray[11, 0] = 13;
            _chestsArray[16, 0] = 8;
            int x = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    _chestsArray[23, x++] = 5 + j;
                }
            }
        }
        private void InitializePlayerTextures()
        {
            _playerAnimationManager = new List<AnimationManager>();
            for (int i = 1; i < 25; i++)
            {
                _playerAnimationManager.Add(GraphicManager.GetAnimationManager_spriteMovement(i, 1.5f));
            }
        }
        static public Gun GetGunCopy(int id, bool hitPlayers, bool dealDmg, Inventory inventoryManager)
        {
            if (id == -1)
            {
                Random x = new Random();
                id = x.Next(0, 5);
            }
            try
            {
                return _guns[id].Copy(hitPlayers, dealDmg, inventoryManager);
            }
            catch
            {
                return null;
            }
        }
        static public MeleeWeapon GetMeleeWeaponCopy(int id, bool hitPlayers, bool dealDmg, Inventory inventoryManager)
        {
            if (id == -1)
            {
                Random x = new Random();
                id = x.Next(0, 1);
            }
            try
            {
                return _meleeWeapons[id].Copy(hitPlayers, dealDmg, inventoryManager);
            }
            catch
            {
                return null;
            }
        }
        public SimpleEnemy GetSimpleEnemyCopy(int id, bool useAstar, bool waitForDestroyedWall)
        {
            try
            {
                return _simple_enemies[id].Copy(useAstar, waitForDestroyedWall);
            }
            catch
            {
                return null;
            }
        }
        public Item GetItem(int id, bool alwaysDrop = true)
        {
            try
            {
                return _items[id].DropAndCopy(alwaysDrop);
            }
            catch
            {
                return null;
            }
        }
        static public AnimationManager GetAnimationManagerCopy(int id, float scale)
        {
            try
            {
                return _playerAnimationManager[id - 1].Copy();
            }
            catch
            {
                return null;
            }
        }
        static public Tuple<string, string> GetMessageFromMessageArray(int level, int messageNum)
        {
            try
            {
                return new Tuple<string, string>(_messagesArray[level, messageNum], _messagesArrayGamePad[level, messageNum]);
            }
            catch
            {
                return null;
            }
        }
        static public int GetItemIDFromChestArray(int level, int chestNum)
        {
            try
            {
                return _chestsArray[level, chestNum];
            }
            catch
            {
                return 0;
            }
        }
    }
}