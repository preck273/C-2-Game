using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using BossFightGame.Sprites;
using BossFightGame.Models;

namespace BossFightGame
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private List<Sprite> _sprites;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// Set the preferred back buffer size
			_graphics.PreferredBackBufferWidth = 1920; 
			_graphics.PreferredBackBufferHeight = 1080; 
			_graphics.ApplyChanges(); 

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// Load animations for the player sprite
			var animations = new Dictionary<string, Animation>()
			{
				{"MoveRight", new Animation(Content.Load<Texture2D>("Player/Right2"), 3) },
				{"MoveLeft", new Animation(Content.Load<Texture2D>("Player/Left2"), 3) },
			};

			// Initialize the player sprite with animations and input controls
			_sprites = new List<Sprite>()
			{
				new Sprite(animations)
				{
					Position = new Vector2(0, 0),
					Input = new Input()
					{
						Right = Keys.D,
						Left = Keys.A,
					},
					_speed = 2f,
				},
			};
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// Update all sprites
			foreach (var sprite in _sprites)
				sprite.Update(gameTime, _sprites);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();
			// Draw all sprites
			foreach (var sprite in _sprites)
				sprite.Draw(_spriteBatch);
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
