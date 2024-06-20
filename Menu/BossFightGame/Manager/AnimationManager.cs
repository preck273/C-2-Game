using BossFightGame.Models;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BossFightGame.Manager
{
	public class AnimationManager
	{
		private Animation _animation; 
		private float _timer; 

		public Vector2 Position { get; set; } 

		// Constructor
		public AnimationManager(Animation animation)
		{
			_animation = animation;
		}

		// Draws the current frame of the animation
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_animation.Texture, Position,
				new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0,
				_animation.FrameWidth, _animation.FrameHeight),
				Color.White);
		}

		// Plays a new animation
		public void Play(Animation animation)
		{
			if (_animation == animation) // If the new animation is the same as the current one, do nothing
				return;

			_animation = animation; 
			_animation.CurrentFrame = 0; 
			_timer = 0; 
		}

		// Stops the animation
		public void Stop()
		{
			_timer = 0f;
		}

		// Updates the animation to change frames based on the timer
		public void Update(GameTime gameTime)
		{
			_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (_timer > _animation.FrameSpeed)
			{
				_timer = 0f;
				_animation.CurrentFrame++;

				if (_animation.CurrentFrame >= _animation.FrameCount)
					_animation.CurrentFrame = 0; // Loop back to the first frame
			}
		}
	}
}
