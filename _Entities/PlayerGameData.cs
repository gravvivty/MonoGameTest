using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    public static class PlayerGameData
    {
        /*
        Weapon Class Attributes:
        private float attackSpeed;
        private float shotSpeed;
        private float bulletSize;
        private float bulletSpread;
        private int bulletsPerShot;
        private float bulletDamage;
        */
        public static Weapon CurrentWeapon;
        public static Weapon BaseWeapon;
        public static Texture2D BulletTexture;
        public static Color BulletTint;

        // Weapon Attribute Multipliers
        public static float AttackSpeedMult = 1;
        public static float ShotSpeedMult = 1;
        public static float BulletSizeMult = 1;
        public static float BulletSpreadMult = 1;
        public static int BulletsPerShotMult = 1;
        public static float BulletDamageMult = 1;

        // Weapon Attribute Flat Values
        public static float AttackSpeedFlat = 0;
        public static float ShotSpeedFlat = 0;
        public static float BulletSizeFlat = 0;
        public static float BulletSpreadFlat = 0;
        public static int BulletsPerShotFlat = 0;
        public static float BulletDamageFlat = 0;

        /// <summary>
        /// Updates the current weapon's attributes based on the multipliers and flat values.
        /// </summary>
        public static void UpdatePlayerGameData()
        {
            // Update the current weapon with the new multipliers and flat values
            CurrentWeapon.attackSpeed = (BaseWeapon.attackSpeed * AttackSpeedMult) + AttackSpeedFlat;
            CurrentWeapon.shotSpeed = (BaseWeapon.shotSpeed * ShotSpeedMult) + ShotSpeedFlat;
            CurrentWeapon.bulletSize = (BaseWeapon.bulletSize * BulletSizeMult) + BulletSizeFlat;
            CurrentWeapon.bulletSpread = (BaseWeapon.bulletSpread * BulletSpreadMult) + BulletSpreadFlat;
            CurrentWeapon.bulletsPerShot = (int)((BaseWeapon.bulletsPerShot * BulletsPerShotMult) + BulletsPerShotFlat);
            CurrentWeapon.bulletDamage = (BaseWeapon.bulletDamage * BulletDamageMult) + BulletDamageFlat;
        }
    }
}