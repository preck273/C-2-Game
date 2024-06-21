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
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;
        private Player player;
        private string playerName;
        private Bob bob;

        private List<Enemy> slimes;
        private List<CloseRangeEnemy> closeGoblins;
        private List<FarRangeEnemy> farGoblins;
        private List<Bullet> bullets;
        private Texture2D bulletTexture;

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
        private float timeSinceLastBullet;
        private const float BulletCooldown = 0.5f;

        public Game1(string playerName)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            bullets = new List<Bullet>();
            this.playerName = playerName;
        }

        private void calculateTransaction()
        {
            var dx = graphics.PreferredBackBufferWidth / 2 - player.pos.X;
            var dy = graphics.PreferredBackBufferHeight / 2 - player.pos.Y;
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
            graphics.PreferredBackBufferWidth = Globals.Bounds.X;
            graphics.PreferredBackBufferHeight = Globals.Bounds.Y;
            var Width = graphics.PreferredBackBufferWidth;
            var Height = graphics.PreferredBackBufferHeight;
            graphics.ApplyChanges();
            var WindowSize = new System.Numerics.Vector2(Width, Height);
            var mapSize = new System.Numerics.Vector2(2000, 2000);
            matrix = Matrix.CreateScale(new System.Numerics.Vector3(WindowSize / mapSize, 1));
            Globals.Matrix = matrix;
            base.Initialize();
            SoundController.PlayJazzFx();
        }

        private void LoadSecondScene()
        {

            bob = new Bob();
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


            player.Load(sheets, new Vector2(100, 200));

            SpriteSheet[] bobSheet = { Content.Load<SpriteSheet>("Defender/Npc/bobSheetIdle.sf", new JsonContentLoader()) };
            bob.Load(bobSheet, new Vector2(100, 200));

       
            _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");

            fromHouse = new House(new Vector2(200, 260), false);

            _gameManager = new();
        }

        protected override void LoadContent()
        {
            currentScene = "outside";
            LoadCommons();

            SpriteSheet[] bobSheet = { Content.Load<SpriteSheet>("Defender/Npc/bobSheetIdle.sf", new JsonContentLoader()) };
            bob.Load(bobSheet, new Vector2(100, 200));


            house = new House(new Vector2(430, 90), false);
            _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");
            _gameManager = new();
        }

        private void LoadFightScene()
        {
            slimes = new List<Enemy>(); ;

            closeGoblins = new List<CloseRangeEnemy>();

            farGoblins = new List<FarRangeEnemy>();

            if (stage == 1)
            {
                currentScene = "first";
                LoadCommons();

                Random random = new Random();

                SpriteSheet slimesheet = Content.Load<SpriteSheet>("Defender/Enemies/slime.sf", new JsonContentLoader());

                for (int i = 0; i < 0; i++)
                {
                    var slime = new Enemy();
                    Vector2 randomPosition = new Vector2(random.Next(100, 1200), random.Next(300, 500));
                    slime.Load(slimesheet, randomPosition);
                    slimes.Add(slime);
                }

                house = new House(new Vector2(430, 90), false);
                _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");
                _gameManager = new();
            }
            else if (stage == 2)
            {
                currentScene = "second";
                LoadCommons();

                Random random = new Random();

                SpriteSheet slimesheet = Content.Load<SpriteSheet>("Defender/Enemies/slime.sf", new JsonContentLoader());

                for (int i = 0; i < 0; i++)
                {
                    var slime = new Enemy();
                    Vector2 randomPosition = new Vector2(random.Next(100, 1200), random.Next(300, 500));
                    slime.Load(slimesheet, randomPosition);
                    slimes.Add(slime);
                }
                SpriteSheet enemyCloseSheets = Content.Load<SpriteSheet>("Defender/Enemies/close.sf", new JsonContentLoader());

                for (int i = 0; i < 0; i++)
                {
                    var closeGoblin = new CloseRangeEnemy();
                    Vector2 randomPosition = new Vector2(random.Next(100, 1200), random.Next(300, 500));
                    closeGoblin.Load(enemyCloseSheets, randomPosition);
                    closeGoblins.Add(closeGoblin);
                }

                house = new House(new Vector2(430, 90), false);
                _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");
                _gameManager = new();
            }
            else if (stage == 3)
            {
                currentScene = "boss";
                LoadCommons();

                Random random = new Random();

                SpriteSheet slimesheet = Content.Load<SpriteSheet>("Defender/Enemies/slime.sf", new JsonContentLoader());

                for (int i = 0; i < 0; i++)
                {
                    var slime = new Enemy();
                    Vector2 randomPosition = new Vector2(random.Next(100, 1200), random.Next(300, 500));
                    slime.Load(slimesheet, randomPosition);
                    slimes.Add(slime);
                }
                SpriteSheet enemyCloseSheets = Content.Load<SpriteSheet>("Defender/Enemies/close.sf", new JsonContentLoader());

                for (int i = 0; i < 0; i++)
                {
                    var closeGoblin = new CloseRangeEnemy();
                    Vector2 randomPosition = new Vector2(random.Next(100, 1200), random.Next(300, 500));
                    closeGoblin.Load(enemyCloseSheets, randomPosition);
                    closeGoblins.Add(closeGoblin);
                }

                SpriteSheet enemyRangeSheets = Content.Load<SpriteSheet>("Defender/Enemies/range.sf", new JsonContentLoader());

                for (int i = 0; i < 0; i++)
                {
                    var farGoblin = new FarRangeEnemy();
                    Vector2 randomPosition = new Vector2(random.Next(100, 1200), random.Next(300, 500));
                    farGoblin.Load(enemyRangeSheets, randomPosition);
                    farGoblins.Add(farGoblin);
                }

                house = new House(new Vector2(430, 90), false);
                _crosshairTexture = Content.Load<Texture2D>("picture/crosshair");
                _gameManager = new();
            }
        }
        //For safe rounds
        public void LoadCommons()
        {
            
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
                                    Content.Load<SpriteSheet>("Defender/Attack/playerSheetAttack.sf", new JsonContentLoader())};

            player.Load(sheets, new Vector2(600, 250));

            bulletTexture = Content.Load<Texture2D>("picture/bullet");

        }

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
                _gameManager.runBobDialogue(stage);
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
        // if bullet touch enemy, Add the points to the player, delte bullet
        // if player slime touch player, remove slime from array

        public void updateFightScene(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastBullet += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > DelayInSeconds)
            {
                _timer = 0;
            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var initpos = player.pos;


            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && timeSinceLastBullet >= BulletCooldown)
            {
                timeSinceLastBullet = 0;
                Vector2 clickPosition = new Vector2(mouseState.X, mouseState.Y);

                int windowWidth = graphics.GraphicsDevice.Viewport.Width;
                int windowHeight = graphics.GraphicsDevice.Viewport.Height;

                clickPosition.X = MathHelper.Clamp(clickPosition.X, 0, windowWidth);
                clickPosition.Y = MathHelper.Clamp(clickPosition.Y, 0, windowHeight);

                Vector2 direction = clickPosition - player.pos;
                direction.Normalize();

                System.Diagnostics.Debug.WriteLine($"Click Position: {clickPosition}, Player Position: {player.pos}, Direction: {direction}");

                bullets.Add(new Bullet(player.pos, direction, 300f, bulletTexture, player.GetDamage()));

            }

            

            if (stage == 1)
            {
                if (player.GetLife() > 0)
                {
                    player.Update(gameTime);
                }
                
                for (int i = slimes.Count - 1; i >= 0; i--)
                {
                    var slime = slimes[i];
                    slime.Update(gameTime, player.pos);

                    if (slime.enemyBounds.Intersects(player.playerBounds) && !slime.isDying)
                    {
                        player.TakeDamage(1);
                    }
                }
            }
            else if(stage == 2)
            {
                if (player.GetLife() > 0)
                {
                    player.Update(gameTime);
                }
                for (int i = slimes.Count - 1; i >= 0; i--)
                {
                    var slime = slimes[i];
                    slime.Update(gameTime, player.pos);

                    if (slime.enemyBounds.Intersects(player.playerBounds) && !slime.isDying)
                    {
                        slime.Kill();
                        player.TakeDamage(1);
                    }
                }
                for (int i = closeGoblins.Count - 1; i >= 0; i--)
                {
                    var closeGoblin = closeGoblins[i];
                    closeGoblin.Update(gameTime, player.pos);

                    if (closeGoblin.enemyBounds.Intersects(player.playerBounds) && !closeGoblin.isDying)
                    {
                        closeGoblin.Attack();
                        player.TakeDamage(2);
                    }
                }

            }
            else if(stage == 3)
            {
                if (player.GetLife() > 0)
                {
                    player.Update(gameTime);
                }
                for (int i = slimes.Count - 1; i >= 0; i--)
                {
                    var slime = slimes[i];
                    slime.Update(gameTime, player.pos);

                    if (slime.enemyBounds.Intersects(player.playerBounds) && !slime.isDying)
                    {
                        slime.Kill();
                        player.TakeDamage(1);
                    }
                }
                for (int i = closeGoblins.Count - 1; i >= 0; i--)
                {
                    var closeGoblin = closeGoblins[i];
                    closeGoblin.Update(gameTime, player.pos);

                    if (closeGoblin.enemyBounds.Intersects(player.playerBounds) && !closeGoblin.isDying)
                    {
                        closeGoblin.Attack();
                        closeGoblins.RemoveAt(i);
                        player.TakeDamage(2);
                    }
                }
              
                for (int i = farGoblins.Count - 1; i >= 0; i--)
                {
                    var farGoblin = farGoblins[i];
                    farGoblin.Update(gameTime, player.pos);

                    if (farGoblin.enemyBounds.Intersects(player.playerBounds))
                    {
                        farGoblins.RemoveAt(i);
                        player.TakeDamage(3);
                    }
                }
            }
            
            var bulletsToRemove = new List<Bullet>();

            foreach (var bullet in bullets)
            {
                for (int i = slimes.Count - 1; i >= 0; i--)
                {
                    var slime = slimes[i];

                    if (slime.enemyBounds.Intersects(bullet.Bounds))
                    {
                        slime.CheckBulletCollision(bullet);
                        bulletsToRemove.Add(bullet);
                        SoundController.PlayDieFx();
                        break;

                    }
                }
                for (int i = closeGoblins.Count - 1; i >= 0; i--)
                {
                    var closeGoblin = closeGoblins[i];

                    if (closeGoblin.enemyBounds.Intersects(bullet.Bounds))
                    {
                        closeGoblin.CheckBulletCollision(bullet);
                        bulletsToRemove.Add(bullet);

                        SoundController.PlayDieFx();
                        break; 
                    }
                }
                for (int i = farGoblins.Count - 1; i >= 0; i--)
                {
                    var farGoblin = farGoblins[i];

                    if (farGoblin.enemyBounds.Intersects(bullet.Bounds))
                    {
                        farGoblin.CheckBulletCollision(bullet);
                        bulletsToRemove.Add(bullet);

                        SoundController.PlayDieFx();
                        break;
                    }
                }
                bullet.Update(gameTime);
            }

            foreach (var bullet in bulletsToRemove)
            {
                bullets.Remove(bullet);
            }


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
                    if(slimes.Count == 0 && stage == 1)
                    {
                        SoundController.PlayDoorOpenFX();
                        LoadSecondScene();
                    }
                    else if(slimes.Count == 0 && closeGoblins.Count == 0  && stage == 2)
                    {
                        SoundController.PlayDoorOpenFX();
                        LoadSecondScene();
                    }
                    else if(slimes.Count == 0 && closeGoblins.Count == 0 && farGoblins.Count == 0 && stage == 3)
                    {
                        SoundController.PlayDoorOpenFX();
                        LoadSecondScene();
                    }
                    else
                    {

                        SoundController.PlayDoorLockedFX();
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

            bob.Update(gameTime);

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
                if (_timer >= DelayInSeconds && stage == 1)
                {
                    _timer = 0;
                    SoundController.PlayDoorOpenFX();
                    stage = 2;
                    LoadFightScene();
                } 
                if (_timer >= DelayInSeconds && stage == 2)
                {
                    _timer = 0;
                    SoundController.PlayDoorOpenFX();
                    stage = 3;
                    LoadFightScene();
                }
                if (_timer >= DelayInSeconds && stage == 3)
                {
                    //Pause the game first tho
                    // Ask for a username and store that to the database with the score if the score is higher than the highest one
                    //Save the records of the player to database and then display the 
                }
            }
            else
            {
                house.touch = false;
            }

            if (bob.playerBounds.Intersects(player.playerBounds))
            {
                bob.Stop();
                _gameManager.runBobDialogue(stage);
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
        //Call gameoover feature on if life is <0
        //Call win() if last list of enemies is clear
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var mouseState = Mouse.GetState();
            var mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (currentScene == "outside")
            {
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
                mapManager.Draw(matrix, _translation);
                fromHouse.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                player.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                bob.Draw(_spriteBatch, matrix, transformMatrix: _translation);
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
            else if (currentScene == "first" || currentScene == "second" || currentScene == "boss")
            {
                mapManager.Draw(matrix, _translation);
                house.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                if(player.GetLife() > 0)
                {
                    player.Draw(_spriteBatch, matrix, transformMatrix: _translation);
                }
                _gameManager.Draw();

                foreach (var bullet in bullets)
                {
                    if (bullet.hasHit == false)
                    {
                    bullet.Draw(_spriteBatch, Matrix.Identity, Matrix.Identity);
                    }
                }

                if (currentScene == "first")
                {
                    DrawEnemies(slimes);
                }
                else if (currentScene == "second")
                {
                    DrawEnemies(slimes);
                    DrawEnemies(closeGoblins);
                }
                else if (currentScene == "boss")
                {
                    DrawEnemies(slimes);
                    DrawEnemies(closeGoblins);
                    DrawEnemies(farGoblins);
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

        private void DrawEnemies<T>(List<T> enemies) where T : ParentEnemy
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i].isDead)
                {

                    enemies.RemoveAt(i);
                }
                else
                {
                    enemies[i].Draw(_spriteBatch, matrix, _translation);
                }
            }
        }

    }
}
