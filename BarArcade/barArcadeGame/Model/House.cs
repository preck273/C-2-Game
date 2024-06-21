using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace barArcadeGame.Model
{
    internal class House
    {

        public Vector2 _position;
        private readonly AnimationController _anims = new();
        public Rectangle bounds;
        public bool touch;

        public House(Vector2 pos, bool anim)
        {
            touch = false;
            var doorOpen = Globals.Content.Load<Texture2D>("picture/door");
            var doorClosed = Globals.Content.Load<Texture2D>("picture/doorClosed");
            _position = pos;
            bounds = new Rectangle((int)_position.X, (int)_position.Y, 60, 100);
           
            _anims.AddAnimation(1, new(doorOpen, 7, 2, 0.2f, 1));
            _anims.AddAnimation(2, new(doorOpen, 7, 2, 2000f, 2));
           
        }


        public void Update()
        {
            if (!touch)
                {
                    _anims.Update(2);
            }
            if (touch)
            {
                    _anims.Update(1);
             }
            
           
        }
        public void UpdateFight()
        {
            if (!touch)
            {
                _anims.Update(2);
            }
            if (touch)
            {
                _anims.Update(2);
            }


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

            _anims.Draw(_position);
            Color rectangleColor = Color.Red;
            Globals.SpriteBatch.DrawRectangle(bounds, rectangleColor);
            spriteBatch.End();
        }
    }
}

