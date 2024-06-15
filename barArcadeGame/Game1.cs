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

    //Tutorial scene- bob is outside
    //Buy scene - player can go inside
    // Morning scene - bob has locked himself inside
    //Buy scene - player can go inside
    //Afternoon scene  - bob has locked himself inside - bg to yellow orage
    // Buy scene - player can go inside for final upgrade - if luck hit game won no additional stuff
    //Evening scene

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;
        private Bob bob;
        private TmxMap map;
        private TileMapManager mapManager1;
        private TileMapManager mapManager;
        private List<Rectangle> collisionObjects;
        private List<Rectangle> collisionDoor;
        private Matrix matrix;
        private Matrix _translation;
        private GameManager _gameManager;
        private string currentScene;
        private House house;
        private House fromHouse;
        private int stage= 0;

        private Texture2D _crosshairTexture;
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
            
            Window.Title = " Last Defender Standing";
            Globals.Bounds = new(850, 750);
            _graphics.PreferredBackBufferWidth = Globals.Bounds.X;
            _graphics.PreferredBackBufferHeight = Globals.Bounds.Y;
            var Width = _graphics.PreferredBackBufferWidth;
            var Height = _graphics.PreferredBackBufferHeight;
            _graphics.ApplyChanges();
            var WindowSize = new System.Numerics.Vector2(Width, Height);
            var mapSize = new System.Numerics.Vector2(2000, 2000);
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
           

            player.Load(sheets, new Vector2(100, 300));

            var arcadeTexture = Globals.Content.Load<Texture2D>("picture/arcadeMachine");
            
            _gameManager = new();
            fromHouse = new House(new Vector2(430, 90), false);
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

           

            SpriteSheet[] sheets = { Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Idle/playerSheetIdle.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Walk/playerSheetWalk.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Attack/playerSheetAttack.sf", new JsonContentLoader()),};

            player.Load(sheets, new Vector2(600,250));


            SpriteSheet[] bobSheet = { Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Npc/bobSheetIdle.sf", new JsonContentLoader()) };
            bob.Load(bobSheet, new Vector2(100, 200));

            _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");

            house = new House(new Vector2(430, 90), false);
            _gameManager = new();
        }
        private void LoadFightScene()//Initialise with enemy types
        {
            if(stage == 1)
            {

                currentScene = "first";
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

                player.Load(sheets, new Vector2(600, 250));


                SpriteSheet[] bobSheet = { Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Npc/bobSheetIdle.sf", new JsonContentLoader()) };
                bob.Load(bobSheet, new Vector2(100, 200));

                _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");

                house = new House(new Vector2(430, 90), false);
                _gameManager = new();
            }
            else if(stage == 2)
            {

                currentScene = "second";
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

                player.Load(sheets, new Vector2(600, 250));


                SpriteSheet[] bobSheet = { Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Npc/bobSheetIdle.sf", new JsonContentLoader()) };
                bob.Load(bobSheet, new Vector2(100, 200));

                _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");

                house = new House(new Vector2(430, 90), false);
                _gameManager = new();
            }
            else if(stage == 3)
            {

                currentScene = "boss";
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

                player.Load(sheets, new Vector2(600, 250));


                SpriteSheet[] bobSheet = { Content.Load<SpriteSheet>("Tiny Adventure Pack/Character/char_two/Npc/bobSheetIdle.sf", new JsonContentLoader()) };
                bob.Load(bobSheet, new Vector2(100, 200));

                house = new House(new Vector2(430, 90), false);
                _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");
                _gameManager = new();
            }

        }

        //For safe rounds
        public void updateOutsideScene(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var initpos = player.pos;

            // Update sprites
            player.Update(gameTime);
            bob.Update(gameTime);
            
            foreach (var rect in collisionObjects)
            {
                if (rect.Intersects(player.playerBounds))
                {
                    player.pos = initpos;               
                    player.isIdle = true;

                }
            }


            //Player collison with door
            if (player.playerBounds.Intersects(house.bounds))
            {
                house.touch = true;
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer >= DelayInSeconds)
                {
                    _timer = 0;
                    //SoundManager.PlayDoorOpenFX();
                    SoundManager.PlayDoorLockedFX();
                    //LoadSecondScene(); 
                }
            }
            else
            {
                house.touch = false;
            }

           
            if (bob.playerBounds.Intersects(player.playerBounds))
            {
                bob.Stop();             
                //Todo: run bob dialogue
                _gameManager.runBobDialogue();
                if (_gameManager.GetBobDialogueRun())
                {
                    stage = 1;
                    LoadFightScene();
                }
                    
            } 
            else
            {
                bob.Panic();
                _gameManager.hideBobDialogue();
            }         

            _gameManager.Update();
            house.UpdateFight();
            calculateTransaction();
            Globals.Update(gameTime);
        }

        //run fight scene
        public void updateFightScene(GameTime gameTime)
        {
            // Call enemies
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var initpos = player.pos;

            // Update sprites
            player.Update(gameTime);
            bob.Update(gameTime);
            //jack.Update();

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
            if (player.playerBounds.Intersects(house.bounds))
            {
                house.touch = true;
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer >= DelayInSeconds)
                {
                    _timer = 0;
                    SoundManager.PlayDoorLockedFX();
                    //LoadSecondScene();
                }
            }
            else
            {
                house.touch = false;
            }

            _gameManager.Update();
            house.UpdateFight();
            calculateTransaction();
            Globals.Update(gameTime);
        }

        public void updateArcadeScene(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var initpos = player.pos;
            player.Update(gameTime);

            //ninja.Update();
            foreach (var rect in collisionObjects)
            {
                if (rect.Intersects(player.playerBounds))
                {
                    player.pos = initpos;
                    player.isIdle = true;
                }
            }

            if (player.playerBounds.Intersects(fromHouse.bounds))
            {
                house.touch = true;
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer >= DelayInSeconds)
                {
                    _timer = 0;
                    SoundManager.PlayDoorOpenFX();
                    stage++;
                    LoadFightScene();
                }
            }
            else
            {
                house.touch = false;
            }

            if (bob.playerBounds.Intersects(player.playerBounds))
            {
                bob.Stop();
                _gameManager.runBobDialogue();
            }
            else
            {
                bob.Panic();
            }

            _gameManager.Update();
            fromHouse.Update();
            calculateTransaction();
            Globals.Update(gameTime);
        }

     

        protected override void Update(GameTime gameTime)
        {
            if(currentScene == "outside")
            {
                updateOutsideScene(gameTime);
            }
            else if(currentScene == "house")
            {
                updateArcadeScene(gameTime);
            }
            else if(currentScene == "first" || currentScene == "second" || currentScene == "boss")
            {
                updateFightScene(gameTime);
            }
           
            base.Update(gameTime);
        }



        // When you're indoors, you can select an ability and this will plant a flower outside for you or something like that
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (currentScene == "outside")
            {
                var mouseState = Mouse.GetState();
                var mousePosition = new Vector2(mouseState.X, mouseState.Y);
                mapManager.Draw(matrix, _translation);

                //Draw sprites
                house.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                player.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                bob.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                _gameManager.Draw();

                _spriteBatch.Begin();
                

                try
                {

                    Vector2 scale = new Vector2(2.5f, 2.5f);
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
                fromHouse.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                player.Draw(_spriteBatch, matrix, transformMatrix: _translation);

                bob.Draw(_spriteBatch, matrix, transformMatrix: _translation);

                var mouseState = Mouse.GetState();
                var mousePosition = new Vector2(mouseState.X, mouseState.Y);
                _spriteBatch.Draw(_crosshairTexture, mousePosition, Color.White);
                _gameManager.Draw();
            }
            else if (currentScene == "first")
            {
                var mouseState = Mouse.GetState();
                var mousePosition = new Vector2(mouseState.X, mouseState.Y);
                mapManager.Draw(matrix, _translation);
                 
                house.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                player.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                _gameManager.Draw();

                _spriteBatch.Begin();


                try
                {
                    _spriteBatch.Draw(_crosshairTexture, mousePosition, Color.White);

                }
                finally
                {
                    _spriteBatch.End();
                }

            }

            base.Draw(gameTime);
        }
    }
}
