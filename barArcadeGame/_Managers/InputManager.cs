using barArcadeGame.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace barArcadeGame._Managers
{
    public static class InputManager
    {
        private static MouseState _lastMouseState;
        public static bool MouseClicked { get; private set; }
        public static bool MouseRightClicked { get; private set; }
        public static Rectangle MouseRectangle { get; private set; }

        public static void Update()
        {
            var ms = Mouse.GetState();
            var onscreen = ms.X >= 0 && ms.X < Globals.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth
                        && ms.Y >= 0 && ms.Y < Globals.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight
                        && Globals.Game.IsActive;

            MouseClicked = ms.LeftButton == ButtonState.Pressed
                            && _lastMouseState.LeftButton == ButtonState.Released
                            && onscreen;
            MouseRightClicked = ms.RightButton == ButtonState.Pressed
                            && _lastMouseState.RightButton == ButtonState.Released
                            && onscreen;
            _lastMouseState = ms;

            MouseRectangle = new(ms.X, ms.Y, 1, 1);
        }
    }

}
