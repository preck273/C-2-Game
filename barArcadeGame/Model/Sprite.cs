using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace barArcadeGame.Model
{
    public class Sprite
    {
        public Vector2 Position { get; set; }
        protected Vector2 origin;
        public Texture2D Texture { get; protected set; }
        protected Vector2 scale;
        protected Color color;
        public float Rotation { get; set; }

        public Sprite(Texture2D tex, Vector2 pos)
        {
            Texture = tex;
            Position = pos;
            origin = new(tex.Width / 2, tex.Height / 2);
            scale = Vector2.One;
            color = Color.White;
        }

        public void setScale(Vector2 value)
        {
            scale = value;
        }

        public virtual void Draw()
        {
            Globals.SpriteBatch.Draw(Texture, Position, null, color, 0f, origin, scale, SpriteEffects.None, 1f);
        }
    }
}





