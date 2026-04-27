using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Controllers;
using System;

namespace Sprint2.UI;

public class Button
{
    private readonly SpriteFont font;
    private readonly Texture2D texture;
    private readonly string text;
    private readonly Action onClick;
    private Rectangle bounds;
    public Rectangle Bounds => bounds;
    private Vector2 textPos;
    public Vector2 Position => textPos;
    private bool hover;

    public Button(SpriteFont textFont, Texture2D buttonTexture, string buttonText, Action command, Vector2 size, Vector2 pos)
    {
        font = textFont;
        texture = buttonTexture;
        text = buttonText;
        onClick = command;
        bounds = new((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        textPos = pos + 0.5f * (size - font.MeasureString(buttonText));
    }

    public void SetBounds(Rectangle newBounds)
    {
        bounds = newBounds;
        textPos = new Vector2(
            bounds.X + (bounds.Width - font.MeasureString(text).X) / 2f,
            bounds.Y + (bounds.Height - font.MeasureString(text).Y) / 2f
        );
    }

    public void Update()
    {
        hover = bounds.Contains(MouseController.VirtualMousePos);
        if (hover && MouseController.IsLeftClick()) onClick?.Invoke();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Color highlight = hover ? Color.LightGray : Color.White;
        spriteBatch.Draw(texture, bounds, highlight);
        Rectangle hoverBounds = hover ? new Rectangle(bounds.X - 2, bounds.Y - 2, bounds.Width + 4, bounds.Height + 4) : bounds;
        spriteBatch.Draw(texture, hoverBounds, highlight);
        spriteBatch.DrawString(font, text, textPos, Color.Black);
    }
}