using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;
using TiledSharp;
using barArcadeGame._Managers;
using System.Collections;
using System.Threading;
using System.Timers;
using Newtonsoft.Json.Linq;
using System.Windows;

namespace barArcadeGame.Model
{
    internal class Bob
    {
        public Vector2 pos;
        private AnimatedSprite[] playerSprite;
        private SpriteSheet sheet;
        public Rectangle playerBounds;
        public bool isIdle = false;
        public bool isPanic = false;
        public bool isPlayed = false;
        public bool isStopped = false;
        public string animation = "";

        public Bob()
        {

            playerSprite = new AnimatedSprite[2];
            pos = new Vector2(200, 50);
            playerBounds = new Rectangle((int)pos.X + 35, (int)pos.Y + 10, 30, 80);
           // playerBounds = new Rectangle((int)pos.X - 8, (int)pos.Y - 8, 16, 17);
            isStopped = false;
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

        public void Update(GameTime gameTime)
        {

            if (!isStopped)
            {
                
                isPanic = true;
                playerSprite[0].Play("bobIdle");
                playerSprite[0].Update(gameTime);
            }
            else
            {

                isPanic = false;
                playerSprite[0].Play("bobForward");
                playerSprite[0].Update(gameTime);
            }

            playerBounds.X = (int)pos.X - 8;
            playerBounds.Y = (int)pos.Y - 8;
        }

        public void Stop()
        {
            isStopped = true;
            //Handle Collision animation
        }
        public void Panic()
        {
            isStopped = false;
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
            Vector2 scale = new Vector2(2.5f, 2.5f);
            spriteBatch.Draw(playerSprite[0], pos, 0, scale);
            spriteBatch.End();
        }


    }
}
