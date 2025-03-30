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

        private readonly SpriteManager _spriteManager;

        private readonly AnimationManager _anims = new();

        public Player(SpriteManager spriteManager)
        {
            texture = Globals.Content.Load<Texture2D>("player");
            _spriteManager = spriteManager;
            speed = 150f;
            // Spawn Pos
            position = new Vector2(50, 50);
            // Offset Pos - used for actually comparing positions
            realPos = new Vector2(58, 62);

            _anims.AddAnimation(new Vector2(0, -1), new(texture, 1, 3, 0.1f, _spriteManager,1));
            _anims.AddAnimation(new Vector2(1, -1), new(texture, 1, 3, 0.1f, _spriteManager, 2));
            _anims.AddAnimation(new Vector2(1, 0), new(texture, 1, 3, 0.1f, _spriteManager, 3));
            _anims.AddAnimation(new Vector2(1, 1), new(texture, 1, 3, 0.1f, _spriteManager, 4));
            _anims.AddAnimation(new Vector2(0, 1), new(texture, 1, 3, 0.1f, _spriteManager, 5));
            _anims.AddAnimation(new Vector2(-1, 1), new(texture, 1, 3, 0.1f, _spriteManager, 6));
            _anims.AddAnimation(new Vector2(-1, 0), new(texture, 1, 3, 0.1f, _spriteManager, 7));
            _anims.AddAnimation(new Vector2(-1, -1), new(texture, 1, 3, 0.1f, _spriteManager, 8));

        }

        public void SetPosition(Vector2 newPos, Vector2 newRealPos)
        {
            position = newPos;
            realPos = newRealPos;
        }

        public void Update()
        {
            _anims.Update(InputManager.moveDirection);
        }

        public void Draw()
        {
            _anims.Draw(position);
        }
    }
}


