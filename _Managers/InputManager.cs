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
        public static Vector2 moveDirection;
        public static bool isMoving;
        public static void Update(Player player)
        {
            moveDirection = Vector2.Zero;
            // How long was the button held
            float delta = Globals.Time;
            KeyboardState keyboardState;
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.W)) moveDirection.Y = -1;
            if (keyboardState.IsKeyDown(Keys.S)) moveDirection.Y = 1;
            if (keyboardState.IsKeyDown(Keys.A)) moveDirection.X = -1;
            if (keyboardState.IsKeyDown(Keys.D)) moveDirection.X = 1;

            // Normalize Vector
            if (moveDirection != Vector2.Zero) moveDirection.Normalize();

            // X - Move Player if not colliding otherwise do not update Pos
            Vector2 tentativePosition = player.position;
            Vector2 tentativePositionReal = player.realPos;
            tentativePosition.X += moveDirection.X * player.speed * delta;
            tentativePositionReal.X += moveDirection.X * player.speed* delta;
            if (!Globals.isColliding(tentativePosition, player.texture))
            {
                player.SetPosition(tentativePosition, tentativePositionReal);
            }
            // Same thing but for Y
            tentativePosition = player.position;
            tentativePositionReal = player.realPos;
            tentativePosition.Y += moveDirection.Y * player.speed * delta;
            tentativePositionReal.Y += moveDirection.Y * player.speed * delta;
            if (!Globals.isColliding(tentativePosition, player.texture))
            {
                player.SetPosition(tentativePosition, tentativePositionReal);
            }
        }

        public static Vector2 GetDirection()
        {
            return moveDirection;
        }

        public static bool IsMoving()
        {
            if (moveDirection != Vector2.Zero)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
