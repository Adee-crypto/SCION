using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Util;
using Sprint2.Util.Settings;

namespace Sprint2.UI.Settings;

public abstract class ButtonListPanel : BasePanel
{
    protected List<Button> Buttons { get; } = [];

    protected virtual Vector2 ButtonSize => new(400, 50);
    protected virtual float RowSpacing => 18f;

    protected ButtonListPanel(Game1 game) : base(game) { }

    protected override void OnBuildPanel()
    {
        Buttons.Clear();

        BuildButtons();

        int totalHeight = 0;
        if (Buttons.Count > 0)
        {
            totalHeight = Buttons.Max(b => b.Bounds.Bottom) - ContentBounds.Y;
        }

        RecalculateMaxScroll(totalHeight);
    }

    protected abstract void BuildButtons();

    protected void AddButtonRow(string text, Action action, int index)
    {
        float x = ContentBounds.X;
        float y = ContentBounds.Y + index * (ButtonSize.Y + RowSpacing) - ScrollOffset;

        Buttons.Add(new Button(
            Assets.UiFont,
            Assets.ButtonTexture,
            text,
            action,
            ButtonSize,
            new Vector2(x, y)
        ));
    }

    protected override void HandleInput(MouseState mouse, KeyboardState keyboard)
    {
        Buttons.ForEach(b => b.Update());
    }

    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        foreach (Button button in Buttons)
        {
            if (button.Bounds.Bottom >= ContentBounds.Top && button.Bounds.Top <= ContentBounds.Bottom)
            {
                button.Draw(spriteBatch);
            }
        }
    }

    protected override void DrawScrollbar(SpriteBatch spriteBatch)
    {
        
    }
}