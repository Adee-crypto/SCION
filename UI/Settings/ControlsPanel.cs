using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.UI.Settings;

public class ControlsPanel : ButtonListPanel
{
    public ControlsPanel(Game1 game) : base(game)
    {
        BuildPanel();
    }

    protected override void BuildButtons()
    {
        AddButtonRow("Move Right", () => { }, 0);
        AddButtonRow("Move Left", () => { }, 1);
        AddButtonRow("Jump", () => { }, 2);
        AddButtonRow("Break Block Below (Hold)", () => { }, 3);
    }
}