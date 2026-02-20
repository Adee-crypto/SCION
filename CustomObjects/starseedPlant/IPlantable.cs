using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IPlantable
{
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    void StartVerticalGrowth();
}
