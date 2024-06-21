using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;

namespace barArcadeGame.Model
{
    public abstract class ParentEnemy
    {
        public Vector2 pos;
        protected AnimatedSprite enemySprite;
        protected SpriteSheet sheet;
        protected float moveSpeed = 1.0f;
        protected Vector2 scale = new Vector2(0.65f, 0.65f); 
        public Rectangle enemyBounds;
        public bool isDead = false;
        public bool isDying = false;
        protected string animation = "";
        protected Vector2 targetPos;
        protected TimeSpan attackDuration = TimeSpan.FromSeconds(1);
        protected TimeSpan attackTimer = TimeSpan.Zero;
        protected int life = 5;

        public ParentEnemy()
        {
            pos = new Vector2(100, 100);
            enemyBounds = new Rectangle((int)pos.X, (int)pos.Y, 30, 30);
        }

        public void Load(SpriteSheet spriteSheet, Vector2 location)
        {
            pos = location;
            sheet = spriteSheet;
            enemySprite = new AnimatedSprite(sheet);
        }

        public virtual void Update(GameTime gameTime, Vector2 playerPos)
        {
            if (isDead) return;

            if (isDying)
            {
                Dying(gameTime);
            }
            else
            {
                Movement(gameTime, playerPos);
            }

            enemyBounds.X = (int)pos.X - 8;
            enemyBounds.Y = (int)pos.Y - 8;
        }

        protected virtual void Dying(GameTime gameTime)
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

        protected abstract void Movement(GameTime gameTime, Vector2 playerPos);

        public void Kill()
        {
                if (life <= 0)
                {
                    isDying = true;
                    attackTimer = TimeSpan.Zero;
                    enemySprite.Play("die");
                }
            
        }
        public void Damaged(int damage)
        {
            if (life > 0)
            {
                life -= damage;
            }
            Kill();
        }

        //fix
        public void CheckBulletCollision(Bullet bullet)
        {
            if (enemyBounds.Intersects(bullet.Bounds))
            {
                Damaged(bullet.damage);
            }
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

            
            spriteBatch.Draw(enemySprite, pos, 0, scale);

            spriteBatch.End();
        }
    }
}
