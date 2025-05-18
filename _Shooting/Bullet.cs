using System;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class Bullet
    {
        private Vector2 _position;
        private Vector2 _shotSpeed;
        private float _bulletSize;
        private bool _isVisible = true;
        private float _timer = 0f;
        private float _visibilityTime = 0.5f;
        private Texture2D _texture;

        public Bullet(Texture2D texture, Vector2 startposition, Vector2 direction, float shotSpeed, float bulletSize)
        {
            this._texture = texture;
            _position = startposition;
            _shotSpeed = Vector2.Normalize(direction) * shotSpeed;
            _bulletSize = bulletSize;
        }

        public void Update()
        {
            _position += _shotSpeed * (float)Globals.Time;
            _timer += (float)Globals.Time;
            System.Diagnostics.Debug.WriteLine("Trying to update Bullet location" + DateTime.Now);

            if (_timer >= _visibilityTime)
            {
                _isVisible = false;
                _timer = 0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                Globals.SpriteBatch.Draw(_texture, _position, new Rectangle(0, 0, 16, 16), Color.White, 0f, Vector2.Zero, _bulletSize, SpriteEffects.None, 1);
                System.Diagnostics.Debug.WriteLine("Trying to draw the Bullet" + DateTime.Now);
            }
        }

        public bool IsVisible => _isVisible;
        public Vector2 Position => _position;
    }
}
