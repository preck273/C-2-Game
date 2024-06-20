using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace barArcadeGame.Model
{
    public class Bullet
    {
        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public float Speed { get; private set; }
        public int damage { get; set; }
        public bool hasHit{ get; set; }
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 10, 10);
        private Texture2D texture;

        public Bullet(Vector2 position, Vector2 direction, float speed, Texture2D texture, int damage)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
            this.texture = texture;
            this.damage = damage;
            hasHit = false;
        }


        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Direction * Speed * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch, Matrix matrix, Matrix translation)
        {
            spriteBatch.Begin(transformMatrix: translation);
            Vector2 scale = new Vector2(0.025f, 0.025f); 
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

    }
}
