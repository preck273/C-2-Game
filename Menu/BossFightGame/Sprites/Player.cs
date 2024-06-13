using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BossFightGame.Sprites;
using BossFightGame.Models;
using BossFightGame.Manager;

namespace BossFightGame.Sprites
{
	public class Player : Sprite
	{

		public bool isDead = false;

		public Input Input { get; set; }

		

		public Player(Texture2D texture) : base(texture)
		{
			_position = new Vector2(30, 40);
			_speed = 5f;
		}

		


	}


}
