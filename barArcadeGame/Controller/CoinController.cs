using barArcadeGame.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace barArcadeGame._Managers
{
    public static class CoinController
    {
        private static Texture2D _texture;
        private static Texture2D _textureCoin;
        private static Rectangle _rectangle;
        public static int Coins { get; set; } = new();
        public static Label Label { get; set; }

        public static void Init(Texture2D tex)
        {
            _textureCoin = tex;
            var font = Globals.Content.Load<SpriteFont>("Font/defaultFont");
            var y = Globals.Bounds.Y / 2;
            var x = Globals.Bounds.X / 2;

            Coins = 0;

            Label = new(font, new(37, Globals.Bounds.Y - 32));

            _texture = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { new(200, 80, 30) });
            _rectangle = new(0, 0, Globals.Bounds.X, 80);
        }

        public static void Update()
        {
            Coins = databaseController.GetCoinValue();
            
        }

        public static void Draw()
        {
            Label.SetText(Coins.ToString());
            Label.Draw();
            Globals.SpriteBatch.Draw(_textureCoin, new(0, Globals.Bounds.Y - 40), null, Color.White * 0.75f, 0f, Vector2.Zero, 0.1f, SpriteEffects.None, 1f);
        }
    }
}

