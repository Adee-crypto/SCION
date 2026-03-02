using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;
using System.Collections.Generic;

namespace Sprint2.UI;

public class PauseMenu(SpriteFont font)
{
    private readonly SpriteFont font = font;
    private readonly List<Button> buttons = [];

    public void AddButton(Button button) => buttons.Add(button);

    public void Update(IMouseController mouseController)
    {
        buttons.ForEach(b => b.Update(mouseController));
    }

    public void Draw(SpriteBatch spriteBatch, (int w, int h) screenSize)
    {
        string pauseText = "GAME PAUSED";

        Vector2 textSize = font.MeasureString(pauseText);
        Vector2 center = new((screenSize.w - textSize.X) / 2, (screenSize.h - textSize.Y) / 2);

        spriteBatch.Draw(Assets.PauseMenuTexture, new Rectangle(0, 0, screenSize.w, screenSize.h), Color.Black * 0.5f);

        Vector2 textPosition = new(center.X, center.Y / 4);
        spriteBatch.DrawString(font, pauseText, textPosition, Color.Black);

        buttons.ForEach(b => b.Draw(spriteBatch));
    }
}