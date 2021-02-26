using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace GameClient
{
    public class CollectionManager
    {
        List<Simple_Enemy> _enemies;
        ContentManager _contentManager;
        private InventoryManager _inventoryManager;
        public static List<Gun> _guns;
        static List<MeleeWeapon> _meleeWeapons;
        static List<Bullet> _bullets;
        static List<Item> _items;
        static List<Simple_Enemy> _simple_enemies;
        public static List<AnimationManager> _playerAnimationManager;
        PlayerManager _playerManager;
        ItemManager _itemManager;

        public static int[] allItems = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public static int[] allWeapons = new int[] { 5, 6, 7, 8, 9 };
        public static int[] allConsumables = new int[] { 2, 4 };
        public CollectionManager()
        {
            
        }
        public void Initialize(List<Simple_Enemy> enemies, ContentManager contentManager ,PlayerManager playerManager, ItemManager itemManager)
        {
            _enemies = enemies;
            _contentManager = contentManager;
            _playerManager = playerManager;
            _itemManager = itemManager;
            InitializeBullets();
            InitializeGuns();
            InitializeMeleeWeapons();
            InitializeItems();
            InitializeSimpleEnemies();
            InitializePlayerTextures();
        }
        private void InitializeMeleeWeapons()
        {
            int id = 0;
            _meleeWeapons = new List<MeleeWeapon>();
            _meleeWeapons.Add(new MeleeWeapon(id++, GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("Weapons/swords-sheet"), 1, 13, 0, 6),
                Vector2.Zero, 0.5f, 1,false,false,null));
            _meleeWeapons.Add(new MeleeWeapon(id++, GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("Weapons/swords-sheet"), 1, 13, 0, 4),
                Vector2.Zero, 0.5f, 1, false, false, null));
        }
        private void InitializeSimpleEnemies()
        {
            int id = 0;
            _simple_enemies = new List<Simple_Enemy>();

            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(1, 1.5f), id++, Vector2.Zero, 1f, _playerManager,
                _itemManager, 15, allConsumables, null, _guns[0], null,null));//skeleton GID=137
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(8, 1.5f), id++, Vector2.Zero, 8, _playerManager,
                _itemManager, 10, allWeapons, GetMeleeWeaponCopy(0,true,true,null), null, null, null));//runner GID=138
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(10, 1.5f), id++, Vector2.Zero, 3, _playerManager,
                _itemManager, 10, allWeapons, null, _guns[2], null, null));//mage GID=139
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(21, 1.5f), id++, Vector2.Zero, 1, _playerManager,
                _itemManager, 10, allWeapons, null, _guns[1], null, null));//sniper GID=140
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(7, 1.5f), id++, Vector2.Zero, 1, _playerManager,
                _itemManager, 10, allWeapons, null, _guns[3], null, null));//machine-gun GID=141


        }
        private void InitializeItems()
        {
            int id = 0;
            _items = new List<Item>();
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 4),//0 - 4 consumables
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 4),
                id++, "Ore", 0.02f, 1, false, false, false, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 0),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 0)
                , id++, "Wood", 0.02f, 1, false, false, false, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 6, 2),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 6, 2),
                id++, "Apple", 0.01f, 1, false, false, false, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 3),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 3),
                id++, "Bone", 0.08f, 1, false, false, false, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 10, 1),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 10, 1),
                id++, "Potion", 0.04f, 1, false, false, false, null, 10));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/1"), null,//5 - 9 weapons
                id++, "M16", 0.1f, 1, false, false, false, _guns[0], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/2"), null,
                id++, "Sniper", 0.1f, 1, false, false, false, _guns[1], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/3"), null,
                id++, "Rifle", 0.1f, 1, false, false, false, _guns[2], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/4"), null,
                id++, "MachineGun", 0.1f, 1, false, false, false, _guns[3], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/5"), null,
                id++, "Uzi", 0.1f, 1, false, false, false, _guns[4], 1));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/Dungeon_Tileset"), 10, 10, 8, 6), null,//10,11 gold,key
                id++, "Gold", 0.01f, 1, false, false, false, null, 1000));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/Dungeon_Tileset"), 10, 10, 9, 9), null,
                id++, "Key", 0, 1, false, false, false, null, 1000));

        }
        private void InitializeBullets()
        {
            int id = 0;
            _bullets = new List<Bullet>();
            Texture2D _bullet_texture = _contentManager.Load<Texture2D>("etc/bullet");
            _bullets.Add(new Bullet(id++, _bullet_texture, 15, 0.1f, 2, 350));
            _bullets.Add(new Bullet(id++, _bullet_texture, 25, 0.3f, 10, 2000));
            _bullets.Add(new Bullet(id++, _bullet_texture, 15, 0.2f, 5, 350));
            _bullets.Add(new Bullet(id++, _bullet_texture, 15, 0.04f, 5, 350));
            _bullets.Add(new Bullet(id++, _bullet_texture, 15, 0.001f, 1, 200));

        }
        private void InitializeGuns()
        {
            int id = 0;
            _guns = new List<Gun>();
            Gun M16 = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/1"), new Vector2(0, 0), _enemies, _bullets[0], false, 0.1f, true, true);
            Gun Sniper = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/2"), new Vector2(0, 0), _enemies, _bullets[1], true, 0, true, true);
            Gun Rifle = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/3"), new Vector2(0, 0), _enemies, _bullets[2], false, 0, true, true);
            Gun MachineGun = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/4"), new Vector2(0, 0), _enemies, _bullets[3], false, 0.5f, true, true);
            Gun Uzi = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/5"), new Vector2(0, 0), _enemies, _bullets[4], false, 0.2f, true, true);
            _guns.Add(M16);
            _guns.Add(Sniper);
            _guns.Add(Rifle);
            _guns.Add(MachineGun);
            _guns.Add(Uzi);
        }
        private void InitializePlayerTextures()
        {
            _playerAnimationManager = new List<AnimationManager>();
            for (int i = 1; i < 25; i++)
            {
                _playerAnimationManager.Add(GraphicManager.GetAnimationManager_spriteMovement(i, 1.5f));
            }
        }
        static public Gun GetGunCopy(int id, bool hitPlayers, bool dealDmg,InventoryManager inventoryManager)
        {
            if (id == -1)
            {
                Random x = new Random();
                id = x.Next(0, 5);
            }
            return _guns[id].Copy(hitPlayers, dealDmg, inventoryManager);
        }
        static public MeleeWeapon GetMeleeWeaponCopy(int id,bool hitPlayers,bool dealDmg,InventoryManager inventoryManager)
        {
            if (id == -1)
            {
                Random x = new Random();
                id = x.Next(0, 1);
            }
            return _meleeWeapons[id].Copy(hitPlayers, dealDmg, inventoryManager);
        }
        public Simple_Enemy GetSimpleEnemyCopy(int id,bool useAstar,bool waitForDestroyedWall)
        {
            return _simple_enemies[id].Copy(useAstar, waitForDestroyedWall);
        }
        public Item GetItem(int id)
        {
            return _items[id];
        }
        static public AnimationManager GetAnimationManagerCopy(int id, float scale)
        {
            return _playerAnimationManager[id - 1].Copy(scale);
        }
    }
}
