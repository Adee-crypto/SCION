namespace Sprint2.UI.Settings;

public class GraphicsPanel : SettingsListPanel
{
    public GraphicsPanel(Game1 game) : base(game)
    {
        BuildPanel();
    }

    protected override void BuildRows()
    {
        // TODO: Implement Graphics Settings
        Rows.Add(new ToggleRow(
            "Fullscreen",
            () => Game.IsFullScreen,
            v => Game.ToggleFullscreen()
        ));
    }
}