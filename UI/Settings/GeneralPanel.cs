using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.UI.Settings;

public class GeneralPanel : SettingsListPanel
{
    public GeneralPanel(Game1 game) : base(game)
    {
        BuildPanel();
    }

    protected override void BuildRows()
    {
        // TODO: Implement General Settings
        
        /*Rows.Add(new SliderRow("Master Volume", 0f, 100f,
            () => Tunables.MasterVolume.Value,
            v => Tunables.MasterVolume.Value = v
        ));*/
    }
}