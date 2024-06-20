using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace barArcadeGame.Model
{
    public class Player
    {
        public Vector2 pos;
        private AnimatedSprite[] playerSprite;
        private SpriteSheet sheet;
        private float moveSpeed = 1.5f;
        private int dash = 50;
        private int lifes = 10;
        private int damage = 1;
        public int dashCountdown = 0;
        public Rectangle playerBounds;
        public bool isIdle = false;
        public bool isPlayed = false;
        public string animation = "";
        public string attackanimation = "";
        private bool isAttacking = false;
        private TimeSpan attackDuration = TimeSpan.FromSeconds(1);
        private TimeSpan attackTimer = TimeSpan.Zero;
        private TimeSpan damageCooldownDuration = TimeSpan.FromSeconds(1); 
        private TimeSpan damageCooldownTimer = TimeSpan.Zero;



        public Player()
        {
            playerSprite = new AnimatedSprite[3];
            pos = new Vector2(600, 50);

            playerBounds = new Rectangle((int)pos.X + 35, (int)pos.Y + 10, 30, 40);

        }
        public void Load(SpriteSheet[] spriteSheets, Vector2 location)
        {
            pos = location;

            Debug.WriteLine(spriteSheets.Length);
            for (int i = 0; i < spriteSheets.Length; i++)
            {

                sheet = spriteSheets[i];
                playerSprite[i] = new AnimatedSprite(sheet);
            }
        }

        public int GetDamage()
        {
            
            return damage;

        }
        
        public int GetLife()
        {
            return lifes;
        }

        public bool TakeDamage(int value)
        {
            if (damageCooldownTimer <= TimeSpan.Zero && lifes > 0)
            {
                Debug.WriteLine(lifes);
                pos.Y -= 3;
                lifes -= value;
                damageCooldownTimer = damageCooldownDuration; 
                return true;
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            // Decrease the cooldown timer
            if (damageCooldownTimer > TimeSpan.Zero)
            {
                damageCooldownTimer -= gameTime.ElapsedGameTime;
            }

            if (dashCountdown < 100)
            {
                dashCountdown += 1;
            }

            if (dashCountdown == 100 && isPlayed == false)
            {
                SoundController.PlayReadyDashFX();
                isPlayed = true;
            }

            var mouseState = Mouse.GetState();
            var keyboardstate = Keyboard.GetState();

            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed && !isAttacking)
            {
                isAttacking = true;
                attackTimer = TimeSpan.Zero;
            }

            if (isAttacking)
            {
                attackTimer += gameTime.ElapsedGameTime;
                if (attackTimer >= attackDuration)
                {
                    isAttacking = false;
                    attackTimer = TimeSpan.Zero;
                }
                else
                {
                    playerSprite[2].Play("attackDown");
                    playerSprite[2].Update(gameTime);
                    isIdle = false;
                }
            }
            else
            {
                if (keyboardstate.IsKeyDown(Keys.Space) || keyboardstate.IsKeyDown(Keys.W) || keyboardstate.IsKeyDown(Keys.A) || keyboardstate.IsKeyDown(Keys.S) || keyboardstate.IsKeyDown(Keys.D))
                {
                    if (keyboardstate.IsKeyDown(Keys.W))
                    {
                        animation = "walkUp";
                        pos.Y -= moveSpeed;
                        isIdle = false;
                    }
                    if (keyboardstate.IsKeyDown(Keys.A))
                    {
                        animation = "walkLeft";
                        pos.X -= moveSpeed;
                        isIdle = false;
                    }
                    if (keyboardstate.IsKeyDown(Keys.S))
                    {
                        animation = "walkDown";
                        pos.Y += moveSpeed;
                        isIdle = false;
                    }
                    if (keyboardstate.IsKeyDown(Keys.D))
                    {
                        animation = "walkRight";
                        pos.X += moveSpeed;
                        isIdle = false;
                    }

                    if (keyboardstate.IsKeyDown(Keys.Space) && dashCountdown == 100) // Dash
                    {
                        isPlayed = false;
                        SoundController.PlayDashFX();
                        if (animation == "walkUp")
                        {
                            animation = "walkUp";
                            pos.Y -= moveSpeed * dash;
                            isIdle = false;
                        }
                        if (animation == "walkLeft")
                        {
                            animation = "walkLeft";
                            pos.X -= moveSpeed * dash;
                            isIdle = false;
                        }

                        if (animation == "walkDown")
                        {
                            animation = "walkDown";
                            pos.Y += moveSpeed * dash;
                            isIdle = false;
                        }
                        if (animation == "walkRight")
                        {
                            animation = "walkRight";
                            pos.X += moveSpeed * dash;
                            isIdle = false;
                        }
                        dashCountdown = 0;
                    }

                    playerSprite[1].Play(animation);
                    playerSprite[1].Update(gameTime);
                    isIdle = false;
                }
                else
                {
                    isIdle = true;
                    playerSprite[0].Play("idleDown");
                    playerSprite[0].Update(gameTime);
                }
            }

            UpdateCrosshair(mousePosition);
            playerBounds.X = (int)pos.X - 8;
            playerBounds.Y = (int)pos.Y - 8;
        }



        private void UpdateCrosshair(Vector2 mousePosition)
        {

        }

        public void Draw(SpriteBatch spriteBatch, Matrix matrix, Matrix transformMatrix)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                samplerState: SamplerState.PointClamp,
                effect: null,
                blendState: null,
                rasterizerState: null,
                depthStencilState: null,
                transformMatrix: transformMatrix);
            if (isIdle)
                spriteBatch.Draw(playerSprite[0], pos, 0, new Vector2(3, 3));
            else if (isAttacking)
                spriteBatch.Draw(playerSprite[2], pos, 0, new Vector2(3, 3));

            else
                spriteBatch.Draw(playerSprite[1], pos, 0, new Vector2(3, 3));
            spriteBatch.End();
        }
    }
}