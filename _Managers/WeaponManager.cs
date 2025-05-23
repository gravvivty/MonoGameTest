using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game;

namespace SWEN_Game
{
    public class WeaponManager
    {
        private Dictionary<string, Weapon> _weapons;

        public WeaponManager()
        {
            _weapons = new Dictionary<string, Weapon>();
        }

        public void InitWeapons()
        {
            // Pistol
            Texture2D vanillaBulletTexture = Globals.Content.Load<Texture2D>("Sprites/Bullets/VanillaBullet");

            Texture2D pistolIconTexture = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_UI/pistol");
            Texture2D pistolIngameTexture = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_Ingame/pistol");
            Weapon pistol = new Weapon(0.5f, 300f, 1f, 1, 1, 8f, vanillaBulletTexture, pistolIconTexture, pistolIngameTexture);
            _weapons.Add("Pistol", pistol);

            Texture2D assaultRifleIconTexture = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_UI/assault_rifle");
            Texture2D assaultIngameTexture = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_Ingame/assault_rifle");
            Weapon assault_rifle = new Weapon(0.2f, 300f, 0.7f, 1, 1, 2.5f, vanillaBulletTexture, assaultRifleIconTexture, assaultIngameTexture);
            _weapons.Add("Assault_Rifle", assault_rifle);

            Texture2D precisionRifleIconTexture = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_UI/precision_rifle");
            Texture2D precisionRifleIngameTexture = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_Ingame/precision_rifle");
            Weapon precision_rifle = new Weapon(0.8f, 500f, 0.7f, 1, 1, 16f, vanillaBulletTexture, precisionRifleIconTexture, precisionRifleIconTexture);
            _weapons.Add("Precision_Rifle", precision_rifle);

            PlayerGameData.BaseWeapon = this.GetWeapon("Pistol");
            PlayerGameData.CurrentWeapon = PlayerGameData.BaseWeapon;
            PlayerGameData.UpdatePlayerGameData();
        }

        public Weapon GetWeapon(string weaponName)
        {
            if (_weapons.TryGetValue(weaponName, out Weapon weapon))
            {
                return weapon;
            }

            // This should never return Null
            // If this return Null. We are fucked.
            // We dont need exception handling ~ Nico
            return null;
        }

            /* public void Update()
            {
                currentWeapon.Update();
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                foreach (var bullet in currentWeapon.GetBullets())
                {
                    bullet.Draw(spriteBatch);
                }
            }

            public void Shoot(Vector2 direction, Vector2 player_position)
            {
                currentWeapon.Shoot(direction, player_position);
            }

            public void SetWeapon(Weapon newWeapon)
            {
                currentWeapon = newWeapon;
            }

            public Weapon GetWeapon()
            {
                return currentWeapon;
            } */
        }
}
