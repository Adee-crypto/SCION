using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Controllers;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.UI.Settings;

public class ActionRow(string actionLabel, Action theAction) : ISettingsRow
{
    private readonly string label = actionLabel;
    private readonly Action action = theAction;

    private Rectangle bounds;
    private bool hover;

    public int Height => 50;

    public void SetBounds(Rectangle bounds)
    {
        this.bounds = bounds;
    }

    public void Update(MouseState mouse, MouseState previousMouse, KeyboardState keyboard, KeyboardState previousKeyboard)
    {
        hover = bounds.Contains(MouseController.VirtualMousePos);

        bool clicked = mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released;

        if (clicked && hover)
        {
            action?.Invoke();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Assets.ButtonTexture, bounds, hover ? Color.LightGray : Color.White);

        Vector2 textSize = Assets.UiFont.MeasureString(label);
        Vector2 textPos = new(
            bounds.X + (bounds.Width - textSize.X) / 2f,
            bounds.Y + (bounds.Height - textSize.Y) / 2f
        );

        spriteBatch.DrawString(Assets.UiFont, label, textPos, Color.Black);
    }
}