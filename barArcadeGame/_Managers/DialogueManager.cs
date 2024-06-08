using barArcadeGame.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace barArcadeGame._Managers
{
    public class DialogueManager
    {
        public static Button NextBtn { get; private set; }
        public static Button ExitBtn { get; private set; }
        private static Texture2D _textureBox;
        private static Rectangle _rectangle;
        //public static List<Label> _data;
        public static List<Label> _data = new List<Label>();
        public List<string> stringList;
        public static Label _displayText;
        public static int _count;

        //List<string> value1
        public void Init()
        {
            NextBtn = new(Globals.Content.Load<Texture2D>("picture/next"), new(Globals.Bounds.X - 20, 60));
            NextBtn.setScale(new(1, 1));
            NextBtn.OnClick += ClickNext;
            ExitBtn = new(Globals.Content.Load<Texture2D>("picture/exit"), new(Globals.Bounds.X - 20, 20));
            ExitBtn.setScale(new(1, 1));
            ExitBtn.OnClick += ClickExit;

            _count = 0;

            var font = Globals.Content.Load<SpriteFont>("Font/defaultFont");

            addText(font);
            _displayText = new(font, new(Globals.Bounds.X / 2, 30));
            _displayText.SetText("Hello, click next button to continue!");

            _textureBox = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            _textureBox.SetData(new Color[] { new(10, 80, 30) });
            _rectangle = new(0, 0, Globals.Bounds.X, 80);
        }

        public void HideAllSpritesAndTextures()
        {
            _displayText.SetText("");
            NextBtn.setScale(new(0, 0));
            ExitBtn.setScale(new(0, 0));
            _rectangle = new(0, 0, 0, 0);
        }

        public static void ClickNext(object sender, EventArgs e)
        {
            if (_count < _data.Count - 1)
            {
                _count++;
            }

            _displayText.SetText(_data.ElementAt(_count).Text);
        }

        public void addText(SpriteFont font)
        {
            stringList = new List<string>()
            {
                "Hey, I'm ninja",
                "You can play some cool games here",
                "My favourite is the quiz game"
            };

            Console.WriteLine(stringList);
            //_data.Clear();
            foreach (string str in stringList)
            {
                Label newLabel = new Label(font, new(Globals.Bounds.X / 2, 70));
                newLabel.SetText(str);
                _data.Add(newLabel);
            }
        }

        public void ClickExit(object sender, EventArgs e)
        {
            DialogueFinished();
        }

        public void DialogueFinished()
        {
            HideAllSpritesAndTextures();
        }

        public void Update()
        {
            NextBtn.Update();
            ExitBtn.Update();
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_textureBox, _rectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            NextBtn.Draw();
            ExitBtn.Draw();
            _displayText.Draw();
        }
    }
}
