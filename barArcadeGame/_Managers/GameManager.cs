using barArcadeGame.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace barArcadeGame._Managers
{
    public class GameManager
    {
        public static bool IsMickDialogueRun;
        public static bool IsJackDialogueRun;
        public Matrix _translation;
        DialogueManager diaMick;
        DialogueJackManager diaJack;
        private readonly List<Button> _buttons = new();
        //public static Button NextBtn { get; private set; }
        //public static Button ExitBtn { get; private set; }

        public GameManager()
        {
            SoundManager.Init();
            CoinManager.Init(Globals.Content.Load<Texture2D>("picture/coin"));
          
            IsJackDialogueRun = false;
            IsMickDialogueRun = false;
            diaJack = new DialogueJackManager();
            diaMick = new DialogueManager();
            List<string> list = new();
            AddButton(SoundManager.MusicBtn);
            AddButton(SoundManager.SoundBtn);
        }

        private Button AddButton(Button button)
        {
            _buttons.Add(button);
            return button;
        }


        public void runJackDialogue()
        {
            if (!IsJackDialogueRun)
            {
                diaJack.Init();

                IsJackDialogueRun = true;
            }
        }

        public void hideJackDialogue()
        {
            if (IsJackDialogueRun)
            {
                diaJack.HideAllSpritesAndTextures();
                IsJackDialogueRun = false;
            }
        }  

        public void Update()
        {
            foreach (var button in _buttons)
            {
                button.Update();
            }

            if (IsJackDialogueRun)
            {
                diaJack.Update();
            }

            if (IsMickDialogueRun)
            {
                diaMick.Update();
            }
      
            CoinManager.Update();
            
            InputManager.Update();
        }

        public void Draw()
        {
            Globals.SpriteBatch.Begin();
            CoinManager.Draw();
            Globals.SpriteBatch.End();

            Globals.SpriteBatch.Begin();
            foreach (var button in _buttons)
            {
                button.Draw();
            }
            Globals.SpriteBatch.End();

            Globals.SpriteBatch.Begin();
            if (IsJackDialogueRun)
            {
                diaJack.Draw();
            }

            if (IsMickDialogueRun)
            {
                diaMick.Draw();
            }
            Globals.SpriteBatch.End();
        }
    }
}
