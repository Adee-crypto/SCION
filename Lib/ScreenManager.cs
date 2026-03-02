using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2.Screens;

public class ScreenManager
{
    private IScreen current;
    public IScreen Current => current;

    public void SetScreen(IScreen screen)
    {
        current?.OnExit();
        current = screen;
        current?.OnEnter();
    }

    public void Update(GameTime gameTime)
    {
        current.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        current.Draw(spriteBatch);
    }
}