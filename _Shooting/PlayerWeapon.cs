using System;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class PlayerWeapon
    {
        /*
        Weapon Class Attributes:
        private float attackSpeed;
        private float shotSpeed;
        private float bulletSize;
        private float bulletSpread;
        private int   bulletsPerShot;
        private float bulletDamage;
        private float timeSinceLastShot;

        */

        /*
        Weapon Names:
        - Pistol
        - SMG
        - Sniper
        */

        private List<Bullet> _bullets = new List<Bullet>();

        private float _timeSinceLastShot;

        public PlayerWeapon(WeaponManager weaponManager)
        {
            PlayerGameData.bulletTexture = Globals.Content.Load<Texture2D>("Sprites/Bullets/FlameBullet");
            PlayerGameData.bulletTint = Color.White;
        }

        protected float TimeSinceLastShot
        {
            get => _timeSinceLastShot;
            set => _timeSinceLastShot = value;
        }
        public List<Bullet> GetBullets() => _bullets;

        public void Update()
        {
            float gametime = Globals.Time;
            TimeSinceLastShot += (float)gametime;

            foreach (var bullet in _bullets)
            {
                bullet.Update();
            }

            _bullets.RemoveAll(b => !b.IsVisible);
        }

        public void Shoot(Vector2 direction, Vector2 player_position)
        {
            System.Diagnostics.Debug.WriteLine("PlayerWeapon is now Trying to shoot" + DateTime.Now);
            if (TimeSinceLastShot >= PlayerGameData.currentWeapon.attackSpeed)
            {
                // Clone bullet anim to make each bullet independent
                Animation bulletAnim = new Animation(
                    PlayerGameData.bulletTexture, 1, 4, 0.1f, Globals.SpriteManager, 1, PlayerGameData.bulletTint, PlayerGameData.currentWeapon.bulletSize);
                // TODO: Direction calculated with bulletSpread factor
                _bullets.Add(new Bullet(bulletAnim, player_position, direction, PlayerGameData.currentWeapon.shotSpeed, PlayerGameData.currentWeapon.bulletSize));
                System.Diagnostics.Debug.WriteLine("PlayerWeapon is now shooting" + DateTime.Now);
                TimeSinceLastShot = 0f;
            }
        }
    }
}
