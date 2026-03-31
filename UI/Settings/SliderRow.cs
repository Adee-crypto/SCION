using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.UI.Settings;

public class SliderRow(string rowLabel, float minVal, float maxVal, Func<float> getValueFunc, Action<float> setValueFunc) : ISettingsRow
{
    private readonly string label = rowLabel;
    private readonly float min = minVal;
    private readonly float max = maxVal;
    private readonly Func<float> getValue = getValueFunc;
    private readonly Action<float> setValue = setValueFunc;

    private Rectangle bounds;
    private Rectangle sliderTrack;
    private Rectangle knobBounds;

    private bool dragging;

    public int Height => 60;

    public void SetBounds(Rectangle bounds)
    {
        this.bounds = bounds;

        sliderTrack = new Rectangle(
            bounds.Right - 220,
            bounds.Y + bounds.Height / 2 - 4,
            180,
            8
        );

        UpdateKnobBounds();
    }

    private void UpdateKnobBounds()
    {
        float value = getValue();
        float percent = (value - min) / (max - min);
        percent = MathHelper.Clamp(percent, 0f, 1f);

        int knobX = sliderTrack.X + (int)(sliderTrack.Width * percent) - 8;

        knobBounds = new Rectangle(
            knobX,
            sliderTrack.Y - 6,
            16,
            20
        );
    }

    public void Update(MouseState mouse, MouseState previousMouse, KeyboardState keyboard, KeyboardState previousKeyboard)
    {
        bool pressed = mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released;
        bool released = mouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed;

        if (pressed && (knobBounds.Contains(mouse.Position) || sliderTrack.Contains(mouse.Position))) dragging = true;
        if (released) dragging = false;
        if (dragging)
        {
            float percent = (mouse.X - sliderTrack.X) / (float)sliderTrack.Width;
            percent = Math.Clamp(percent, 0f, 1f);

            float value = MathHelper.Lerp(min, max, percent);
            setValue(value);
            UpdateKnobBounds();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        float value = getValue();

        spriteBatch.Draw(Assets.ButtonTexture, bounds, Color.White);
        spriteBatch.DrawString(Assets.UiFont, label, new Vector2(bounds.X + 12,  bounds.Y + 8), Color.Black);

        string valueText = $"{value:0.##}";
        spriteBatch.DrawString(Assets.UiFont, valueText, new Vector2(bounds.X + 12, bounds.Y + 30), Color.Black);

        spriteBatch.Draw(Assets.PixelTexture, sliderTrack, Color.DarkGray);
        spriteBatch.Draw(Assets.ButtonTexture, knobBounds, Color.White);
    }
}