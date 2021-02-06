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
        List<Gun> _guns;
        List<MeleeWeapon> _meleeWeapons;
        List<Bullet> _bullets;
        List<Item> _items;
        List<Simple_Enemy> _simple_enemies;
        PlayerManager _playerManager;
        ItemManager _itemManager;
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
            int[] allItems = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] allWeapons = new int[] { 5, 6, 7, 8, 9 };
            int[] allConsumables = new int[] { 2, 4 };
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(1, 1.5f), id++, Vector2.Zero, 1f, _playerManager, _itemManager, 10, allConsumables, null,null));
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(24, 1.5f), id++, Vector2.Zero, 1, _playerManager, _itemManager, 10, allWeapons, null, null));
        }
        private void InitializeItems()
        {
            int id = 0;
            _items = new List<Item>();
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 4),//0 - 4 consumables
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 4),
                id++, "Ore", 0.02f, 1, false, false, false, null, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 0),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 0)
                , id++, "Wood", 0.02f, 1, false, false, false, null, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 6, 2),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 6, 2),
                id++, "Apple", 0.01f, 1, false, false, false, null, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 3),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 3),
                id++, "Bone", 0.08f, 1, false, false, false, null, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 10, 1),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 10, 1),
                id++, "Potion", 0.04f, 1, false, false, false, null, null, 10));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/1"),//5 - 9 weapons
                _contentManager.Load<Texture2D>("Weapons/1"),
                id++, "M16", 0.1f, 1, false, false, false, null, _guns[0], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/2"),
                _contentManager.Load<Texture2D>("Weapons/2"),
                id++, "Sniper", 0.1f, 1, false, false, false, null, _guns[1], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/3"),
                _contentManager.Load<Texture2D>("Weapons/3"),
                id++, "Rifle", 0.1f, 1, false, false, false, null, _guns[2], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/4"),
                _contentManager.Load<Texture2D>("Weapons/4"),
                id++, "MachineGun", 0.1f, 1, false, false, false, null, _guns[3], 1));
            _items.Add(new Item(_contentManager.Load<Texture2D>("Weapons/5"),
                _contentManager.Load<Texture2D>("Weapons/5"),
                id++, "Uzi", 0.1f, 1, false, false, false, null, _guns[4], 1));

        }
        private void InitializeBullets()
        {
            int id = 0;
            _bullets = new List<Bullet>();
            Texture2D _bullet_texture = _contentManager.Load<Texture2D>("etc/bullet");
            _bullets.Add(new Bullet(id++, _bullet_texture, 15, 0.1f, 2,350));
            _bullets.Add(new Bullet(id++, _bullet_texture, 25, 0.5f, 10,2000));
            _bullets.Add(new Bullet(id++, _bullet_texture, 15, 0.2f, 5,350));
            _bullets.Add(new Bullet(id++, _bullet_texture, 15, 0.1f, 5,350));
            _bullets.Add(new Bullet(id++, _bullet_texture, 15, 0.05f, 1,200));

        }
        private void InitializeGuns()
        {
            int id = 0;
            _guns = new List<Gun>();
            Gun M16 = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/1"), new Vector2(0, 0), _enemies, _bullets[0], false,0.1f,true);
            Gun Sniper = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/2"), new Vector2(0, 0), _enemies, _bullets[1], true,0, true);
            Gun Rifle = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/3"), new Vector2(0, 0), _enemies, _bullets[2], false,0, true);
            Gun MachineGun = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/4"), new Vector2(0, 0), _enemies, _bullets[3], false,0.5f, true);
            Gun Uzi = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/5"), new Vector2(0, 0), _enemies, _bullets[4], false,0.2f, true);
            _guns.Add(M16);
            _guns.Add(Sniper);
            _guns.Add(Rifle);
            _guns.Add(MachineGun);
            _guns.Add(Uzi);
        }
        public Gun GetGunCopy(int id, float scale,bool hitPlayers)
        {
            if(id == -1)
            {
                Random x = new Random();
                id = x.Next(0, 5);
            }
            return _guns[id].Copy(scale, hitPlayers);
        }
        public MeleeWeapon GetMeleeWeaponCopy(int id, float scale)
        {
            return _meleeWeapons[id].Copy(scale);
        }
        public Simple_Enemy GetSimpleEnemyCopy(int id, float scale)
        {
            return _simple_enemies[id].Copy(scale,GetGunCopy(-1,0.7f,true),null);
        }
        public Item GetItem(int id)
        {
            return _items[id];
        }
    }
}
