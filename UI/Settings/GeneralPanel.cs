using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.UI.Settings;

public class GeneralPanel : ButtonListPanel
{
    public GeneralPanel(Game1 game) : base(game)
    {
        BuildPanel();
    }

    protected override void BuildButtons()
    {
        AddButtonRow("Master Volume", () => { }, 0);
        AddButtonRow("Music Volume", () => { }, 1);
        AddButtonRow("FX Volume", () => { }, 2);
    }
}