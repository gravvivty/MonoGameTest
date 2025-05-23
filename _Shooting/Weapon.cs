using System;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class Weapon
    {
        public float attackSpeed;
        public float shotSpeed;
        public float bulletSize;
        public float bulletSpread;
        public int   bulletsPerShot;
        public float bulletDamage;
        public float timeSinceLastShot;

        public Texture2D bulletTexture;
        public Texture2D ingameSprite;
        public Texture2D iconSprite;

        public Weapon(float attackspeed, float shotspeed, float bulletSize, float bulletSpread, int bulletsPerShot, float bulletDamage, Texture2D bulletTexture, Texture2D icon, Texture2D ingame)
        {
            this.attackSpeed = attackspeed;
            this.shotSpeed = shotspeed;
            this.bulletSize = bulletSize;
            this.bulletSpread = bulletSpread;
            this.bulletsPerShot = bulletsPerShot;
            this.bulletDamage = bulletDamage;

            this.bulletTexture = bulletTexture;
            this.ingameSprite = ingame;
            this.iconSprite = icon;
        }

        /* protected float FireCooldown
        {
            get => _fireCooldown;
            set => _fireCooldown = value;
        }

        protected float TimeSinceLastShot
        {
            get => _timeSinceLastShot;
            set => _timeSinceLastShot = value;
        }

        protected Texture2D BulletTexture => _bulletTexture;
        protected Vector2 Position => _position;
        protected List<Bullet> Bullets => _bullets;

        public Weapon(Texture2D bulletTexture, Vector2 position)
        {
            _bulletTexture = bulletTexture;
            _position = position;
        }

        public abstract void Update();
        public abstract void Shoot(Vector2 direction, Vector2 player_position);

        public List<Bullet> GetBullets() => _bullets;
        */
    }
}
