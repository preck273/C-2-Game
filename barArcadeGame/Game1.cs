using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using TiledSharp;
using barArcadeGame._Managers;
using System.Collections;
using MonoGame.Extended.Timers;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Newtonsoft.Json.Linq;
using System.Windows;
using barArcadeGame.Model;

namespace barArcadeGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;
        private Bob bob;
        private Jack jack;
        private TmxMap map;
        private TileMapManager mapManager1;
        private TileMapManager mapManager;
        private List<Rectangle> collisionObjects;
        private List<Rectangle> collisionDoor;
        private Matrix matrix;
        private Matrix _translation;
        private GameManager _gameManager;
        private string currentScene;
        private Fridge fridge;
        private ArcadeMachine arcadeMachine;

        private Texture2D _crosshairTexture;

        /*MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.NavigateToMainWindow();
            }*/
        private float _timer;
        private const float DelayInSeconds = 1.0f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        private void calculateTransaction()
        {
            var dx = (_graphics.PreferredBackBufferWidth / 2) - player.pos.X;
            var dy = (_graphics.PreferredBackBufferHeight / 2) - player.pos.Y;
            _translation = Matrix.CreateTranslation(dx, dy, 0f);
        }

        protected override void Initialize()
        {      
            Globals.Game = this; 
            currentScene = "outside";
            player = new Player();
            bob = new Bob();
            
            Window.Title = "Bararcade Game";
            Globals.Bounds = new(1000, 1000);
            _graphics.PreferredBackBufferWidth = Globals.Bounds.X;
            _graphics.PreferredBackBufferHeight = Globals.Bounds.Y;
            var Width = _graphics.PreferredBackBufferWidth;
            var Height = _graphics.PreferredBackBufferHeight;
            _graphics.ApplyChanges();
            var WindowSize = new System.Numerics.Vector2(Width, Height);
            var mapSize = new System.Numerics.Vector2(500, 500);
            matrix = Matrix.CreateScale(new System.Numerics.Vector3(WindowSize / mapSize, 1));
            Globals.Matrix = matrix;         
            base.Initialize();
        }

        private void LoadSecondScene()
        {
            currentScene = "house";
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
            Globals.Content = Content;
            map = new TmxMap("Content/arcade/arcadeRoom.tmx");
         
            var tileset = Content.Load<Texture2D>("arcade/" + map.Tilesets[0].Name.ToString());
            var tileWidth = map.Tilesets[0].TileWidth;
            var tileHeight = map.Tilesets[0].TileHeight;

            var TileSetTilesWide = tileset.Width / tileWidth;
            mapManager = new TileMapManager(_spriteBatch, map, tileset, TileSetTilesWide, tileWidth, tileHeight);

            collisionObjects = new List<Rectangle>();
            foreach (var o in map.ObjectGroups["Objects"].Objects)
            {
                collisionObjects.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
            }

            SpriteSheet[] sheets = { Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Idle/playerSheetIdle.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Walk/playerSheetWalk.sf",new JsonContentLoader()), 
                                    Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Attack/playerSheetAttack.sf", new JsonContentLoader())};
           

            player.Load(sheets, new Vector2(100, 200));
            
          
           
            var arcadeTexture = Globals.Content.Load<Texture2D>("picture/arcadeMachine");
            arcadeMachine = new ArcadeMachine(arcadeTexture,new Vector2(100, 60));
            _gameManager = new();
        }

        protected override void LoadContent()
        {
            

            currentScene = "outside";
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
            Globals.Content = Content;
            map = new TmxMap("Content/barMap/mapBar.tmx");
            var tileset = Content.Load<Texture2D>("barMap/" + map.Tilesets[0].Name.ToString());
            var tileWidth = map.Tilesets[0].TileWidth;
            var tileHeight = map.Tilesets[0].TileHeight;
            var TileSetTilesWide = tileset.Width / tileWidth;
            mapManager = new TileMapManager(_spriteBatch, map, tileset, TileSetTilesWide, tileWidth, tileHeight);

            collisionObjects = new List<Rectangle>();
            foreach (var o in map.ObjectGroups["Objects"].Objects)
            {
                collisionObjects.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
            }

            collisionDoor = new List<Rectangle>();
            foreach (var o in map.ObjectGroups["door"].Objects)
            {
                collisionDoor.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
            }

            SpriteSheet[] sheets = { Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Idle/playerSheetIdle.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Walk/playerSheetWalk.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Attack/playerSheetAttack.sf", new JsonContentLoader()),};

            player.Load(sheets, new Vector2(600,250));


            SpriteSheet[] bobSheet = { Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Npc/bobSheetIdle.sf", new JsonContentLoader()) };
            bob.Load(bobSheet, new Vector2(100, 200));

            _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");

            jack = new Jack();
            fridge = new Fridge(new Vector2(430, 90));
            _gameManager = new();
        }

        public void updateBarScene(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var initpos = player.pos;

            // Update sprites
            player.Update(gameTime);
            bob.Update(gameTime);
            jack.Update();

            foreach (var rect in collisionObjects)
            {
                if (rect.Intersects(player.playerBounds))
                {
                    player.pos = initpos;               
                    player.isIdle = true;

                    //TOdo: play can't move out of the map
                    SoundManager.PlayCollideFx();
                }
            }


            //Player collison with door
            if (player.playerBounds.Intersects(fridge.bounds))
            {
                fridge.touch = true;               
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer >= DelayInSeconds)
                {
                    _timer = 0;
                    SoundManager.PlayDoorOpenFX();
                    LoadSecondScene(); 
                }
            }
            else
            {
                fridge.touch = false;
            }

            if (jack.playerBounds.Intersects(player.playerBounds))
            {
                jack.stop();             
                _gameManager.runJackDialogue();
            } 
            if (bob.playerBounds.Intersects(player.playerBounds))
            {
                bob.Stop();             
                //Todo: run bob dialogue
                //_gameManager.runJackDialogue();
            } 
            else
            {
                bob.Panic();
                jack.walk();
                _gameManager.hideJackDialogue();
            }         

            _gameManager.Update();
            fridge.Update();
            calculateTransaction();
            Globals.Update(gameTime);
        }

        public void updateArcadeScene(GameTime gameTime)
        {
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var initpos = player.pos;
            player.Update(gameTime);

            ninja.Update();
            foreach (var rect in collisionObjects)
            {
                if (rect.Intersects(player.playerBounds))
                {
                    player.pos = initpos;
                    player.isIdle = true;
                }
            }

            if (player.playerBounds.Intersects(doorToBar.bounds))
            {
                door.touch = true;
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer >= DelayInSeconds)
                {
                    _timer = 0;
                    SoundManager.PlayDoorOpenFX();
                    LoadContent(); 
                }
            }
            else
            {
                door.touch = false;
            }

            if (ninja.playerBounds.Intersects(player.playerBounds))
            {
                ninja.stop();
                _gameManager.runMickDialogue();
            }
            else
            {
                ninja.walk();
                _gameManager.hideMickDialogue();
            }

            if (arcadeMachine._rectangle.Intersects(player.playerBounds))
            {
                //currentScene = "close Scenes";
                runArcadeGame();      
            }

            _gameManager.Update();
            doorToBar.Update();
            arcadeMachine.Update();
            calculateTransaction();
            Globals.Update(gameTime);*/
        }

     

        protected override void Update(GameTime gameTime)
        {
            if(currentScene == "outside")
            {
                updateBarScene(gameTime);
            }
            else if(currentScene == "house")
            {
                updateArcadeScene(gameTime);
            }
           
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            // When you're indoors, you can select an ability and this will plant a flower outside for you or something like that

            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (currentScene == "outside")
            {
                var mouseState = Mouse.GetState();
                var mousePosition = new Vector2(mouseState.X, mouseState.Y);
                mapManager.Draw(matrix, _translation);

               //Draw sprites
                fridge.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                player.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                bob.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                jack.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                _gameManager.Draw();

                _spriteBatch.Begin();
                

                try
                {
                    _spriteBatch.Draw(_crosshairTexture, mousePosition, Color.White);
                   
                }
                finally {
                _spriteBatch.End();
                }
               
            }
            else if(currentScene == "house")
            {
                Vector2 scale = new Vector2(2.5f, 2.5f);
                mapManager.Draw(matrix, _translation);
                //doorToBar.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                player.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                arcadeMachine.Draw(_spriteBatch, matrix, transformMatrix: _translation);


                var mouseState = Mouse.GetState();
                var mousePosition = new Vector2(mouseState.X, mouseState.Y);
                _spriteBatch.Draw(_crosshairTexture, mousePosition, Color.White);
                _gameManager.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
