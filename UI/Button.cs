using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Sprint2.UI;

public class Button
{
    private readonly SpriteFont font;
    private readonly Texture2D texture;
    private readonly string text;
    private readonly Action onClick;
    private readonly Rectangle bounds;
    private readonly Vector2 textPos;
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

    public void Update(IMouseController mouseController)
    {
        hover = bounds.Contains(Mouse.GetState().Position);
        if (hover && mouseController.IsLeftClick()) onClick?.Invoke();
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