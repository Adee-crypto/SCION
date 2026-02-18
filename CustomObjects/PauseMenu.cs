using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Interfaces;

namespace Sprint2.UI;

public class PauseMenu
{
    private readonly SpriteFont font;
    private readonly Texture2D overlay;
    private readonly List<Button> buttons = new();

    public PauseMenu(SpriteFont font, GraphicsDevice graphicsDevice)
    {
        this.font = font;

        overlay = new Texture2D(graphicsDevice, 1, 1);
        overlay.SetData(new[] { Color.White });
    }

    public void AddButton(Button button) => buttons.Add(button);

    public void Update(IMouseController mouseController)
    {
        foreach (var button in buttons)
        {
            button.Update(mouseController);
        }
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        string pauseText = "GAME PAUSED";

        Vector2 textSize = font.MeasureString(pauseText);
        Vector2 center = new Vector2((graphicsDevice.Viewport.Width - textSize.X) / 2, (graphicsDevice.Viewport.Height - textSize.Y) / 2);
        
        spriteBatch.Draw(overlay, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.Black * 0.5f);

        Vector2 textPosition = new Vector2(center.X, center.Y / 4);
        spriteBatch.DrawString(font, pauseText, textPosition, Color.Black);

        foreach (var b in buttons)
        {
            b.Draw(spriteBatch);
        }
    }
}