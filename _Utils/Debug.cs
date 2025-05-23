using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game;

namespace SWEN_Game
{
    public class Debug
    {
        private readonly Player _player;
        private readonly Renderer _renderer;
        public Debug(Player player, Renderer renderer)
        {
            _player = player;
            _renderer = renderer;
        }

        public void DrawWorldDebug()
        {
            Globals.SpriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                transformMatrix: _renderer.CalcTranslation(),
                samplerState: SamplerState.PointClamp);

            // Draw the player sprite using its calculated depth.
            // _spriteManager.DrawPlayer(Globals.SpriteBatch, _player.texture, _player.position);

            // Draw the player's collision box for debugging, using a pink overlay.
            Rectangle entityRect = new Rectangle((int)_player.Position.X + 5, (int)_player.Position.Y + 10, 8, 8);
            Globals.SpriteBatch.Draw(
                Globals.Content.Load<Texture2D>("debug_rect"),
                entityRect,
                null,
                Color.Pink,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                1f);

            // Draw any collision areas in red.
            foreach (var collision in Globals.Collisions)
            {
                Globals.SpriteBatch.Draw(
                    Globals.Content.Load<Texture2D>("debug_rect"),
                    collision,
                    null,
                    Color.Red,
                    0f,
                    new Vector2(0, 0),
                    SpriteEffects.None,
                    1f);
            }

            // Draw Player Position/Rectangle
            Rectangle posRect = new Rectangle(
                (int)_player.RealPos.X,
                (int)_player.RealPos.Y,
                _player.Texture.Width / 20,
                _player.Texture.Height / 10);
            Globals.SpriteBatch.Draw(
                Globals.Content.Load<Texture2D>("debug_rect"),
                posRect,
                null,
                Color.Blue,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                1f);

            Globals.SpriteBatch.End();
        }
    }
}