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

namespace barArcadeGame.View
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

        private Enemy[] slimes;
        private Enemy[] slimes1;
        private Enemy[] slimes2;

        private CloseRangeEnemy[] closeGoblins;
        private CloseRangeEnemy[] closeGoblins1;
        
        private FarRangeEnemy[] farGoblins;

        private TmxMap map;
        private TileMapController mapManager1;
        private TileMapController mapManager;
        private List<Rectangle> collisionObjects;
        private List<Rectangle> collisionDoor;
        private Matrix matrix;
        private Matrix _translation;
        private GameController _gameManager;
        private string currentScene;
        private House house;
        private House fromHouse;
        private int stage = 0;

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
            var dx = _graphics.PreferredBackBufferWidth / 2 - player.pos.X;
            var dy = _graphics.PreferredBackBufferHeight / 2 - player.pos.Y;
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
            SoundController.PlayJazzFx();
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
            mapManager = new TileMapController(_spriteBatch, map, tileset, TileSetTilesWide, tileWidth, tileHeight);

            collisionObjects = new List<Rectangle>();
            foreach (var o in map.ObjectGroups["Objects"].Objects)
            {
                collisionObjects.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
            }

            SpriteSheet[] sheets = { Content.Load<SpriteSheet>("Defender/Idle/playerSheetIdle.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Defender/Walk/playerSheetWalk.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Defender/Attack/playerSheetAttack.sf", new JsonContentLoader())};


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
            mapManager = new TileMapController(_spriteBatch, map, tileset, TileSetTilesWide, tileWidth, tileHeight);

            collisionObjects = new List<Rectangle>();
            foreach (var o in map.ObjectGroups["Objects"].Objects)
            {
                collisionObjects.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
            }



            SpriteSheet[] sheets = { Content.Load<SpriteSheet>("Defender/Idle/playerSheetIdle.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Defender/Walk/playerSheetWalk.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Defender/Attack/playerSheetAttack.sf", new JsonContentLoader()),};

            player.Load(sheets, new Vector2(600, 250));

            SpriteSheet[] bobSheet = { Content.Load<SpriteSheet>("Defender/Npc/bobSheetIdle.sf", new JsonContentLoader()) };
            bob.Load(bobSheet, new Vector2(100, 200));

            _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");

            house = new House(new Vector2(430, 90), false);
            _gameManager = new();
        }

        private void LoadFightScene()//Initialise with enemy types
        {
            //Init all Enemies 
            slimes = new Enemy[10];

            closeGoblins = new CloseRangeEnemy[10];

            farGoblins = new FarRangeEnemy[10];


            SoundController.PlayBattleFx();
            if (stage == 1)
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
                mapManager = new TileMapController(_spriteBatch, map, tileset, TileSetTilesWide, tileWidth, tileHeight);

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

                SpriteSheet[] sheets = { Content.Load<SpriteSheet>("Defender/Idle/playerSheetIdle.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Defender/Walk/playerSheetWalk.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Defender/Attack/playerSheetAttack.sf", new JsonContentLoader()),};

                player.Load(sheets, new Vector2(600, 250));
                //ENEMY SPAWN HANDLING
                SpriteSheet slimesheet = Content.Load<SpriteSheet>("Defender/Enemies/slime.sf", new JsonContentLoader());

                Random random = new Random();
                for (int i = 0; i < slimes.Length; i++)
                {
                    slimes[i] = new Enemy();
                    Vector2 randomPosition = new Vector2(random.Next(100, 1200), random.Next(300, 500));
                    slimes[i].Load(slimesheet, randomPosition);
                }

                _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");

                house = new House(new Vector2(430, 90), false);
                _gameManager = new();
            }
            else if (stage == 2)
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
                mapManager = new TileMapController(_spriteBatch, map, tileset, TileSetTilesWide, tileWidth, tileHeight);

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

                SpriteSheet[] sheets = { Content.Load<SpriteSheet>("Defender/Idle/playerSheetIdle.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Defender/Walk/playerSheetWalk.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Defender/Attack/playerSheetAttack.sf", new JsonContentLoader()),};

                player.Load(sheets, new Vector2(600, 250));

                //ENEMY SPAWN HANDLING
                SpriteSheet slimesheet = Content.Load<SpriteSheet>("Defender/Enemies/slime.sf", new JsonContentLoader());

                Random random = new Random();
                for (int i = 0; i < slimes.Length; i++)
                {
                    slimes[i] = new Enemy();
                    Vector2 randomPosition = new Vector2(random.Next(100, 600), random.Next(300, 700));
                    slimes[i].Load(slimesheet, randomPosition);
                }

                SpriteSheet enemyCloseSheets = Content.Load<SpriteSheet>("Defender/Enemies/close.sf", new JsonContentLoader());

                for (int i = 0; i < closeGoblins.Length; i++)
                {
                    closeGoblins[i] = new CloseRangeEnemy();
                    Vector2 randomPosition = new Vector2(random.Next(100, 700), random.Next(300, 700));
                    closeGoblins[i].Load(enemyCloseSheets, randomPosition);
                }


                _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");

                house = new House(new Vector2(430, 90), false);
                _gameManager = new();
            }
            else if (stage == 3)
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
                mapManager = new TileMapController(_spriteBatch, map, tileset, TileSetTilesWide, tileWidth, tileHeight);

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

                SpriteSheet[] sheets = { Content.Load<SpriteSheet>("Defender/Idle/playerSheetIdle.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Defender/Walk/playerSheetWalk.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("Defender/Attack/playerSheetAttack.sf", new JsonContentLoader()),};

                player.Load(sheets, new Vector2(600, 250));

                //ENEMY SPAWN HANDLING
                SpriteSheet slimesheet = Content.Load<SpriteSheet>("Defender/Enemies/slime.sf", new JsonContentLoader());

                Random random = new Random();
                for (int i = 0; i < slimes.Length; i++)
                {
                    slimes[i] = new Enemy();
                    Vector2 randomPosition = new Vector2(random.Next(100, 700), random.Next(300, 500));
                    slimes[i].Load(slimesheet, randomPosition);
                }

                SpriteSheet enemyCloseSheets = Content.Load<SpriteSheet>("Defender/Enemies/close.sf", new JsonContentLoader());

                for (int i = 0; i < closeGoblins.Length; i++)
                {
                    closeGoblins[i] = new CloseRangeEnemy();
                    Vector2 randomPosition = new Vector2(random.Next(100, 700), random.Next(300, 500));
                    closeGoblins[i].Load(enemyCloseSheets, randomPosition);
                }

                SpriteSheet enemyRangeSheets = Content.Load<SpriteSheet>("Defender/Enemies/range.sf", new JsonContentLoader());

                for (int i = 0; i < farGoblins.Length; i++)
                {
                    farGoblins[i] = new FarRangeEnemy();
                    Vector2 randomPosition = new Vector2(random.Next(100, 700), random.Next(300, 500));
                    farGoblins[i].Load(enemyRangeSheets, randomPosition);
                }

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


            bob.Update(gameTime);
            player.Update(gameTime);

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
                    SoundController.PlayDoorLockedFX();
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var initpos = player.pos;
            
           
            foreach (var rect in collisionObjects)
            {
                if (rect.Intersects(player.playerBounds))
                {
                    player.pos = initpos;
                    player.isIdle = true;

                    SoundController.PlayCollideFx();
                }
            }
            
            foreach (var slime in slimes)
            {
                //fix for each sprite
                if (slime.enemyBounds.Intersects(player.playerBounds))
                {
                    //lose hp from player
                    //Pop the slime from the list

                    //replace with hurt sound
                    SoundController.PlayHurtFx();
                }

            }

            if(stage == 1)
            {
                player.Update(gameTime);
                foreach (var enemy in slimes)
                {
                    enemy.Update(gameTime, player.pos);
                }
            }
            else if(stage == 2)
            {
                player.Update(gameTime);
                foreach (var enemy in slimes)
                {
                    enemy.Update(gameTime, player.pos);
                }

                foreach (var enemy in closeGoblins)
                {
                    enemy.Update(gameTime, player.pos);
                }

            }
            else if(stage == 3)
            {
                player.Update(gameTime);
                foreach (var enemy in slimes)
                {
                    enemy.Update(gameTime, player.pos);
                }

                foreach (var enemy in closeGoblins)
                {
                    enemy.Update(gameTime, player.pos);
                }

                foreach (var enemy in farGoblins)
                {
                    enemy.Update(gameTime, player.pos);
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
                    SoundController.PlayDoorLockedFX();
                    if(slimes.Length > 0 && closeGoblins.Length == 10 && farGoblins.Length == 10 && stage == 1)
                    {
                        //If the slime array is empty
                        LoadSecondScene();
                    }
                    else if(slimes.Length == 0 && closeGoblins.Length == 0 && farGoblins.Length == 10 && stage == 2)
                    {
                        //If the slime array is empty
                        LoadSecondScene();
                    }
                    if(slimes.Length == 0 && closeGoblins.Length == 0 && farGoblins.Length == 0 && stage == 3)
                    {
                        //If the slime array is empty
                        LoadSecondScene();
                    }
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
                    SoundController.PlayDoorOpenFX();
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
            if (currentScene == "outside")
            {
                updateOutsideScene(gameTime);
            }
            else if (currentScene == "house")
            {
                updateArcadeScene(gameTime);
            }
            else if (currentScene == "first" || currentScene == "second" || currentScene == "boss")
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
                finally
                {
                    _spriteBatch.End();
                }

            }
            else if (currentScene == "house")
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
            else if (currentScene == "first" || currentScene == "second" || currentScene == "third")
            {
                var mouseState = Mouse.GetState();
                var mousePosition = new Vector2(mouseState.X, mouseState.Y);
                mapManager.Draw(matrix, _translation);

                house.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                player.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                _gameManager.Draw();

                foreach (var slime in slimes)
                {
                    slime.Draw(_spriteBatch, matrix, _translation);
                }

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
            else if (currentScene == "second" )
            {
                var mouseState = Mouse.GetState();
                var mousePosition = new Vector2(mouseState.X, mouseState.Y);
                mapManager.Draw(matrix, _translation);

                house.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                player.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                _gameManager.Draw();

                foreach (var slime in slimes)
                {
                    slime.Draw(_spriteBatch, matrix, _translation);
                }

                foreach (var cGoblin in closeGoblins)
                {
                    cGoblin.Draw(_spriteBatch, matrix, _translation);
                }

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

            else if (currentScene == "third")
            {
                var mouseState = Mouse.GetState();
                var mousePosition = new Vector2(mouseState.X, mouseState.Y);
                mapManager.Draw(matrix, _translation);

                house.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                player.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                _gameManager.Draw();

                foreach (var slime in slimes)
                {
                    slime.Draw(_spriteBatch, matrix, _translation);
                }
                foreach (var fGoblin in farGoblins)
                {
                    fGoblin.Draw(_spriteBatch, matrix, _translation);
                }

                foreach (var cGoblin in closeGoblins)
                {
                    cGoblin.Draw(_spriteBatch, matrix, _translation);
                }

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
