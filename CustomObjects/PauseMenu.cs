using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Interfaces;

namespace Sprint2.UI;

public class PauseMenu
{
    private readonly SpriteFont font;
    private readonly Texture2D overlay;
    private readonly List<Button> buttons = [];

    public PauseMenu(SpriteFont font, GraphicsDevice graphicsDevice)
    {
        this.font = font;

        overlay = new Texture2D(graphicsDevice, 1, 1);
        overlay.SetData(new[] { Color.White });
    }

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
        
        spriteBatch.Draw(overlay, new Rectangle(0, 0, screenSize.w, screenSize.h), Color.Black * 0.5f);

        Vector2 textPosition = new(center.X, center.Y / 4);
        spriteBatch.DrawString(font, pauseText, textPosition, Color.Black);

        buttons.ForEach(b => b.Draw(spriteBatch));
    }
}