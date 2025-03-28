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
        public static void manageInput(Player player)
        {
            Vector2 moveDirection = Vector2.Zero;
            float delta = Globals.Time;
            KeyboardState keyboardState;
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.W)) moveDirection.Y = -1;
            if (keyboardState.IsKeyDown(Keys.S)) moveDirection.Y = 1;
            if (keyboardState.IsKeyDown(Keys.A)) moveDirection.X = -1;
            if (keyboardState.IsKeyDown(Keys.D)) moveDirection.X = 1;

            // TIL you can not use curly brackets
            if (moveDirection != Vector2.Zero) moveDirection.Normalize();

            Vector2 tentativePosition = player.position;
            tentativePosition.X += moveDirection.X * player.speed * delta;
            if (!Globals.isColliding(tentativePosition, player.texture))
            {
                player.SetPosition(tentativePosition);
            }
            tentativePosition = player.position;
            tentativePosition.Y += moveDirection.Y * player.speed * delta;
            if (!Globals.isColliding(tentativePosition, player.texture))
            {
                player.SetPosition(tentativePosition);
            }
        }
    }
}
