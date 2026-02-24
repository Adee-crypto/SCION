using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Advait;

public interface IPlantable
{
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    void StartVerticalGrowth();
}
