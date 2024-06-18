using barArcadeGame._Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace barArcadeGame.Model
{
    public class Button 
    {
        private readonly Rectangle _rectangle;
        public Vector2 Position { get; set; }
        protected Vector2 origin;
        public Texture2D Texture { get; protected set; }
        protected Vector2 scale;
        protected Color color;
        public float Rotation { get; set; }
        public bool Disabled { get; set; }

        public Button(Texture2D tex, Vector2 pos) 
        {
            Texture = tex;
            Position = pos;
            origin = new(tex.Width / 2, tex.Height / 2);
            scale = Vector2.One;
            color = Color.White;
            _rectangle = new((int)(pos.X - origin.X), (int)(pos.Y - origin.Y), tex.Width, tex.Height);
        }
        public void setScale(Vector2 value)
        {
            scale = value;
        }

        public event EventHandler OnClick;

        public void Update()
        {
            color = Color.White;

            if (_rectangle.Contains(InputController.MouseRectangle))
            {
                color = Color.DarkGray;

                if (InputController.MouseClicked)
                {
                    OnClick?.Invoke(this, EventArgs.Empty);
                }
            }

            if (Disabled) color *= 0.3f;
        }

        public virtual void Draw()
        {
            Globals.SpriteBatch.Draw(Texture, Position, null, color, 0f, origin, scale, SpriteEffects.None, 1f);
        }
    }
}