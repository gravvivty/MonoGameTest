using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpFont;

namespace SWEN_Game
{
    public static class InputManager
    {
        private static Texture2D cursorTexture = Globals.Content.Load<Texture2D>("crosshair");

        private static Vector2 moveDirection;
        public static void Update(Player player)
        {
            moveDirection = Vector2.Zero;

            // How long was the button held
            float delta = Globals.Time;
            KeyboardState keyboardState;
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.W)) { moveDirection.Y = -1; }
            if (keyboardState.IsKeyDown(Keys.S)) { moveDirection.Y = 1; }
            if (keyboardState.IsKeyDown(Keys.A)) { moveDirection.X = -1; }
            if (keyboardState.IsKeyDown(Keys.D)) { moveDirection.X = 1; }

            // Normalize Vector
            if (moveDirection != Vector2.Zero) { moveDirection.Normalize(); }

            // X - Move Player if not colliding otherwise do not update Pos
            Vector2 tentativePosition = player.Position;
            Vector2 tentativePositionReal = player.RealPos;
            tentativePosition.X += moveDirection.X * player.Speed * delta;
            tentativePositionReal.X += moveDirection.X * player.Speed * delta;
            if (!Globals.IsColliding(tentativePosition, player.Texture))
            {
                player.SetPosition(tentativePosition, tentativePositionReal);
            }

            // Same thing but for Y
            tentativePosition = player.Position;
            tentativePositionReal = player.RealPos;
            tentativePosition.Y += moveDirection.Y * player.Speed * delta;
            tentativePositionReal.Y += moveDirection.Y * player.Speed * delta;
            if (!Globals.IsColliding(tentativePosition, player.Texture))
            {
                player.SetPosition(tentativePosition, tentativePositionReal);
            }

            // Normalize for Animation Use
            moveDirection = new Vector2(Math.Sign(moveDirection.X), Math.Sign(moveDirection.Y));
        }

        public static Vector2 GetDirection()
        {
            return moveDirection;
        }

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
