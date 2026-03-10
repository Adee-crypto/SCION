using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.UI.Settings;

public class ControlsPanel : ISettingsPanel 
{
    private readonly Menu panel;
    private readonly Game1 game;
    public ControlsPanel(Game1 game)
    {
        this.game = game;

        panel = new(Assets.UiFont) { Title = "", DimBackground = false };

        BuildPanel();
    }

    public void BuildPanel()
    {
        panel.ClearButtons();

        Vector2 buttonSize = new(400, 50);
        float spacer = 18f;

        float x = (game.GraphicsDevice.Viewport.Width - buttonSize.X) * 0.75f;
        float y = game.GraphicsDevice.Viewport.Height * 0.3f;

        panel.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Move Right", () => {}, buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 0)));
        panel.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Move Left", () => {}, buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 1)));
        panel.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Jump", () => {}, buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 2)));
        panel.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Break Block Below (Hold)", () => {}, buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 3)));
    
    }

    public void Resize((int w, int h) size)
    {
        BuildPanel();
    }

    public void Update()
    {
        panel.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        panel.Draw(spriteBatch, (game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height));
    }
}