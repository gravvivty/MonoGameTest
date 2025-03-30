using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SWEN_Game
{
    public class AnimationManager
    {
        private readonly Dictionary<object, Animation> _animations = new Dictionary<object, Animation>();
        private object _lastKey;
        public AnimationManager()
        {

        }

        public void AddAnimation(object key, Animation animation)
        {
            _animations.Add(key, animation);
            // Only assigns key to _lastKey if _lastKey is NULL
            _lastKey = key;
        }

        public void Update(object key)
        {
            if (_animations.ContainsKey(key))
            {
                _animations[key].Start();
                _animations[key].Update();
                _lastKey = key;
            }
            else if (_lastKey != null && _animations.ContainsKey(_lastKey))
            {
                _animations[_lastKey].Stop();
                _animations[_lastKey].Reset();
            }
        }

        public void Draw(Vector2 position)
        {
            _animations[_lastKey].Draw(position);
        }
    }
}

