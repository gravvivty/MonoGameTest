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
            _player = new Player();
            _spriteManager = new SpriteManager();
            _renderer = new Renderer(_player, _spriteManager);
            Globals.calculateAllCollisions();
        }
        public void Update()
        {
            InputManager.manageInput(_player);
        }
        public void Draw()
        {
            _renderer.drawWorld();
        }
    }
}

