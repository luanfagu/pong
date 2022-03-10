using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Entities
{
    public class BaseEntity
    {
        public Vector2 position;

        private Vector2 _originalPosition;

        public BaseEntity(Vector2 position)
        {
            this.position = position;
            this._originalPosition = position;
        }
        
        public virtual void Update(GameTime time) {
        }

        public virtual void Draw(GameTime time, SpriteBatch batch) {
        }

        public void ResetPosition()
        {
            position = _originalPosition;
        }
    }
}