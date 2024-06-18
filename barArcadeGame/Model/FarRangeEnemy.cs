using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;

namespace barArcadeGame.Model
{
    public class FarRangeEnemy : ParentEnemy
    {
        private bool isAttacking = false;

        public FarRangeEnemy() : base()
        {
            enemyBounds = new Rectangle((int)pos.X + 35, (int)pos.Y + 10, 30, 30);
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
                Attack();
            }
        }

        private void Attacking(GameTime gameTime)
        {
            attackTimer += gameTime.ElapsedGameTime;
            if (attackTimer >= attackDuration)
            {
                isAttacking = false;
                attackTimer = TimeSpan.Zero;
            }
            else
            {
                enemySprite.Play("attack");
                enemySprite.Update(gameTime);
            }
        }

        public void Attack()
        {
            if (!isAttacking)
            {
                isAttacking = true;
                attackTimer = TimeSpan.Zero;
                enemySprite.Play("attack");
            }
        }

        public override void Update(GameTime gameTime, Vector2 playerPos)
        {
            if (isDead) return;

            if (isDying)
            {
                Dying(gameTime);
            }
            else if (isAttacking)
            {
                Attacking(gameTime);
            }
            else
            {
                Movement(gameTime, playerPos);
            }

            enemyBounds.X = (int)pos.X - 8;
            enemyBounds.Y = (int)pos.Y - 8;
        }
    }
}
