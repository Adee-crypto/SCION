using Microsoft.Xna.Framework;


namespace Sprint2.Interfaces
{
    public interface IEntityStateMachine
    {
        public void ChangeDirection(Vector2 direction);
        public void Update(GameTime gameTime);
        public void Draw();
    }
}
