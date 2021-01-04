using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace GameClient.Weapons
{
    class MeleeWeapon
    {
        public int _id;

        private Vector2 _position;

        protected Texture2D _texture;

        protected Vector2 _velocity;

        protected float _scale = 0.5f;

        protected Texture2D _bullet_texture;

        public List<Bullet> _bullets = new List<Bullet>();

        private Vector2 _direction;

        private List<Simple_Enemy> _enemies;

        public Bullet _bullet;
        public Vector2 Position { get => _position; set => _position = value; }

        private bool _isSniper;

        private bool _isGamePad;

    }
}
