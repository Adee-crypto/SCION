using Microsoft.Xna.Framework;

namespace Sprint2.Interfaces
{
    public interface IPlayer
    {
        public void ChangeDirection(Vector2 direction);
        public void Update(GameTime gameTime);
        public void Draw();
    }
}
