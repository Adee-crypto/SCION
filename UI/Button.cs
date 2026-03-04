using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Controllers;
using Sprint2.Util;
using System;

namespace Sprint2.UI;

public class Button
{
    private readonly SpriteFont font;
    private readonly Texture2D texture;
    private readonly string text;
    private readonly Action onClick;
    private Rectangle bounds;
    private Vector2 textPos;
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

    public void Resize((int w, int h) screenSize)
    {
        Vector2 size = new(bounds.Width, bounds.Height);
        Vector2 pos = new(bounds.X, bounds.Y);
        if (screenSize != Consts.DefaultScreenSize)
        {
            float widthRatio = screenSize.w / (float)Consts.DefaultScreenSize.w;
            float heightRatio = screenSize.h / (float)Consts.DefaultScreenSize.h;
            size *= new Vector2(widthRatio, heightRatio);
            pos *= new Vector2(widthRatio, heightRatio);
        }
        bounds = new((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        textPos = pos + 0.5f * (size - font.MeasureString(text));
    }

    public void Update()
    {
        hover = bounds.Contains(Mouse.GetState().Position);
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