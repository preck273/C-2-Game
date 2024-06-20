using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using BossFightGame.Models;
using BossFightGame.Manager;
using System.Linq;

namespace BossFightGame.Sprites
{
	public class Sprite
	{
		protected AnimationManager _animationManager; 
		protected Dictionary<string, Animation> _animations; // Dictionary of animations

		public Vector2 _velocity; 
		protected Vector2 _position { get; set; } 
		protected Texture2D _texture { get; set; } 
		public float _speed = 1f; 
		public Input Input { get; set; } 

		public Vector2 Position
		{
			get { return _position; }
			set
			{
				_position = value;
				if (_animationManager != null)
					_animationManager.Position = _position;
			}
		}

		// Constructor initializing the sprite with a texture
		public Sprite(Texture2D texture)
		{
			_texture = texture;
		}

		// Constructor initializing the sprite with animations
		public Sprite(Dictionary<string, Animation> animations)
		{
			_animations = animations;
			_animationManager = new AnimationManager(_animations.First().Value);
		}

		
		public Rectangle Rectangle
		{
			get
			{
				return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
			}
		}

		// Update the sprite's position and animation
		public virtual void Update(GameTime gameTime, List<Sprite> sprites)
		{
			Move(); 
			AddAnimations(); 
			_animationManager?.Update(gameTime); 

			_position += _velocity; 
			_velocity = Vector2.Zero; // Reset velocity
		}

		// Play animations based on velocity
		protected virtual void AddAnimations()
		{
			if (_velocity.X > 0)
			{
				_animationManager?.Play(_animations["MoveRight"]);
			}
			else if (_velocity.X < 0)
			{
				_animationManager?.Play(_animations["MoveLeft"]);
			}
			else
			{
				_animationManager?.Stop();
			}
		}

		// Update the sprite's velocity based on input
		private void Move()
		{
			var state = Keyboard.GetState();

			if (state.IsKeyDown(Input.Right))
				_velocity.X = _speed;
			else if (state.IsKeyDown(Input.Left))
				_velocity.X = -_speed;
		}

		// Draw the sprite
		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (_texture != null)
			{
				spriteBatch.Draw(_texture, _position, Color.White);
			}
			else if (_animationManager != null)
			{
				_animationManager.Draw(spriteBatch);
			}
		}
	}
}
