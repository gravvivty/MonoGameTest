using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpFont;

namespace SWEN_Game
{
    public static class Cursor
    {
        private static Texture2D cursorTexture = Globals.Content.Load<Texture2D>("crosshair");

        public static void DrawCursor()
        {
            MouseState mouse = Mouse.GetState();

            // Downscale Mouse Pos
            Vector2 worldMousePos = new Vector2(mouse.X, mouse.Y) / Globals.Zoom;

            // Matrix also upscales MousePos - that's why worldMousePos
            Globals.SpriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                transformMatrix: Matrix.CreateScale(Globals.Zoom, Globals.Zoom, 1f),
                samplerState: SamplerState.PointClamp);

            Globals.SpriteBatch.Draw(
                cursorTexture,
                worldMousePos,
                null,
                Color.White,
                0f,
                new Vector2(5, 5),
                1f,
                SpriteEffects.None,
                1f);

            Globals.SpriteBatch.End();
        }
    }
}
