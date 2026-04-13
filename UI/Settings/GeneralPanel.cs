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
        Rows.Add(new SliderRow(
            "Master Volume",
            0f,
            100f,
            () => Tunables.MasterVolume.Value,
            v => Tunables.MasterVolume.Value = v
        ));
        Rows.Add(new SliderRow(
            "Music Volume",
            0f,
            100f,
            () => Tunables.MusicVolume.Value,
            v => Tunables.MusicVolume.Value = v
        ));
        Rows.Add(new SliderRow(
            "SFX Volume",
            0f,
            100f,
            () => Tunables.SFXVolume.Value,
            v => Tunables.SFXVolume.Value = v
        ));
        
    }
}