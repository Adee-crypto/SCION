using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.UI.Settings;

public class GraphicsPanel : ButtonListPanel
{
    public GraphicsPanel(Game1 game) : base(game)
    {
        BuildPanel();
    }

    protected override void BuildButtons()
    {
        AddButtonRow("Full-Screen", () => { }, 0);
        AddButtonRow("Particles", () => { }, 1);
    }
}