using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    public class GameManager
    {
        private readonly Player _player;
        private readonly Renderer _renderer;

        public GameManager()
        {
            _player = new Player();
            _renderer = new Renderer(_player);
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

