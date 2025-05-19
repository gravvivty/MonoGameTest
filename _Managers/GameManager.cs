using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game;

namespace SWEN_Game
{
    public class GameManager
    {
        private readonly Player _player;
        private readonly Renderer _renderer;
        private readonly SpriteManager _spriteManager;
        private readonly SpriteCalculator _spriteCalculator;
        private readonly WeaponManager _weaponManager;
        private readonly PlayerWeapon _playerWeapon;
        private readonly Debug _debug;

        public GameManager()
        {
            _spriteManager = new SpriteManager();
            _spriteManager.MapTileToTexture();
            _player = new Player();
            _player.AddSpriteManager(_spriteManager);
            _spriteCalculator = new SpriteCalculator(_spriteManager, _player);
            _renderer = new Renderer(_player, _spriteManager, _spriteCalculator);

            _weaponManager = new WeaponManager();
            _weaponManager.InitWeapons();
            _playerWeapon = new PlayerWeapon(_weaponManager);

            _debug = new Debug(_player, _renderer);

            // Calculates ALL collisions in the level
            Globals.CalculateAllCollisions();

            // Set Global Classes
            Globals.SpriteManager = _spriteManager;
        }

        public void Update()
        {
           // System.Diagnostics.Debug.WriteLine("GameManager Update running" + DateTime.Now);

            // Every Frame check input
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            InputManager.Update(_player, keyboard);
            MouseManager.UpdateMouse(_player, _playerWeapon, mouse);
            _player.Update();
            _playerWeapon.Update();
        }

        public void Draw()
        {
            // Begin the sprite batch with depth sorting (FrontToBack) and apply the camera transformation.
            Globals.SpriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                transformMatrix: _renderer.CalcTranslation(),
                samplerState: SamplerState.PointClamp);
            _renderer.DrawWorld();
            foreach (var bullet in _playerWeapon.GetBullets())
            {
                bullet.Draw(Globals.SpriteBatch);
            }

            Globals.SpriteBatch.End();

            //_debug.DrawWorldDebug();
            Cursor.DrawCursor();

           // System.Diagnostics.Debug.WriteLine("GameManager Draw running" + DateTime.Now);
        }
    }
}
