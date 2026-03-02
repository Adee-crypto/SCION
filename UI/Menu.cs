using Sprint2.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;
using System.Collections.Generic;

namespace Sprint2.UI;

public class Menu
{
    private readonly SpriteFont font = font;
    private readonly List<Button> buttons = [];

    public string Title { get; set; } = "";
    public bool DimBackground { get; set; } = true;

    public Menu(SpriteFont font, GraphicsDevice graphicsDevice)
    {
        this.font = font;
    }

    public void AddButton(Button button) => buttons.Add(button);
    public void ClearButtons() => buttons.Clear();

    public void Update(IMouseController mouseController)
    {
        buttons.ForEach(b => b.Update(mouseController));
    }

    public void Draw(SpriteBatch spriteBatch, (int w, int h) screenSize)
    {
        if (DimBackground) spriteBatch.Draw(Assets.PauseMenuTexture, new Rectangle(0, 0, screenSize.w, screenSize.h), Color.Black * 0.5f);

        if (!string.IsNullOrWhiteSpace(Title))
        {
            Vector2 textSize = font.MeasureString(Title);
            Vector2 titlePos = new((screenSize.w - textSize.X) / 2, (screenSize.h - textSize.Y) / buttons.Count);
            spriteBatch.DrawString(font, Title, titlePos, Color.Black);
        }

        buttons.ForEach(b => b.Draw(spriteBatch));
    }
}