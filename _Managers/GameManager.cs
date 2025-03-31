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
        private readonly SpriteCalculator _spriteCalculator;
        private readonly Debug _debug;

        public GameManager()
        {
            _spriteManager = new SpriteManager();
            _player = new Player(_spriteManager);
            _spriteCalculator = new SpriteCalculator(_spriteManager, _player);
            _renderer = new Renderer(_player, _spriteManager, _spriteCalculator);
            _debug = new Debug(_player, _renderer);

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
            // Begin the sprite batch with depth sorting (FrontToBack) and apply the camera transformation.
            Globals.SpriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                transformMatrix: _renderer.CalcTranslation(),
                samplerState: SamplerState.PointClamp);
            _renderer.DrawWorld();
            Globals.SpriteBatch.End();
            _debug.DrawWorldDebug();

            InputManager.DrawCursor();
        }
    }
}