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
    public class GameController
    {
        public static bool IsMickDialogueRun;
        public static bool IsBobDialogueRun;
        public static bool IsBobDialogueDone = false;
        public Matrix _translation;
        //DialogueManager diaMick;
        DialogueBobOutsideController diaBob;
        private readonly List<Button> _buttons = new();

        public GameController()
        {
            SoundController.Init();
            CoinController.Init(Globals.Content.Load<Texture2D>("picture/coin"));
          
            IsBobDialogueRun = false;
            diaBob = new DialogueBobOutsideController();
            //diaMick = new DialogueManager();
            List<string> list = new();
            AddButton(SoundController.MusicBtn);
            AddButton(SoundController.SoundBtn);
        }

        private Button AddButton(Button button)
        {
            _buttons.Add(button);
            return button;
        }


        public void runBobDialogue()
        {
            if (!IsBobDialogueRun)
            {
                diaBob.Init();

                IsBobDialogueRun = true;
            }
        }
        
        public bool GetBobDialogueRun()
        {
            return diaBob.initDone;
            
        }

        public void hideBobDialogue()
        {
            if (IsBobDialogueRun)
            {
                diaBob.HideAllSpritesAndTextures();
                IsBobDialogueRun = false;
            }
        }  

        public void Update()
        {
            foreach (var button in _buttons)
            {
                button.Update();
            }

            if (IsBobDialogueRun)
            {
                diaBob.Update();
            }
      
            CoinController.Update();
            
            InputController.Update();
        }

        public void Draw()
        {
            Globals.SpriteBatch.Begin();
            CoinController.Draw();
            Globals.SpriteBatch.End();

            Globals.SpriteBatch.Begin();
            foreach (var button in _buttons)
            {
                button.Draw();
            }
            Globals.SpriteBatch.End();

            Globals.SpriteBatch.Begin();
            if (IsBobDialogueRun)
            {
                diaBob.Draw();
            }

            Globals.SpriteBatch.End();
        }
    }
}
