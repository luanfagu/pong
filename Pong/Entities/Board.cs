using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Startup;

namespace Pong.Entities
{
    public class Board: BaseEntity
    {
        
        private Texture2D boardSprite = MlemGame.LoadContent<Texture2D>("board");
        
        public Board(Vector2 position) : base(position)
        {
        }

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            batch.Draw(boardSprite, position, Color.White);
            base.Draw(time, batch);
        }
    }
}