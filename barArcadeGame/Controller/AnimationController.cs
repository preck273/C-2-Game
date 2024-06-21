namespace barArcadeGame
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System.Collections.Generic;

    public class AnimationController
    {
        private readonly Dictionary<object, Animation> _animations = new();
        private object _currentKey;

        public void AddAnimation(object key, Animation animation)
        {
            _animations[key] = animation;
            _currentKey ??= key;
        }

        public void Update(object key)
        {
            if (_animations.TryGetValue(key, out var animation))
            {
                animation.Start();
                animation.Update();
                _currentKey = key;
            }
            else if (_currentKey != null)
            {
                _animations[_currentKey].Stop();
                _animations[_currentKey].Reset();
            }
        }

        public void Draw(Vector2 position)
        {
            if (_currentKey != null)
            {
                _animations[_currentKey].Draw(position);
            }
        }

        public void DrawNinja(Vector2 pos)
        {
            if (_currentKey != null)
            {
                _animations[_currentKey].DrawNinja(pos);
            }
        }
    }
}
