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
        public static void Update(Player player, KeyboardState keyboardState)
        {
            Vector2 moveDirection = Vector2.Zero;

            // How long was the button held
            float delta = Globals.Time;
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

            Rectangle xCollision = new Rectangle((int)tentativePosition.X + 5, (int)tentativePosition.Y + 10, 8, 8);

            if (!Globals.IsColliding(xCollision))
            {
                player.SetPosition(tentativePosition, tentativePositionReal);
            }

            // Same thing but for Y
            tentativePosition = player.Position;
            tentativePositionReal = player.RealPos;
            tentativePosition.Y += moveDirection.Y * player.Speed * delta;
            tentativePositionReal.Y += moveDirection.Y * player.Speed * delta;

            Rectangle yCollision = new Rectangle((int)tentativePosition.X + 5, (int)tentativePosition.Y + 10, 8, 8);

            if (!Globals.IsColliding(yCollision))
            {
                player.SetPosition(tentativePosition, tentativePositionReal);
            }

            // Normalize for Animation Use
            moveDirection = new Vector2(Math.Sign(moveDirection.X), Math.Sign(moveDirection.Y));
            player.SetDirection(moveDirection);
        }
    }
}
