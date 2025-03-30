using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    public class AnimationManager
    {
        // Key being used will be a direction Vector2
        private readonly Dictionary<object, Animation> _animations = new Dictionary<object, Animation>();
        private object _currentKey;
        public AnimationManager()
        {
            // dummy
        }

        public void AddAnimation(object key, Animation animation)
        {
            _animations.Add(key, animation);
            if (_currentKey == null)
            {
                _currentKey = key;
            }
        }

        public void Update(object key)
        {
            // If Key exists - start the animation and update it
            if (_animations.ContainsKey(key))
            {
                _animations[key].Start();
                _animations[key].Update();
                _currentKey = key;
            }

            /*
            else if (_lastKey != null && _animations.ContainsKey(_lastKey))
            {
                _animations[_lastKey].Stop();
                _animations[_lastKey].Reset();
            }
            */
        }

        public void Draw(Vector2 position)
        {
            _animations[_currentKey].Draw(position);
        }
    }
}