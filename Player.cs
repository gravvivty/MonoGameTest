using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace SWEN_Game
{
    public class Player
    {
        public Texture2D texture {  get; private set; }
        // Used for drawing the Sprite
        public Vector2 position { get; private set; }
        // Used to check if something is near the player
        public Vector2 realPos { get; private set; }
        public float speed { get; private set; }
        public Rectangle playerRect { get; private set; }

        public Player()
        {
            texture = Globals.Content.Load<Texture2D>("hm_1");
            speed = 150f;
            // Spawn Pos
            position = new Vector2(50, 50);
            // Offset Pos - used for actually comparing positions
            realPos = new Vector2(50 + texture.Width/2-5, 50+texture.Height-10);
        }

        public void SetPosition(Vector2 newPos, Vector2 newRealPos)
        {
            position = newPos;
            realPos = newRealPos;
        }
    }
}


