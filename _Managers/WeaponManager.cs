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
            Weapon pistol = new Weapon(0.3f, 250f, 1f, 1, 1, 1, vanillaBulletTexture);
            _weapons.Add("Pistol", pistol);

            Weapon smg = new Weapon(0.1f, 400f, 0.5f, 1, 1, 1, vanillaBulletTexture);
            _weapons.Add("SMG", smg);

            Weapon sniper = new Weapon(0.8f, 600f, 0.4f, 1, 1, 1, vanillaBulletTexture);
            _weapons.Add("Sniper", sniper);

            PlayerGameData.baseWeapon = this.GetWeapon("Pistol");
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
