using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace barArcadeGame.Model
{
    public class Label
    {
        private readonly SpriteFont _font;
        private Vector2 _centerPos;
        private Vector2 _pos;
        public string Text { get; set; }

        public Label(SpriteFont font, Vector2 position)
        {
            _font = font;
            _centerPos = position;
        }

        public Label() { }

        public void SetText(string text)
        {
            Text = text;
            _pos = new(_centerPos.X - _font.MeasureString(Text).X / 2 + 3, _centerPos.Y);
        }

        public void Draw()
        {
            Globals.SpriteBatch.DrawString(_font, Text, _pos, Color.White);
        }
    }
}
