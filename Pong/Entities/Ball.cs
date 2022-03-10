using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Startup;

namespace Pong.Entities
{
    public class Ball: BaseEntity
    {
        private readonly Texture2D _ballSprite = MlemGame.LoadContent<Texture2D>("ball");
        private Vector2 _speed = new Vector2(600, 300);

        public Ball(Vector2 position) : base(position)
        {
        }

        private bool IsCollidingWithPlayer()
        {
            var playerOne = Pong.Instance.PlayerOne;
            var playerTwo = Pong.Instance.PlayerTwo;
            
            var isCollidingPlayerOne = playerOne.position.Y <= position.Y + 32 
                               && playerOne.position.Y + 128 >= position.Y 
                               && playerOne.position.X + 32 >= position.X
                               && playerOne.position.X + 25 < position.X;
            var isCollidingPlayerTwo = playerTwo.position.Y <= position.Y + 32 
                               && playerTwo.position.Y + 128 >= position.Y 
                               && playerTwo.position.X <= position.X + 32
                               && playerTwo.position.X > position.X + 25;
            
            return isCollidingPlayerOne || isCollidingPlayerTwo;
        }

        public override void Update(GameTime time)
        {
            position += _speed * (float)time.ElapsedGameTime.TotalSeconds;

            int maxX = Pong.Instance.GraphicsDevice.Viewport.Width - _ballSprite.Width;

            int maxY = Pong.Instance.GraphicsDevice.Viewport.Height - _ballSprite.Height - 16;

            if (position.X > maxX || position.X < 0)
            {
                if (position.X > maxX) Pong.Instance.PlayerOne.Points += 1;
                else Pong.Instance.PlayerTwo.Points += 1;
                _speed.X *= -1;
                this.ResetPosition();
            } 
            else if (IsCollidingWithPlayer())
            {
                _speed.X *= -1;
            }

            if (position.Y > maxY || position.Y < 16)
                _speed.Y *= -1; 
        }

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            batch.Draw(_ballSprite, position, Color.White);
        }
    }
}