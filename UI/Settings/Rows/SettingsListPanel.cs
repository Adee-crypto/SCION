using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Extensions;

namespace Sprint2.UI.Settings;

public abstract class SettingsListPanel(Game1 game) : BasePanel(game)
{
    protected List<ISettingsRow> Rows { get; } = [];

    protected virtual int RowSpacing => 12;

    protected override void OnBuildPanel()
    {
        Rows.Clear();
        BuildRows();
        LayoutRows();
    }

    protected abstract void BuildRows();

    protected void LayoutRows()
    {
        int y = ContentBounds.Y - ScrollOffset;
        int contentHeight = 0;

        foreach (ISettingsRow row in Rows)
        {
            Rectangle rowBounds = new(
                ContentBounds.X,
                y,
                ContentBounds.Width,
                row.Height
            );

            row.SetBounds(rowBounds);

            y += row.Height + RowSpacing;
            contentHeight += row.Height + RowSpacing;
        }

        if (Rows.Count > 0) contentHeight -= RowSpacing;

        RecalculateMaxScroll(contentHeight);
    }

    protected override void HandleInput(MouseState mouse, KeyboardState keyboard)
    {
        Rows.ForEach(r => r.Update(mouse, PreviousMouse, keyboard, PreviousKeyboard));
    }

    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        Rows.ForEach(r => r.Draw(spriteBatch));
    }

    protected override void DrawScrollbar(SpriteBatch spriteBatch)
    {
        
    }
}