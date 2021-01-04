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
            InitializeItems();
            InitializeSimpleEnemies();

        }
        private void InitializeSimpleEnemies()
        {
            int id = 0;
            _simple_enemies = new List<Simple_Enemy>();
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimation4x4Dictionary(_contentManager.Load<Texture2D>("Patreon sprites 1/1")),
                id++,Vector2.Zero,20,_playerManager,_itemManager,10,new int[] { 0, 1, 2, 3, 4 }));
            _simple_enemies.Add(new Simple_Enemy(GraphicManager.GetAnimation4x4Dictionary(_contentManager.Load<Texture2D>("Patreon sprites 1/24")),
                id++, Vector2.Zero,5, _playerManager, _itemManager, 20, new int[] { 0, 1, 2, 3, 4 }));
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
            _bullets = new List<Bullet>();
            int id = 0;
            Texture2D _bullet_texture = _contentManager.Load<Texture2D>("etc/bullet");
            _bullets.Add(new Bullet(id++, _bullet_texture, _enemies, 5, 0.01f,1));
            _bullets.Add(new Bullet(id++, _bullet_texture, _enemies, 20, 1, 10));

        }
        private void InitializeGuns()
        {
            _guns = new List<Gun>();
            int id = 0;
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
