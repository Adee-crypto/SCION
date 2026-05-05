using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Controllers;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.UI.Settings;

public class DevPanel : ISettingsPanel
{
    private readonly Game1 game;
    private readonly List<DevEntry> entries = [];

    private Rectangle panelBounds;
    private Rectangle contentBounds;

    private int scrollOffset;
    private int maxScroll;

    private MouseState previousMouse;
    private KeyboardState previousKeyboard;

    private EditableField activeField;

    private const int RowHeight = 56;
    private const int RowSpacing = 10;
    private const int HeaderHeight = 48;
    private const int Padding = 16;

    public DevPanel(Game1 game)
    {
        this.game = game;

        foreach ((string name, TunableFloat value) in Tunables.FloatSettings)
        {
            entries.Add(new DevEntry(
                name,
                () => value.Value.ToString("0.###", CultureInfo.InvariantCulture),
                text =>
                {
                    if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsed))
                    {
                        value.Value = parsed;
                        return true;
                    }

                    return false;
                },
                () => value.Reset()
            ));
        }

        foreach ((string name, TunableInt value) in Tunables.IntSettings)
        {
            entries.Add(new DevEntry(
                name,
                () => value.Value.ToString(CultureInfo.InvariantCulture),
                text =>
                {
                    if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed))
                    {
                        value.Value = parsed;
                        return true;
                    }

                    return false;
                },
                () => value.Reset()
            ));
        }
    }

    public void BuildPanel()
    {
        (int w, int h) = game.VirtualScreenSize.ToPoint();

        panelBounds = new Rectangle(
            (int)(w * 0.27f),
            (int)(h * 0.27f),
            (int)(w * 0.56f),
            (int)(h * 0.52f)
        );

        contentBounds = new Rectangle(
            panelBounds.X + Padding,
            panelBounds.Y + HeaderHeight,
            panelBounds.Width - (Padding * 2),
            panelBounds.Height - HeaderHeight - Padding
        );

        RecalculateScrollLimits();
    }

    public void Update()
    {
        MouseState mouse = Mouse.GetState();
        KeyboardState keyboard = Keyboard.GetState();

        HandleScroll(mouse);
        HandleMouse(mouse);
        HandleKeyboard(keyboard);

        previousMouse = mouse;
        previousKeyboard = keyboard;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Assets.PixelTexture, panelBounds, Color.White * 0.08f);
        spriteBatch.Draw(
            Assets.PixelTexture,
            new Rectangle(contentBounds.X, contentBounds.Y - 8, contentBounds.Width, 2),
            Color.Black * 0.25f
        );

        int y = contentBounds.Y - scrollOffset;

        for (int i = 0; i < entries.Count; i++)
        {
            Rectangle rowBounds = GetRowBounds(y);

            if (rowBounds.Bottom >= contentBounds.Y && rowBounds.Y <= contentBounds.Bottom)
            {
                DrawRow(spriteBatch, entries[i], rowBounds);
            }

            y += RowHeight + RowSpacing;
        }

        DrawScrollbar(spriteBatch);
    }

    private void HandleScroll(MouseState mouse)
    {
        int wheelDelta = mouse.ScrollWheelValue - previousMouse.ScrollWheelValue;
        if (wheelDelta == 0 || !contentBounds.Contains(MouseController.VirtualMousePos)) return;

        scrollOffset -= wheelDelta / 6;
        scrollOffset = Math.Clamp(scrollOffset, 0, maxScroll);
    }

    private void HandleMouse(MouseState mouse)
    {
        bool leftJustPressed =
            mouse.LeftButton == ButtonState.Pressed &&
            previousMouse.LeftButton == ButtonState.Released;

        if (!leftJustPressed) return;

        Point pos = MouseController.VirtualMousePos;
        bool clickedSomething = false;

        int y = contentBounds.Y - scrollOffset;

        for (int i = 0; i < entries.Count; i++)
        {
            Rectangle rowBounds = GetRowBounds(y);

            if (rowBounds.Bottom < contentBounds.Y || rowBounds.Y > contentBounds.Bottom)
            {
                y += RowHeight + RowSpacing;
                continue;
            }

            Rectangle valueBounds = GetValueBoxBounds(rowBounds);
            Rectangle resetBounds = GetResetButtonBounds(rowBounds);

            if (valueBounds.Contains(pos))
            {
                SetActiveField(entries[i], valueBounds);
                clickedSomething = true;
                break;
            }

            if (resetBounds.Contains(pos))
            {
                entries[i].Reset();

                if (activeField != null && activeField.Entry == entries[i])
                    activeField.Buffer = entries[i].GetDisplayValue();

                clickedSomething = true;
                break;
            }

            y += RowHeight + RowSpacing;
        }

        if (!clickedSomething && activeField != null)
        {
            CancelActiveField();
        }
    }

    private void HandleKeyboard(KeyboardState keyboard)
    {
        if (activeField == null) return;

        foreach (Keys key in keyboard.GetPressedKeys())
        {
            if (previousKeyboard.IsKeyUp(key))
            {
                HandleKeyPressed(key);
            }
        }
    }

    private void HandleKeyPressed(Keys key)
    {
        if (activeField == null) return;

        switch (key)
        {
            case Keys.Enter:
                ConfirmActiveField();
                return;

            case Keys.Escape:
                CancelActiveField();
                return;

            case Keys.Back:
                if (activeField.Buffer.Length > 0)
                    activeField.Buffer = activeField.Buffer[..^1];
                return;

            case Keys.OemPeriod:
            case Keys.Decimal:
                if (!activeField.Buffer.Contains('.'))
                    activeField.Buffer += ".";
                return;

            case Keys.OemMinus:
            case Keys.Subtract:
                if (activeField.Buffer.Length == 0)
                    activeField.Buffer = "-";
                return;
        }

        if (key >= Keys.D0 && key <= Keys.D9)
        {
            activeField.Buffer += (char)('0' + (key - Keys.D0));
            return;
        }

        if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
        {
            activeField.Buffer += (char)('0' + (key - Keys.NumPad0));
        }
    }

    private void SetActiveField(DevEntry entry, Rectangle bounds)
    {
        if (activeField != null && activeField.Entry == entry)
        {
            activeField.Bounds = bounds;
            return;
        }

        activeField = new EditableField(entry, bounds, entry.GetDisplayValue());
    }

    private void ConfirmActiveField()
    {
        if (activeField == null) return;

        activeField.Entry.TrySetValue(activeField.Buffer);
        activeField.Buffer = activeField.Entry.GetDisplayValue();
        activeField = null;
    }

    private void CancelActiveField()
    {
        if (activeField == null) return;

        activeField.Buffer = activeField.Entry.GetDisplayValue();
        activeField = null;
    }

    private Rectangle GetRowBounds(int y)
    {
        return new Rectangle(
            contentBounds.X,
            y,
            contentBounds.Width,
            RowHeight
        );
    }

    private static Rectangle GetValueBoxBounds(Rectangle rowBounds)
    {
        return new Rectangle(
            rowBounds.Right - 180,
            rowBounds.Y + 8,
            100,
            40
        );
    }

    private static Rectangle GetResetButtonBounds(Rectangle rowBounds)
    {
        return new Rectangle(
            rowBounds.Right - 70,
            rowBounds.Y + 8,
            60,
            40
        );
    }

    private void DrawRow(SpriteBatch spriteBatch, DevEntry entry, Rectangle rowBounds)
    {
        Rectangle valueBounds = GetValueBoxBounds(rowBounds);
        Rectangle resetBounds = GetResetButtonBounds(rowBounds);

        bool isActive = activeField != null && activeField.Entry == entry;
        string valueText = isActive ? activeField.Buffer : entry.GetDisplayValue();

        spriteBatch.Draw(Assets.PixelTexture, rowBounds, Color.White * 0.12f);

        spriteBatch.DrawString(
            Assets.UiFont,
            entry.Name,
            new Vector2(rowBounds.X + 12, rowBounds.Y + 14),
            Color.Black
        );

        spriteBatch.Draw(
            Assets.PixelTexture,
            valueBounds,
            isActive ? Color.White : Color.LightGray
        );

        spriteBatch.DrawString(
            Assets.UiFont,
            valueText,
            new Vector2(valueBounds.X + 8, valueBounds.Y + 8),
            Color.Black
        );

        spriteBatch.Draw(Assets.PixelTexture, resetBounds, Color.LightGray);

        Vector2 resetTextSize = Assets.UiFont.MeasureString("Reset");
        spriteBatch.DrawString(
            Assets.UiFont,
            "Reset",
            new Vector2(
                resetBounds.X + (resetBounds.Width - resetTextSize.X) / 2f,
                resetBounds.Y + (resetBounds.Height - resetTextSize.Y) / 2f
            ),
            Color.Black
        );
    }

    private void RecalculateScrollLimits()
    {
        int totalHeight = entries.Count * RowHeight + Math.Max(0, entries.Count - 1) * RowSpacing;
        maxScroll = Math.Max(0, totalHeight - contentBounds.Height);
        scrollOffset = Math.Clamp(scrollOffset, 0, maxScroll);
    }

    private void DrawScrollbar(SpriteBatch spriteBatch)
    {
        if (maxScroll <= 0) return;

        Rectangle track = new(
            panelBounds.Right - 8,
            contentBounds.Y,
            4,
            contentBounds.Height
        );

        float visibleRatio = contentBounds.Height / (float)(contentBounds.Height + maxScroll);
        int thumbHeight = Math.Max(20, (int)(track.Height * visibleRatio));

        float scrollRatio = scrollOffset / (float)maxScroll;
        int thumbY = track.Y + (int)((track.Height - thumbHeight) * scrollRatio);

        Rectangle thumb = new(track.X, thumbY, track.Width, thumbHeight);

        spriteBatch.Draw(Assets.PixelTexture, track, Color.Black * 0.15f);
        spriteBatch.Draw(Assets.PixelTexture, thumb, Color.Black * 0.45f);
    }

    private sealed class DevEntry(string name, Func<string> getDisplayValue, Func<string, bool> trySetValue, Action reset)
    {
        public string Name { get; } = name;
        public Func<string> GetDisplayValue { get; } = getDisplayValue;
        public Func<string, bool> TrySetValue { get; } = trySetValue;
        public Action Reset { get; } = reset;
    }

    private sealed class EditableField(DevEntry entry, Rectangle bounds, string buffer)
    {
        public DevEntry Entry { get; } = entry;
        public Rectangle Bounds { get; set; } = bounds;
        public string Buffer { get; set; } = buffer;
    }
}