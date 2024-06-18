using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;

namespace barArcadeGame.Model
{
    public class Enemy : ParentEnemy
    {
        public int count = 1;
        public int life = 5;

        public Enemy() : base()
        {
            scale = new Vector2(2.5f, 2.5f);
            enemyBounds = new Rectangle((int)pos.X + 35, (int)pos.Y + 10, 30, 30);
        }

        public int GetCount()
        {
            return count;
        }

        protected override void Movement(GameTime gameTime, Vector2 playerPos)
        {
            targetPos = playerPos;
            Vector2 direction = targetPos - pos;

            if (direction.Length() > 1)
            {
                direction.Normalize();
                pos += direction * moveSpeed;

                animation = "move";
                enemySprite.Play(animation);
                enemySprite.Update(gameTime);
            }
            else
            {
                Kill();
            }
        }

    }
}
