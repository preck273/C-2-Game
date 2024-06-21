using barArcadeGame.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace barArcadeGame.Managers
{
    public static class CoinController
    {
        private static Texture2D _coinTexture;
        private static Texture2D _backgroundTexture;
        private static Rectangle _backgroundRectangle;
        public static int Coins { get; private set; }
        public static Label CoinLabel { get; private set; }

        public static void Init(Texture2D coinTexture)
        {
            _coinTexture = coinTexture;
            var font = Globals.Content.Load<SpriteFont>("Font/defaultFont");
            var screenHeight = Globals.Bounds.Y;
            var screenWidth = Globals.Bounds.X;

            Coins = 0;
            CoinLabel = new Label(font, new Vector2(37, screenHeight - 32));

            _backgroundTexture = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { new Color(200, 80, 30) });
            _backgroundRectangle = new Rectangle(0, 0, screenWidth, 80);
        }

        public static void Update()
        {
            Coins = DatabaseController.GetCoinValue();
        }

        public static void Draw()
        {
            CoinLabel.SetText(Coins.ToString());
            CoinLabel.Draw();
            Globals.SpriteBatch.Draw(
                _coinTexture,
                new Vector2(0, Globals.Bounds.Y - 40),
                null,
                Color.White * 0.75f,
                0f,
                Vector2.Zero,
                0.1f,
                SpriteEffects.None,
                1f
            );
        }
    }
}
