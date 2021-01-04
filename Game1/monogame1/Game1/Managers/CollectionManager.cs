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
        public CollectionManager(List<Simple_Enemy> enemies, ContentManager contentManager)
        {
            _contentManager = contentManager;
            _enemies = enemies;
            Initialize();
        }
        public void player(int playerTemplateNum)
        {

        }
        private void Initialize()
        {
            InitializeBullets();
            InitializeGuns();


        }
        private void InitializeBullets()
        {
            _bullets = new List<Bullet>();
            int id = 0;
            Texture2D _bullet_texture = _contentManager.Load<Texture2D>("etc/bullet");
            Bullet bullet = new Bullet(id++, _bullet_texture, _enemies, 5);
            _bullets.Add(bullet);
        }
        private void InitializeGuns()
        {
            _guns = new List<Gun>();
            int id = 0;
            Gun M16 = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/1"), new Vector2(0, 0), _enemies, _bullets[0]);
            Gun Sniper = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/2"), new Vector2(0, 0), _enemies, _bullets[0]);
            Gun Rifle = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/3"), new Vector2(0, 0), _enemies, _bullets[0]);
            Gun MachineGun = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/4"), new Vector2(0, 0), _enemies, _bullets[0]);
            Gun Uzi = new Gun(id++, _contentManager.Load<Texture2D>("Weapons/5"), new Vector2(0, 0), _enemies, _bullets[0]);
            _guns.Add(M16);
            _guns.Add(Sniper);
            _guns.Add(Rifle);
            _guns.Add(MachineGun);
            _guns.Add(Uzi);
        }
        public Gun GetGun(int id)
        {
            return _guns[id].Copy();
        }
    }
}
