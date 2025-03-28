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
        public Vector2 position { get; private set; }
        public float speed { get; private set; }
        public Rectangle rect { get; private set; }

        public Player()
        {
            texture = Globals.Content.Load<Texture2D>("hm_1");
            speed = 150f;
            // Spawn Pos
            position = new Vector2(50, 50);
        }

        public void SetPosition(Vector2 newPos)
        {
            position = newPos;
        }
    }
}


