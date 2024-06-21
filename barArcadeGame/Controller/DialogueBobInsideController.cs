using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using barArcadeGame.Model;
using Label = barArcadeGame.Model.Label;

namespace barArcadeGame.Managers
{
    public class DialogueBobInsideController
    {
        public static Button NextBtn { get; private set; }
        public static Button ExitBtn { get; private set; }
        private static Texture2D _textureBox;
        private static Rectangle _rectangle;
        public static List<Label> Data = new List<Label>();
        public static Label DisplayText;
        public static int Count;

        private List<Speech> _questions;
        private int _currentQuestionIndex;
        private int _score;
        private SpriteFont _font;
        private Texture2D _checkboxChecked;
        private Texture2D _checkboxUnchecked;
        private int _selectedOption;
        private bool _displayQuestions;
        private bool _initDone;
        private bool _waveDone;
        private bool _waveStart;
        private List<string> _answersSelected;

        private readonly List<string> _result = new List<string> { "coffee", "lemon juice", "red tea" };
        private readonly List<string> _choice1 = new List<string> { "Speed", "Defense", "Nothing" };
        private readonly List<string> _choice2 = new List<string> { "Agility", "Damage", "Nothing" };
        private readonly List<string> _choice3 = new List<string> { "Luck", "Reinforce", "Nothing" };

        public void Init(int level)
        {
            if (!Initialize(level))
                return;

            _checkboxChecked = Globals.Content.Load<Texture2D>("picture/checked");
            _checkboxUnchecked = Globals.Content.Load<Texture2D>("picture/unchecked");

            _currentQuestionIndex = 0;
            _score = 0;

            InitializeButtons();
            InitializeDisplayText();
            InitializeTextureBox();
        }

        private bool Initialize(int level)
        {
            if (level == 0 && !_initDone)
            {
                InitializeQuestionsForLevel0();
                _initDone = true;
                return true;
            }
            if (level == 1)
            {
                InitializeQuestionsForLevel1();
                _initDone = false;
                return true;
            }
            return false;
        }

        private void InitializeQuestionsForLevel0()
        {
            _answersSelected = new List<string>();
            _displayQuestions = true;
            _questions = new List<Speech>
            {
                new Speech("The world is ending and all of earth's DEFENDERS have been wiped out!", new List<string> {"Wiped out? I'm one of them?"}),
                new Speech("Prove it by killing the next wave of invaders and I'll aid you in your journey.", new List<string> {}),
                new Speech("I'll be hiding in the house. Come see me if you survive.", new List<string> {})
            };
        }

        private void InitializeQuestionsForLevel1()
        {
            _answersSelected = new List<string>();
            _displayQuestions = true;
            _questions = new List<Speech>
            {
                new Speech("Welcome to the Bar, click next to order drinks and foods", new List<string> { }),
                new Speech("I am the bar tender Jack, nice to meet you. Are you happy today?", new List<string> { "Yes I am", "No I am not"}),
                new Speech("What would you like to do?", new List<string> { "Order drinks", "Order foods"}),
                new Speech("What food you would like to order?", new List<string> { "Bread", "Soup", "Pasta" }),
                new Speech("Which drink would you like to choose?", _result),
                new Speech("Here you are the food, enjoy!", new List<string> { }),
                new Speech("Here you are the drink, enjoy!", new List<string> { }),
            };
        }

        private void InitializeButtons()
        {
            NextBtn = new Button(Globals.Content.Load<Texture2D>("picture/next"), new Vector2(Globals.Bounds.X - 20, 60))
            {
                Scale = new Vector2(1, 1)
            };
            NextBtn.OnClick += ClickNext;

            ExitBtn = new Button(Globals.Content.Load<Texture2D>("picture/exit"), new Vector2(Globals.Bounds.X - 20, 20))
            {
                Scale = new Vector2(1, 1)
            };
            ExitBtn.OnClick += ClickExit;
        }

        private void InitializeDisplayText()
        {
            _font = Globals.Content.Load<SpriteFont>("Font/defaultFont");
            DisplayText = new Label(_font, new Vector2(Globals.Bounds.X / 2, 40));
        }

        private void InitializeTextureBox()
        {
            _textureBox = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            _textureBox.SetData(new[] { Color.White });
            _rectangle = new Rectangle(0, 0, Globals.Bounds.X, 140);
        }

        private void InitCheck()
        {
            if (_displayQuestions && !_initDone)
            {
                if (_questions[_currentQuestionIndex].Options.Count != 0)
                {
                    // Additional initialization logic if needed
                }
            }

            if (_currentQuestionIndex >= _questions.Count)
            {
                HideAllSpritesAndTextures();
            }

            _initDone = true;
            _waveStart = true;
        }

        private void CheckAnswer(int selectedOption)
        {
            if (_displayQuestions)
            {
                if (_questions[_currentQuestionIndex].Options.Count != 0)
                {
                    _answersSelected.Add(_questions[_currentQuestionIndex].Options[selectedOption]);

                    if (_questions[_currentQuestionIndex].Options[selectedOption].Equals("Order foods"))
                    {
                        _currentQuestionIndex = 3;
                    }
                    else if (_questions[_currentQuestionIndex].Options[selectedOption].Equals("Order drinks"))
                    {
                        _currentQuestionIndex = 4;
                    }
                    else
                    {
                        _currentQuestionIndex = _currentQuestionIndex == 3 ? 5 : _currentQuestionIndex == 4 ? 6 : _currentQuestionIndex + 1;
                        if (_currentQuestionIndex == 6)
                        {
                            DatabaseController.RemoveCoinValue();
                        }
                    }
                }
                else
                {
                    _currentQuestionIndex++;
                }
            }

            if (_currentQuestionIndex >= _questions.Count)
            {
                HideAllSpritesAndTextures();
            }
        }

        public void HideAllSpritesAndTextures()
        {
            DisplayText.SetText("");
            foreach (string str in _answersSelected)
            {
                DisplayText.SetText(DisplayText.Text + " | " + str);
            }

            NextBtn.Scale = new Vector2(0, 0);
            ExitBtn.Scale = new Vector2(0, 0);
            _rectangle = new Rectangle(0, 0, 0, 0);
            _displayQuestions = false;
        }

        public void ClickNext(object sender, EventArgs e)
        {
            if (_displayQuestions && !_initDone)
            {
                InitCheck();
            }
            if (_displayQuestions && _initDone)
            {
                CheckAnswer(_selectedOption);
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

            if (_displayQuestions)
            {
                for (int i = 0; i < _questions[_currentQuestionIndex].Options.Count; i++)
                {
                    var optionRectangle = new Rectangle(Globals.Bounds.X / 2 - 25, 40 + i * 30, 20, 20);

                    if (InputController.MouseClicked && optionRectangle.Contains(InputController.MouseRectangle))
                    {
                        _selectedOption = i;
                    }
                }
            }
        }

        public void Draw()
        {
            _font = Globals.Content.Load<SpriteFont>("Font/defaultFont");
            Globals.SpriteBatch.Draw(_textureBox, _rectangle, Color.White);

            NextBtn.Draw();
            ExitBtn.Draw();

            if (_displayQuestions)
            {
                var currentQuestion = _questions[_currentQuestionIndex];
                Globals.SpriteBatch.DrawString(_font, currentQuestion.QuestionText, new Vector2(40, 20), Color.Black);

                var optionPosition = new Vector2(Globals.Bounds.X / 2, 40);
                for (int i = 0; i < currentQuestion.Options.Count; i++)
                {
                    var checkboxRectangle = new Rectangle(Globals.Bounds.X / 2 - 25, 40 + i * 30, 20, 20);
                    var checkboxTexture = i == _selectedOption ? _checkboxChecked : _checkboxUnchecked;
                    Globals.SpriteBatch.Draw(checkboxTexture, checkboxRectangle, Color.White);

                    var optionText = $"{i + 1}. {currentQuestion.Options[i]}";
                    Globals.SpriteBatch.DrawString(_font, optionText, optionPosition, Color.Black);
                    optionPosition.Y += 30;
                }
            }
        }
    }
}
