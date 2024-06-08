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


    internal class DialogueBobManager
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

        private List<Speech> _questions;
        private int _currentQuestionIndex;
        private int _score;
        private SpriteFont font;

        private Texture2D _checkboxChecked;
        private Texture2D _checkboxUnchecked;

        public int _selectedOption;

        public bool displayQuestions;

        public List<string> answersSelected;

        //List<string> value1
        public List<string> result = new List<string> { "coffee", "lemon joice", "red tea" };

        public void Init()
        {
            GetDrinks();
            answersSelected = new List<string>();
            displayQuestions = true;
            // Initialize your quiz questions
            _questions = new List<Speech>
            {
                new Speech("Welcome to the Bar, click next to order drinks and foods", new List<string> { }),
                new Speech("I am the bar tender Jack, nice to meet you. Are you happy today?", new List<string> { "Yes I am", "No I am not"}),
                new Speech("What would you like to do?", new List<string> { "Order drinks", "Order foods"}),

                new Speech("What food you would like to order?", new List<string> { "Bread", "Soup", "Pasta" }),
                new Speech("Which drink you would like to choose?", result),
                new Speech("Here you are the food, enjoy!", new List<string> {  }),
                new Speech("Here you are the drink, enjoy!", new List<string> {  }),
            };

            _checkboxChecked = Globals.Content.Load<Texture2D>("picture/checked");
            _checkboxUnchecked = Globals.Content.Load<Texture2D>("picture/unchecked");

            _currentQuestionIndex = 0;
            _score = 0;
            NextBtn = new(Globals.Content.Load<Texture2D>("picture/next"), new(Globals.Bounds.X - 20, 60));
            NextBtn.setScale(new(1, 1));
            NextBtn.OnClick += ClickNext;
            ExitBtn = new(Globals.Content.Load<Texture2D>("picture/exit"), new(Globals.Bounds.X - 20, 20));
            ExitBtn.setScale(new(1, 1));
            ExitBtn.OnClick += ClickExit;

            _count = 0;

            var font = Globals.Content.Load<SpriteFont>("Font/defaultFont");

            _displayText = new(font, new(Globals.Bounds.X / 2, 40));

            _textureBox = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            _textureBox.SetData(new Color[] { new(10, 80, 30) });
            _rectangle = new(600, 600, Globals.Bounds.X, 140);
        }

        public void HideAllSpritesAndTextures()
        {
            _displayText.SetText("");
            foreach (string str in answersSelected)
            {
                _displayText.SetText(_displayText.Text + " | " + str);
            }

            NextBtn.setScale(new(0, 0));
            ExitBtn.setScale(new(0, 0));
            _rectangle = new(0, 0, 0, 0);
            displayQuestions = false;
        }

        public void ClickNext(object sender, EventArgs e)
        {
            CheckAnswer(_selectedOption);
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
                for (int i = 0; i < _questions[_currentQuestionIndex].Options.Count; i++)
                {
                    var optionRectangle = new Rectangle(Globals.Bounds.X / 2 - 25, 40 + i * 30, 20, 20);

                    if (InputManager.MouseClicked && optionRectangle.Contains(InputManager.MouseRectangle))
                    {
                        _selectedOption = i;
                    }
                }
            }
        }

        public void Draw()
        {
            font = Globals.Content.Load<SpriteFont>("Font/defaultFont");
            Globals.SpriteBatch.Draw(_textureBox, _rectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            NextBtn.Draw();
            ExitBtn.Draw();

            if (displayQuestions)
            {
                var currentQuestion = _questions[_currentQuestionIndex];
                Globals.SpriteBatch.DrawString(font, currentQuestion.QuestionText, new Vector2(40, 20), Color.White);

                var optionPosition = new Vector2(Globals.Bounds.X / 2, 40);
                for (int i = 0; i < currentQuestion.Options.Count; i++)
                {
                    var checkboxRectangle = new Rectangle(Globals.Bounds.X / 2 - 25, 40 + i * 30, 20, 20);
                    var checkboxTexture = i == _selectedOption ? _checkboxChecked : _checkboxUnchecked;
                    Globals.SpriteBatch.Draw(checkboxTexture, checkboxRectangle, Color.White);

                    var optionText = $"{i + 1}. {currentQuestion.Options[i]}";

                    Globals.SpriteBatch.DrawString(font, optionText, optionPosition, Color.White);
                    optionPosition.Y += 30;
                }
            }
        }

        private void CheckAnswer(int selectedOption)
        {
            if (displayQuestions)
            {
                
                if (_questions[_currentQuestionIndex].Options.Count != 0)
                {
                    answersSelected.Add(_questions[_currentQuestionIndex].Options[_selectedOption]);

                    if (_questions[_currentQuestionIndex].Options[_selectedOption].Equals("Order foods"))
                    {
                        _currentQuestionIndex = 3;
                    }
                    else if (_questions[_currentQuestionIndex].Options[_selectedOption].Equals("Order drinks"))
                    {
                        _currentQuestionIndex = 4;
                    }
                    else
                    {
                        if (_currentQuestionIndex == 3)
                        {
                            _currentQuestionIndex = 5;
                        }
                        else if (_currentQuestionIndex == 4)
                        {
                            _currentQuestionIndex = 6;
                            database.RemoveCoinValue();
                        }
                        else
                        {
                            _currentQuestionIndex++;
                        }
                    }
                }
                else
                {
                    if (_currentQuestionIndex == 5)
                    {
                        _currentQuestionIndex = 7;
                    }
                    _currentQuestionIndex++;
                }
            }
            if (_currentQuestionIndex >= _questions.Count)
            {
                HideAllSpritesAndTextures();
            }
        }

        public static int GenerateRandomNumber(int minValue, int maxValue)
        {
            Random random = new Random();
            return random.Next(minValue, maxValue + 1);
        }
        private async void GetDrinks()
        {
            HttpClient httpClient = new HttpClient();
            string apiUrl = "https://www.thecocktaildb.com/api/json/v1/1/filter.php?i=Gin";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Cocktail cocktail = JsonConvert.DeserializeObject<Cocktail>(responseBody);

                if (cocktail?.Drinks != null)
                {
                    result = new List<string> { };
                    for (int i = 0; i < 3 && i < cocktail.Drinks.Length; i++)
                    {
                        result.Add(cocktail.Drinks[GenerateRandomNumber(0, 10)].StrDrink);
                    }

                }
                else
                {
                    result = new List<string> { "coffee", "lemon joice", "red tea" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                httpClient.Dispose();
            }
        }

    }
}
