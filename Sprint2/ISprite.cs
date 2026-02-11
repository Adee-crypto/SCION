using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

interface ISprite
{
    void Draw(SpriteBatch batch);
    void Update(GameTime gameTime);
}