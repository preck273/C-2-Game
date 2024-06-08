namespace barArcadeGame;

using barArcadeGame.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

public class Jack
{
    private Vector2 _position = new(0, 0);
    public Vector2 Velocity { get; set; }
    public Vector2 Static { get; set; }
    private readonly float _speed = 50f;
    public float Rotation { get; set; }
    private readonly AnimationManager _anims = new();
    private readonly AnimationManager _stopAnims = new();
    public Rectangle playerBounds;
    public float Speed { get; set; }
    public float MaxX { get; set; }
    public float MinX { get; set; } 
    public bool toStop { get; set; } 

    public Jack()
    {
        Speed = 10;
        MaxX = 100;
        MinX = 0;
        toStop = false;
        Velocity = new Vector2(1, 1);
        var heroTexture = Globals.Content.Load<Texture2D>("picture/hero");
        playerBounds = new Rectangle((int)_position.X-40/*centered at centre*/, (int)_position.Y-40, 16, 17);    
        _anims.AddAnimation(new Vector2(0, 1), new(heroTexture, 8, 8, 0.1f, 1));
        _anims.AddAnimation(new Vector2(-1, 0), new(heroTexture, 8, 8, 0.1f, 2));
        _anims.AddAnimation(new Vector2(1, 0), new(heroTexture, 8, 8, 0.1f, 3));
        _anims.AddAnimation(new Vector2(0, -1), new(heroTexture, 8, 8, 0.1f, 4));
        _anims.AddAnimation(new Vector2(-1, 1), new(heroTexture, 8, 8, 0.1f, 5));
        _anims.AddAnimation(new Vector2(-1, -1), new(heroTexture, 8, 8, 0.1f, 6));
        _anims.AddAnimation(new Vector2(1, 1), new(heroTexture, 8, 8, 0.1f, 7));
        _anims.AddAnimation(new Vector2(1, -1), new(heroTexture, 8, 8, 0.1f, 8));
        _anims.AddAnimation(new Vector2(0, 0), new(heroTexture, 8, 8, 2000f));
    }

    public void Update()
    {
        // Update NPC logic here

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

        playerBounds = new Rectangle((int)_position.X + 35/*centered at centre*/, (int)_position.Y + 10, 30, 80);
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
        _anims.Draw(_position);

        //Rectangle rectangle = new Rectangle(100, 100, 200, 100); // Example rectangle parameters
        Color rectangleColor = Color.Red; // Example rectangle color
        spriteBatch.DrawRectangle(playerBounds, rectangleColor);
        spriteBatch.End();
    }
}
