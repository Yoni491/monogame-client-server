using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
                GraphicManager.GetAnimationManager_swordSwing(), Vector2.Zero, _enemies,0.5f));
            _meleeWeapons.Add(new MeleeWeapon(id++, GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("Weapons/swords-sheet"), 1, 13, 0, 4),
                GraphicManager.GetAnimationManager_swordSwing(), Vector2.Zero, _enemies, 0.5f));
        }
        private void InitializeSimpleEnemies()
        {
            int id = 0;
            _simple_enemies = new List<Simple_Enemy>();
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(1), id++,Vector2.Zero,1,_playerManager,_itemManager,10,new int[] { 0, 1, 2, 3, 4 },GetMeleeWeaponCopy(0)));
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimationManager_spriteMovement(24), id++, Vector2.Zero, 1, _playerManager, _itemManager, 10, new int[] { 0, 1, 2, 3, 4 }, GetMeleeWeaponCopy(1)));
        }
        private void InitializeItems()
        {
            int id = 0;
            _items = new List<Item>();
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 4),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 4),
                id++, "Ore", 0.12f, 1, false, false, false, null, null,10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 0),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 0)
                , id++, "Wood", 0.12f, 1, false, false, false, null, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 6, 2),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 6, 2),
                id++, "Apple", 0.1f, 1, false, false, false, null, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 0, 3),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 0, 3),
                id++, "Bone", 0.08f, 1, false, false, false, null, null, 10));
            _items.Add(new Item(GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_basic"), 11, 11, 10, 1),
                GraphicManager.GetTextureSqaure(_contentManager.Load<Texture2D>("resources/resources_outlined"), 11, 11, 10, 1),
                id++, "Bone", 0.04f, 1, false, false, false, null, null, 10));
        }
        private void InitializeBullets()
        {
            int id = 0;
            _bullets = new List<Bullet>();
            Texture2D _bullet_texture = _contentManager.Load<Texture2D>("etc/bullet");
            _bullets.Add(new Bullet(id++, _bullet_texture, _enemies, 5, 0.01f,1));
            _bullets.Add(new Bullet(id++, _bullet_texture, _enemies, 20, 1, 10));

        }
        private void InitializeGuns()
        {
            int id = 0;
            _guns = new List<Gun>();
            Gun M16 = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/1"), new Vector2(0, 0), _enemies, _bullets[0],false);
            Gun Sniper = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/2"), new Vector2(0, 0), _enemies, _bullets[1],true);
            Gun Rifle = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/3"), new Vector2(0, 0), _enemies, _bullets[0], false);
            Gun MachineGun = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/4"), new Vector2(0, 0), _enemies, _bullets[0],false);
            Gun Uzi = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/5"), new Vector2(0, 0), _enemies, _bullets[0], false);
            _guns.Add(M16);
            _guns.Add(Sniper);
            _guns.Add(Rifle);
            _guns.Add(MachineGun);
            _guns.Add(Uzi);
        }
        public Gun GetGunCopy(int id)
        {
            return _guns[id].Copy();
        }
        public MeleeWeapon GetMeleeWeaponCopy(int id)
        {
            return _meleeWeapons[id].Copy();
        }
        public Simple_Enemy GetSimpleEnemyCopy(int id)
        {
            return _simple_enemies[id].Copy();
        }
        public Item GetItem(int id)
        {
            return _items[id];
        }
    }
}
