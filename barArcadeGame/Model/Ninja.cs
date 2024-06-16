namespace barArcadeGame;

using barArcadeGame.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

public class Ninja
{

    private Vector2 _position = new(155, 200);
    public Vector2 Velocity { get; set; }
    public Vector2 Static { get; set; }
    private readonly float _speed = 50f;
    public float Rotation { get; set; }
    private readonly AnimationController _anims = new();
    public Rectangle playerBounds;
    public float Speed { get; set; }
    public float MaxX { get; set; } // Maximum X coordinate to move to
    public float MinX { get; set; } // Minimum X coordinate to move to
    public bool toStop { get; set; } 

    public Ninja()
    {
        Speed = 10;
        MaxX = 230;
        MinX = 100;
        toStop = false;
        Velocity = new Vector2(1, 1);
        var ninjaTexture = Globals.Content.Load<Texture2D>("picture/ninja");
        playerBounds = new Rectangle((int)_position.X/*centered at centre*/, (int)_position.Y, 45,45);    
        _anims.AddAnimation(new Vector2(0, 1), new(ninjaTexture, 4, 7, 0.1f, 1));
        _anims.AddAnimation(new Vector2(-1, 0), new(ninjaTexture, 4, 7, 0.1f, 2));
        _anims.AddAnimation(new Vector2(1, 0), new(ninjaTexture, 4, 7, 0.1f, 3));
        _anims.AddAnimation(new Vector2(0, -1), new(ninjaTexture, 4, 7, 0.1f, 4));
        _anims.AddAnimation(new Vector2(-1, 1), new(ninjaTexture, 4, 7, 0.1f, 5));
        _anims.AddAnimation(new Vector2(-1, -1), new(ninjaTexture, 4, 7, 0.1f, 6));
        _anims.AddAnimation(new Vector2(1, 1), new(ninjaTexture, 4, 7, 0.1f, 7));
        _anims.AddAnimation(new Vector2(1, -1), new(ninjaTexture, 4, 7, 0.1f, 8));
        _anims.AddAnimation(new Vector2(0, 0), new(ninjaTexture, 4, 7, 20f));
    }

    public void Update()
    {
        // Check if NPC reached the boundaries
        if (!toStop)
        {
            _anims.Update(Velocity);
            //Velocity = new Vector2(1, 0);
            Move();
            if (_position.X >= MaxX)
            {
                // Reverse the direction
                Velocity = new Vector2(-1, -1);
            }
            else if (_position.X <= MinX)
            {
                Velocity = new Vector2(1, 1);
            }
        }
        if(toStop)
        {
            Static = new Vector2(0, 0);
            _anims.Update(Static);         
        }

        //_anims.Update(Velocity);
        //aInputManagerJack.Update(Velocity);

        playerBounds = new Rectangle((int)_position.X/*centered at centre*/, (int)_position.Y , 45, 45);
    }

    public void Move()
    {
        // Move the NPC based on its velocity and speed
        _position += Vector2.Normalize(Velocity) * _speed * Globals.Time;
    }

    public void stop()
    {
        toStop = true;
    }

    public void walk()
    {
        toStop = false;
    }

    public void Draw(SpriteBatch spriteBatch, Matrix matrix, Matrix transformMatrix)
    {
        spriteBatch.Begin(//All of these need to be here :(
            SpriteSortMode.Deferred,
            samplerState: SamplerState.PointClamp,
            effect: null,
            blendState: null,
            rasterizerState: null,
            depthStencilState: null,
            transformMatrix: transformMatrix);/*<-This is the main thing*/
        _anims.DrawNinja(_position);

        //Rectangle rectangle = new Rectangle(100, 100, 200, 100); // Example rectangle parameters
        Color rectangleColor = Color.Yellow; // Example rectangle color
        spriteBatch.DrawRectangle(playerBounds, rectangleColor);
        spriteBatch.End();
    }
}
