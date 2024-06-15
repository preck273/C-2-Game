using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;
//using System.Windows.Threading;
using System.Media;
using Newtonsoft.Json;
using barArcadeGame.Model;

using Label = barArcadeGame.Model.Label;

namespace barArcadeGame._Managers
{
    public class DialogueBobOutsideController
    {
        public static Button NextBtn { get; private set; }
        public static Button ExitBtn { get; private set; }
        private static Texture2D textureBox;

        private static Rectangle rectangle;
        public static List<Label> data = new List<Label>();
        public List<string> stringList;
        public static Label displayText;
        public static int count;

        private List<Speech> questions;
        private int currentQuestionIndex;
        private int score;
        private SpriteFont font;

        private Texture2D checkboxChecked;
        private Texture2D checkboxUnchecked;

        public int selectedOption;

        public bool displayQuestions;
        public bool initDone = false;
        public bool waveDone = false;
        public bool waveStart = false;


        public List<string> answersSelected;

        //List<string> value1
        public List<string> result = new List<string> { "coffee", "lemon joice", "red tea" };
        public List<string> choice1 = new List<string> { "Speed", "Defence", "Nothing" };
        public List<string> choice2 = new List<string> { "Agility", "Damage", "Nothing" };
        public List<string> choice3 = new List<string> { "Luck", "Reinforce", "Nothing" };

        //Add sounds for each power up 
        // Final day will never come but make it on a randomiser/ nothing happened


        //Make questions based on scene
        //Each Scene has a wait period at night where the player can buy stuff. The door will take the player to next scene by sleeping to call it
        //Load day anim
        //Load night anim
        public void Init()
        {
            if (!initDone)
            {
                answersSelected = new List<string>();
                displayQuestions = true;

                 questions = new List<Speech>
             {
                 new Speech("The world is ending and all of earth's DEFENDERS have been wiped out!", new List<string> {"Wiped out? I'm one of them?"}),
                 new Speech("Prove it buy killing the next wave of invaders and I'll aid you in your journey.", new List<string> {}),
                 new Speech("I'll be hiding in the house. Come see me if you survive.", new List<string> {})
             };


                 checkboxChecked = Globals.Content.Load<Texture2D>("picture/checked");
                 checkboxUnchecked = Globals.Content.Load<Texture2D>("picture/unchecked");

                 currentQuestionIndex = 0;
                 score = 0;

                NextBtn = new(Globals.Content.Load<Texture2D>("picture/next"), new(Globals.Bounds.X - 20, 60));
                NextBtn.setScale(new(1, 1));
                NextBtn.OnClick += ClickNext;
                ExitBtn = new(Globals.Content.Load<Texture2D>("picture/exit"), new(Globals.Bounds.X - 20, 20));
                ExitBtn.setScale(new(1, 1));
                ExitBtn.OnClick += ClickExit;

                 count = 0;

                var font = Globals.Content.Load<SpriteFont>("Font/defaultFont");

                 displayText = new(font, new(Globals.Bounds.X / 2, 40));

                 textureBox = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
                 textureBox.SetData(new Color[] { Color.White });
                 rectangle = new(0, 0, Globals.Bounds.X, 140);
            }
            
        }
        
        public void BuyDialogue(int level)
        {
            answersSelected = new List<string>();
            displayQuestions = true;
            initDone = false;

             questions = new List<Speech>
            {
                new Speech("Welcome to the Bar, click next to order drinks and foods", new List<string> { }),
                new Speech("I am the bar tender Jack, nice to meet you. Are you happy today?", new List<string> { "Yes I am", "No I am not"}),
                new Speech("What would you like to do?", new List<string> { "Order drinks", "Order foods"}),

                new Speech("What food you would like to order?", new List<string> { "Bread", "Soup", "Pasta" }),
                new Speech("Which drink you would like to choose?", result),
                new Speech("Here you are the food, enjoy!", new List<string> {  }),
                new Speech("Here you are the drink, enjoy!", new List<string> {  }),
            };



             checkboxChecked = Globals.Content.Load<Texture2D>("picture/checked");
             checkboxUnchecked = Globals.Content.Load<Texture2D>("picture/unchecked");

             currentQuestionIndex = 0;
             score = 0;

            NextBtn = new(Globals.Content.Load<Texture2D>("picture/next"), new(Globals.Bounds.X - 20, 60));
            NextBtn.setScale(new(1, 1));
            NextBtn.OnClick += ClickNext;
            ExitBtn = new(Globals.Content.Load<Texture2D>("picture/exit"), new(Globals.Bounds.X - 20, 20));
            ExitBtn.setScale(new(1, 1));
            ExitBtn.OnClick += ClickExit;

             count = 0;

            var font = Globals.Content.Load<SpriteFont>("Font/defaultFont");

             displayText = new(font, new(Globals.Bounds.X / 2, 40));

             textureBox = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
             textureBox.SetData(new Color[] { Color.White });
             rectangle = new(0, 0, Globals.Bounds.X, 140);
        }

        private void initCheck()
        {
            if (displayQuestions && !initDone)
            {
                currentQuestionIndex++;
            }

            if
                ( currentQuestionIndex >=  questions.Count)
            {
                initDone = true;
                HideAllSpritesAndTextures();
            }
            
            //waveStart = true;
        }


        private void CheckAnswer(int selectedOption)
        {
            if (displayQuestions)
            {
                if ( questions[ currentQuestionIndex].Options.Count != 0)
                {
                    answersSelected.Add( questions[ currentQuestionIndex].Options[ selectedOption]);

                    if ( questions[ currentQuestionIndex].Options[ selectedOption].Equals("Order foods"))
                    {
                         currentQuestionIndex = 3;
                    }
                    else if ( questions[ currentQuestionIndex].Options[ selectedOption].Equals("Order drinks"))
                    {
                         currentQuestionIndex = 4;
                    }
                    else
                    {
                        if ( currentQuestionIndex == 3)
                        {
                             currentQuestionIndex = 5;
                        }
                        else if ( currentQuestionIndex == 4)
                        {
                             currentQuestionIndex = 6;
                            database.RemoveCoinValue();
                        }
                        else
                        {
                             currentQuestionIndex++;
                        }
                    }
                }
                else
                {
                    if ( currentQuestionIndex == 5)
                    {
                         currentQuestionIndex = 7;
                    }
                     currentQuestionIndex++;
                }
            }
            if ( currentQuestionIndex >=  questions.Count)
            {
                HideAllSpritesAndTextures();
            }
        }

        public void HideAllSpritesAndTextures()
        {
             displayText.SetText("");
            foreach (string str in answersSelected)
            {
                 displayText.SetText( displayText.Text + " | " + str);
            }

            NextBtn.setScale(new(0, 0));
            ExitBtn.setScale(new(0, 0));
             rectangle = new(0, 0, 0, 0);
            displayQuestions = false;
        }

        public void ClickNext(object sender, EventArgs e)
        {
            if (displayQuestions && !initDone)
            {
                initCheck();
            }
            if (displayQuestions && initDone)
            {
                CheckAnswer(selectedOption);
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
            ExitBtn.Update();
            NextBtn.Update();

            if (displayQuestions)
            {
                for (int i = 0; i <  questions[ currentQuestionIndex].Options.Count; i++)
                {
                    var optionRectangle = new Rectangle(Globals.Bounds.X / 2 - 25, 40 + i * 30, 20, 20);

                    if (InputManager.MouseClicked && optionRectangle.Contains(InputManager.MouseRectangle))
                    {
                         selectedOption = i;
                    }
                }
            }
        }

        public void Draw()
        {
            font = Globals.Content.Load<SpriteFont>("Font/defaultFont");
            Globals.SpriteBatch.Draw( textureBox,  rectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            NextBtn.Draw();
            ExitBtn.Draw();

            if (displayQuestions)
            {
                var currentQuestion =  questions[ currentQuestionIndex];
                Globals.SpriteBatch.DrawString(font, currentQuestion.QuestionText, new Vector2(40, 20), Color.Black);

                var optionPosition = new Vector2(Globals.Bounds.X / 2, 40);
                for (int i = 0; i < currentQuestion.Options.Count; i++)
                {
                    var checkboxRectangle = new Rectangle(Globals.Bounds.X / 2 - 25, 40 + i * 30, 20, 20);
                    var checkboxTexture = i ==  selectedOption ?  checkboxChecked :  checkboxUnchecked;
                    Globals.SpriteBatch.Draw(checkboxTexture, checkboxRectangle, Color.White);

                    var optionText = $"{i + 1}. {currentQuestion.Options[i]}";

                    Globals.SpriteBatch.DrawString(font, optionText, optionPosition, Color.Black);
                    optionPosition.Y += 30;
                }
            }
        }

      

    }
}
