using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Startup;

namespace Pong.Entities
{
    public class Player: BaseEntity
    {
        private Texture2D playerSprite = MlemGame.LoadContent<Texture2D>("player");
        private float _maxSpeed = 6f;
        private float _acceleration = 0.4f;
        private float _velocity;

        public int Points = 0;

        private ControlType _controlType;

        public Player(Vector2 position, ControlType controlType = ControlType.WASD) : base(position)
        {
            _controlType = controlType;
        }

        public override void Update(GameTime time)
        {
            if (MlemGame.Input.IsAnyDown(_controlType == ControlType.WASD ? Keys.W : Keys.Up))
            {
                if (_velocity < _maxSpeed)
                    _velocity += _acceleration;
                if (position.Y > 16)
                    position.Y -= _velocity;
                else
                    position.Y = 16;
            } else if (MlemGame.Input.IsAnyDown(_controlType == ControlType.WASD ? Keys.S : Keys.Down)) {
                if (_velocity < _maxSpeed)
                    _velocity += _acceleration;
                if (position.Y + 128 < Pong.Instance.GraphicsDevice.Viewport.Height - 16)
                    position.Y += _velocity;
                else
                    position.Y = Pong.Instance.GraphicsDevice.Viewport.Height - 144;
            }
            else
            {
                _velocity = 0;
            }
        }

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            batch.Draw(playerSprite, position, Color.White);
            base.Draw(time, batch);
        }
        
    }

    public enum ControlType
    {
        WASD,
        ARROWS
    }
}