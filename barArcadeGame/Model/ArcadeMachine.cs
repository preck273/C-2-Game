using barArcadeGame._Managers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace barArcadeGame.Model
{
    public class ArcadeMachine : Sprite
    {
        public readonly Rectangle _rectangle;

        public ArcadeMachine(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
            _rectangle = new((int)(pos.X - origin.X), (int)(pos.Y - origin.Y), tex.Width, tex.Height);
        }

        public event EventHandler OnClick;

        public void Update()
        {
            color = Color.White;

            if (_rectangle.Contains(InputManager.MouseRectangle))
            {
                color = Color.DarkGray;

                if (InputManager.MouseClicked)
                {
                    OnClick?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Matrix matrix, Matrix transformMatrix)
        {
            // SpriteBatch spriteBatch, Matrix matrix, Matrix transformMatrix
            spriteBatch.Begin(//All of these need to be here :(
                SpriteSortMode.Deferred,
                samplerState: SamplerState.PointClamp,
                effect: null,
                blendState: null,
                rasterizerState: null,
                depthStencilState: null,
                transformMatrix: transformMatrix);/*<-This is the main thing*/

            Color rectangleColor = Color.Red; //The color of the example rectangle
            Globals.SpriteBatch.DrawRectangle(_rectangle, rectangleColor);
            Globals.SpriteBatch.Draw(Texture, Position, null, color, 0f, origin, scale, SpriteEffects.None, 1f);
            spriteBatch.End();
        }
    }
}


