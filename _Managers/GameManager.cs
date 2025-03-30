using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    public class GameManager
    {
        private readonly Player _player;
        private readonly Renderer _renderer;
        private readonly SpriteManager _spriteManager;

        public GameManager()
        {
            _spriteManager = new SpriteManager();
            _player = new Player(_spriteManager);
            _renderer = new Renderer(_player, _spriteManager);

            // Calculates ALL collisions in the level
            Globals.CalculateAllCollisions();
        }

        public void Update()
        {
            // Every Frame check input
            InputManager.Update(_player);
            _player.Update();
        }

        public void Draw()
        {
            _renderer.DrawWorld();
        }
    }
}