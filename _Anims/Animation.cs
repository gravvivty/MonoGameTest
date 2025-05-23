using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    public class Animation
    {
        public float _scale;
        public int frameSize = 16;

        private readonly Texture2D _texture;

        // Used to determine WHERE in the Spritesheet to get the Frame from
        private readonly List<Rectangle> _srcRect = new();
        private readonly int _totalFrames;
        private readonly float _frameTime;
        private int _currentFrame;
        private float _frameTimeLeft;
        private bool isActive = true;

        private Color _tintColor;

        public Animation(Texture2D texture, int framesX, int framesY, float frameTime, int column = 1, Color? tintColor = null, float? scale = null)
        {
            // Spritesheet
            _texture = texture;

            // How long frame should last
            _frameTime = frameTime;

            // How much time a frame has left before changing
            _frameTimeLeft = _frameTime;

            // Total Frames for anim
            _totalFrames = framesY;
            int frameWidth = 16;
            int frameHeight = 16;

            // Add the sprite frames from the sheet
            for (int i = 0; i < _totalFrames; i++)
            {
                _srcRect.Add(new Rectangle((column - 1) * frameWidth, i * frameHeight, frameWidth, frameHeight));
            }

            _tintColor = tintColor ?? Color.White;
            _scale = scale ?? 1f;
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

            // Subtract time from the current frame
            _frameTimeLeft -= Globals.Time;

            // If no time --> next frame + add time
            if (_frameTimeLeft <= 0)
            {
                _frameTimeLeft += _frameTime;
                _currentFrame = (_currentFrame + 1) % _totalFrames;
            }
        }

        public void Draw(Vector2 position)
        {
            float depth = Globals.SpriteManager.GetDepth(position, 16);
            Globals.SpriteBatch.Draw(
                _texture,
                position,
                _srcRect[_currentFrame],
                _tintColor,
                0,
                Vector2.Zero,
                _scale,
                SpriteEffects.None,
                depth);
        }
    }
}