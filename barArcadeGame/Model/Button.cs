﻿using barArcadeGame._Managers;
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
    public class Button : Sprite
    {
        private readonly Rectangle _rectangle;
        public bool Disabled { get; set; }

        public Button(Texture2D tex, Vector2 pos) : base(tex, pos)
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

            if (Disabled) color *= 0.3f;
        }
    }
}