using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace GameServer
{
    public class CollectionManager
    {
        List<Simple_Enemy> _enemies;
        ContentManager _contentManager;
        static List<Gun> _guns;
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
        public CollectionManager(List<Simple_Enemy> enemies, ContentManager contentManager)
        {
            _contentManager = contentManager;
            _enemies = enemies;
        }
        public void Initialize(PlayerManager playerManager, ItemManager itemManager)
        {
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
                Vector2.Zero, 0.5f, 1));
            _meleeWeapons.Add(new MeleeWeapon(id++, GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("Weapons/swords-sheet"), 1, 13, 0, 4),
                Vector2.Zero, 0.5f, 1));
        }
        private void InitializeSimpleEnemies()
        {
            int id = 0;
            _simple_enemies = new List<Simple_Enemy>();
            
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(1, 1.5f), id++, Vector2.Zero, 1f, _playerManager, _itemManager, 15, allConsumables, null,null,null));//skeleton
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(8, 1.5f), id++, Vector2.Zero, 3, _playerManager, _itemManager, 10, allWeapons, null, null,null));//runner
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(10, 1.5f), id++, Vector2.Zero, 3, _playerManager, _itemManager, 10, allWeapons, null, null, null));//mage
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(21, 1.5f), id++, Vector2.Zero, 1, _playerManager, _itemManager, 10, allWeapons, null, null, null));//sniper
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(7, 1.5f), id++, Vector2.Zero, 1, _playerManager, _itemManager, 10, allWeapons, null, null, null));//machine-gun


        }
        private void InitializeItems()
        {
            int id = 0;
            _items = new List<Item>();
            _items.Add(new Item(id++, "Ore", 0.02f, 1));//0 - 4 consumables 
            _items.Add(new Item(id++, "Wood", 0.02f, 1));
            _items.Add(new Item(id++, "Apple", 0.01f, 1));
            _items.Add(new Item(id++, "Bone", 0.08f, 1));
            _items.Add(new Item(id++, "Potion", 0.04f, 1));
            _items.Add(new Item(id++, "M16", 0.1f, 1));
            _items.Add(new Item(id++, "Sniper", 0.1f, 1));
            _items.Add(new Item(id++, "Rifle", 0.1f, 1));
            _items.Add(new Item(id++, "MachineGun", 0.1f, 1));
            _items.Add(new Item(id++, "Uzi", 0.1f,1));
            _items.Add(new Item(id++, "Gold", 0.01f, 1));

        }

        static public Gun GetGunCopy(int id, float scale,bool hitPlayers)
        {
            if(id == -1)
            {
                Random x = new Random();
                id = x.Next(0, 5);
            }
            return _guns[id].Copy(scale, hitPlayers);
        }
        static public MeleeWeapon GetMeleeWeaponCopy(int id, float scale)
        {
            if (id == -1)
            {
                Random x = new Random();
                id = x.Next(0, 1);
            }
            return _meleeWeapons[id].Copy(scale);
        }
        public Simple_Enemy GetSimpleEnemyCopyWithGun(int id, float scale)
        {
            return _simple_enemies[id].Copy(scale,GetGunCopy(-1, scale, true),null);
        }
        public Simple_Enemy GetSimpleEnemyCopyWithWeapon(int id, float scale)
        {
            return _simple_enemies[id].Copy(scale, null, GetMeleeWeaponCopy(-1, 0.7f));
        }
        public Item GetItem(int id)
        {
            return _items[id];
        }
        static public AnimationManager GetAnimationManagerCopy(int id,float scale)
        {
            return _playerAnimationManager[id - 1].Copy(scale);
        }
    }
}
