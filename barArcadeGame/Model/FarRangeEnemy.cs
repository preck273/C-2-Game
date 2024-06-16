
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;

namespace barArcadeGame.Model
{
    public class FarRangeEnemy
    {
        public Vector2 pos;

        private AnimatedSprite enemySprite;
        private SpriteSheet sheet;
        private float moveSpeed = 1.0f;
        public Rectangle enemyBounds;
        public bool isDead = false;
        public bool isDying = false;
        public bool isAttacking = false;
        public string animation = "";
        private Vector2 targetPos;
        private TimeSpan attackDuration = TimeSpan.FromSeconds(1);
        private TimeSpan attackTimer = TimeSpan.Zero;

        public FarRangeEnemy()
        {
            pos = new Vector2(100, 100);
            enemyBounds = new Rectangle((int)pos.X + 35, (int)pos.Y + 10, 30, 80);
        }

        public void Load(SpriteSheet spriteSheet, Vector2 location)
        {
            pos = location;
            sheet = spriteSheet;
            enemySprite = new AnimatedSprite(sheet);
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            if (isDead) return;

            if (isDying)
            {
                attackTimer += gameTime.ElapsedGameTime;
                if (attackTimer >= attackDuration)
                {
                    isDying = false;
                    isDead = true;
                    attackTimer = TimeSpan.Zero;
                }
                else
                {
                    enemySprite.Play("die");
                    enemySprite.Update(gameTime);
                }
            }
            else if (isAttacking)
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
            else
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
                else // attack
                {
                    StartAttack();
                }
            }

            enemyBounds.X = (int)pos.X - 8;
            enemyBounds.Y = (int)pos.Y - 8;
        }

        public void Kill()
        {
            isDying = true;
            isAttacking = false;
            attackTimer = TimeSpan.Zero;
        }

        private void StartAttack()
        {
            isAttacking = true;
            attackTimer = TimeSpan.Zero;
        }

        public void Draw(SpriteBatch spriteBatch, Matrix matrix, Matrix transformMatrix)
        {
            if (isDead) return;

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                samplerState: SamplerState.PointClamp,
                effect: null,
                blendState: null,
                rasterizerState: null,
                depthStencilState: null,
                transformMatrix: transformMatrix);

            Vector2 scale = new Vector2(0.65f, 0.65f);
            spriteBatch.Draw(enemySprite, pos, 0, scale);

            spriteBatch.End();
        }
    }
}
