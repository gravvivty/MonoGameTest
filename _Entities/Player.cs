using System;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class Player
    {
        private SpriteManager _spriteManager;
        private AnimationManager _anims = new();

        // For general enemies/entities we can copy most of these variables
        public Texture2D Texture { get; private set; }

        // Used for drawing the Sprite
        public Vector2 Position { get; private set; }

        // Used to check if something is near the player
        public Vector2 RealPos { get; private set; }
        public float Speed { get; private set; }

        public Player()
        {
            Speed = 130f;

            // Spawn Pos
            Position = new Vector2(450, 450);

            // Offset Pos - used for actually comparing positions
            RealPos = new Vector2(456, 458);
        }

        public void AddSpriteManager(SpriteManager spriteManager)
        {
            Texture = Globals.Content.Load<Texture2D>("player");
            _spriteManager = spriteManager;

            _anims.AddAnimation(new Vector2(0, -1), new(Texture, 1, 3, 0.1f, 1));
            _anims.AddAnimation(new Vector2(1, -1), new(Texture, 1, 3, 0.1f, 2));
            _anims.AddAnimation(new Vector2(1, 0), new(Texture, 1, 3, 0.1f, 3));
            _anims.AddAnimation(new Vector2(1, 1), new(Texture, 1, 3, 0.1f, 4));
            _anims.AddAnimation(new Vector2(0, 1), new(Texture, 1, 3, 0.1f, 5));
            _anims.AddAnimation(new Vector2(-1, 1), new(Texture, 1, 3, 0.1f, 6));
            _anims.AddAnimation(new Vector2(-1, 0), new(Texture, 1, 3, 0.1f, 7));
            _anims.AddAnimation(new Vector2(-1, -1), new(Texture, 1, 3, 0.1f, 8));
        }

        public void SetPosition(Vector2 newPos, Vector2 newRealPos)
        {
            Position = newPos;
            RealPos = newRealPos;
        }

        public void Update()
        {
            _anims.Update(GetDirection());
        }

        private Vector2 _direction;
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public Vector2 GetDirection()
        {
            return _direction;
        }

        public void Draw()
        {
            _anims.Draw(Position);
        }
    }
}