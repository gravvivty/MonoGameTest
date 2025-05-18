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
        public static Weapon currentWeapon = new Weapon(1, 1, 1, 1, 1, 1, bulletTexture); // dummy
        public static Weapon baseWeapon;
        public static Texture2D bulletTexture;
        public static Color bulletTint;

        // Weapon Attribute Multipliers
        public static float attackSpeedMult = 1;
        public static float shotSpeedMult = 1;
        public static float bulletSizeMult = 1;
        public static float bulletSpreadMult = 1;
        public static int bulletsPerShotMult = 1;
        public static float bulletDamageMult = 1;

        // Weapon Attribute Flat Values
        public static float attackSpeedFlat = 0;
        public static float shotSpeedFlat = 0;
        public static float bulletSizeFlat = 0;
        public static float bulletSpreadFlat = 0;
        public static int bulletsPerShotFlat = 0;
        public static float bulletDamageFlat = 0;

        /// <summary>
        /// Updates the current weapon's attributes based on the multipliers and flat values.
        /// </summary>
        public static void UpdatePlayerGameData()
        {
            // Update the current weapon with the new multipliers and flat values
            currentWeapon.attackSpeed = (baseWeapon.attackSpeed * attackSpeedMult) + attackSpeedFlat;
            currentWeapon.shotSpeed = (baseWeapon.shotSpeed * shotSpeedMult) + shotSpeedFlat;
            currentWeapon.bulletSize = (baseWeapon.bulletSize * bulletSizeMult) + bulletSizeFlat;
            currentWeapon.bulletSpread = (baseWeapon.bulletSpread * bulletSpreadMult) + bulletSpreadFlat;
            currentWeapon.bulletsPerShot = (int)((baseWeapon.bulletsPerShot * bulletsPerShotMult) + bulletsPerShotFlat);
            currentWeapon.bulletDamage = (baseWeapon.bulletDamage * bulletDamageMult) + bulletDamageFlat;
        }
    }
}