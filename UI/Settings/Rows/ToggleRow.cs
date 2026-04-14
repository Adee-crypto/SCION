using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Controllers;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.UI.Settings;

public class ToggleRow(string stringLabel, Func<bool> getValueFunc, Action<bool> setValueAction) : ISettingsRow
{
    private readonly string label = stringLabel;
    private readonly Func<bool> getValue = getValueFunc;
    private readonly Action<bool> setValue = setValueAction;

    private Rectangle bounds;
    private Rectangle toggleBounds;
    private bool hover;

    public int Height => 50;

    public void SetBounds(Rectangle bounds)
    {
        this.bounds = bounds;

        toggleBounds = new Rectangle(
            bounds.Right - 110,
            bounds.Y + 5,
            100,
            bounds.Height - 10
        );
    }

    public void Update(MouseState mouse, MouseState previousMouse, KeyboardState keyboard, KeyboardState previousKeyboard)
    {
        hover = toggleBounds.Contains(MouseController.VirtualMousePos);

        bool clicked = mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released;

        if (clicked && hover)
        {
            setValue(!getValue());
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Assets.ButtonTexture, toggleBounds, Color.White);
        spriteBatch.DrawString(Assets.UiFont, label, new Vector2(bounds.X + 12, bounds.Y + 12), Color.Black);

        bool value = getValue();
        Color toggleColor = value ? Color.LightGreen : Color.IndianRed;

        spriteBatch.Draw(Assets.ButtonTexture, toggleBounds, hover ? Color.LightGray : toggleColor);

        string text = value ? "ON" : "OFF";
        Vector2 textSize = Assets.UiFont.MeasureString(text);
        Vector2 textPos = new(
            toggleBounds.X + (toggleBounds.Width - textSize.X) / 2f,
            toggleBounds.Y + (toggleBounds.Height - textSize.Y) / 2f
        );

        spriteBatch.DrawString(Assets.UiFont, text, textPos, Color.Black);
    }
}