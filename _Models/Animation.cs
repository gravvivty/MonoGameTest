using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SWEN_Game
{
    public class Animation
    {
        private readonly Texture2D _texture;
        private readonly List<Rectangle> _srcRect = new();
        private readonly SpriteManager _spriteManager;
        private readonly int _totalFrames;
        private int _currentFrame;
        private readonly float _frameTime;
        private float _frameTimeLeft;
        private bool isActive = true;

        public Animation(Texture2D texture, int framesX, int framesY, float frameTime, SpriteManager spriteManager, int column = 1)
        {
            _spriteManager = spriteManager;
            _texture = texture;
            _frameTime = frameTime;
            _frameTimeLeft = _frameTime;
            _totalFrames = framesX;
            int frameWidth = 16;
            int frameHeight = 16;

            for (int i = 0; i < _totalFrames; i++)
            {
                _srcRect.Add(new Rectangle((column-1) * frameWidth, i * frameHeight, frameWidth, frameHeight));
            }
        }

        public void Start()
        {
            isActive = true;
        }

        public void Stop()
        {
            isActive = false;
        }

        public void Reset()
        {
            _currentFrame = 0;
            _frameTimeLeft = _frameTime;
        }

        public void Update()
        {
            if (!isActive)
            {
                return;
            }

            _frameTimeLeft -= Globals.Time;

            if (_frameTimeLeft <= 0)
            {
                _frameTimeLeft += _frameTime;
                _currentFrame = (_currentFrame+1) % _totalFrames;
            }
        }

        public void Draw(Vector2 position)
        {
            float depth = _spriteManager.GetDepth(position, 50);
            Globals.SpriteBatch.Draw(_texture, position, _srcRect[_currentFrame], Color.White, 0, Vector2.Zero, Vector2.One
                , SpriteEffects.None, depth);
        }
    }
}

