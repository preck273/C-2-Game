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
    public static class CoinManager
    {
        private static float _firstRoundTime;
        private static float _turnTime;
        public static float TurnTimeLeft { get; private set; }
        private static Texture2D _texture;
        private static Texture2D _textureCoin;
        private static Rectangle _rectangle;
        public static int Score { get; private set; }
        public static bool Active { get; private set; }
        private const string _fileName = "highscores.dat";
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
            //Label.SetText("This is a label.");


            Console.WriteLine(Label.Text);

            _texture = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { new(200, 80, 30) });
            _rectangle = new(0, 0, Globals.Bounds.X, 80);
            LoadCoins();
        }

        public static void UpdateCoinsLabel()
        {
            //Label.SetText(Label.ToString());
        }

        public static void DrawCoins()
        {
            //Label.Draw(Coins.ToString());
        }

        public static void LoadCoins()
        {
            if (File.Exists(_fileName))
            {
                using BinaryReader binaryReader = new(File.Open(_fileName, FileMode.Open));

                Coins = binaryReader.ReadInt32();
                binaryReader.Close();
            }

            UpdateCoinsLabel();
        }

        public static void SaveCoins()
        {
            UpdateCoinsLabel();

            using BinaryWriter binaryWriter = new(File.Create(_fileName));

            binaryWriter.Write(Coins);
            binaryWriter.Close();
        }

        public static void Start()
        {
            Active = true;
        }

        public static void Stop()
        {
            Active = false;
        }

        public static void Reset()
        {
            _turnTime = _firstRoundTime;
            TurnTimeLeft = _turnTime;
            Coins = 0;
            Active = false;
            _rectangle.Width = Globals.Bounds.X;
        }

        public static void NextTurn()
        {
            Coins += (int)Math.Round(10 * TurnTimeLeft);
            _turnTime--;
            TurnTimeLeft = _turnTime;
        }

        public static void Miss()
        {
            Coins -= 10;
        }

        public static void Update()
        {
            Coins = database.GetCoinValue();
            //TurnTimeLeft -= Globals.Time;
            //_rectangle.Width = (int)(Globals.Bounds.X * TurnTimeLeft / _turnTime);
        }

        public static void Draw()
        {
            //Globals.SpriteBatch.Begin();
            Label.SetText(Coins.ToString());
            Label.Draw();
            Globals.SpriteBatch.Draw(_textureCoin, new(0, Globals.Bounds.Y - 40), null, Color.White * 0.75f, 0f, Vector2.Zero, 0.1f, SpriteEffects.None, 1f);
            //Globals.SpriteBatch.Draw(_texture, _rectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
    }
}

