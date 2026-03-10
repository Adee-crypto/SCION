using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.UI;
using Sprint2.UI.Settings;
using Sprint2.Util;

namespace Sprint2.Screens;

public class ScreenSettings : IScreen, IResizableScreen
{
    private readonly Game1 game;
    private readonly ScreenManager screenManager;
    private readonly Menu menu;
    private SettingsTab currentTab = SettingsTab.General;
    private Dictionary<SettingsTab, ISettingsPanel> panels;
    public ISettingsPanel CurrentPanel => panels[currentTab];
    
    public ScreenSettings(Game1 game, ScreenManager screenManager)
    {
        this.game = game;
        this.screenManager = screenManager;

        menu = new(Assets.UiFont) { Title = "Settings: General", DimBackground = true };

        panels = new Dictionary<SettingsTab, ISettingsPanel>
        {
            { SettingsTab.General, new GeneralPanel(game) },
            { SettingsTab.Graphics, new GraphicsPanel(game) },
            { SettingsTab.Controls, new ControlsPanel(game) },
        };
    }

    public void OnEnter()
    {
        BuildMenu();
    }

    private void BuildMenu()
    {
        menu.ClearButtons();

        Vector2 buttonSize = new(200, 50);
        float spacer = 18f;

        float x = (game.GraphicsDevice.Viewport.Width - buttonSize.X) / 8;
        float y = game.GraphicsDevice.Viewport.Height * 0.3f;

        menu.AddButton(new(
            Assets.UiFont, 
            Assets.ButtonTexture, 
            "General", 
            () => {
                currentTab = SettingsTab.General;
                menu.Title = "Settings: General";
            },
            buttonSize, 
            new Vector2(x, y + (buttonSize.Y + spacer) * 0)
        ));
        menu.AddButton(new(
            Assets.UiFont, 
            Assets.ButtonTexture, 
            "Graphics", 
            () => {
                currentTab = SettingsTab.Graphics;
                menu.Title = "Settings: Graphics";
            },
            buttonSize, 
            new Vector2(x, y + (buttonSize.Y + spacer) * 1)
        ));
        menu.AddButton(new(
            Assets.UiFont, 
            Assets.ButtonTexture, 
            "Controls", 
            () => {
                currentTab = SettingsTab.Controls;
                menu.Title = "Settings: Controls";
            }, 
            buttonSize, 
            new Vector2(x, y + (buttonSize.Y + spacer) * 2)
        ));
        menu.AddButton(new(
            Assets.UiFont, 
            Assets.ButtonTexture, 
            "Back", 
            () => screenManager.SetScreen(new ScreenMainMenu(game, screenManager)), 
            buttonSize, 
            new Vector2(x, y + (buttonSize.Y + spacer) * 4)
        ));
    }

    public void OnExit() { }

    public void Resize((int w, int h) size)
    {
        BuildMenu();
        panels[currentTab].Resize(size);
    }

    public void Update(GameTime gameTime)
    {
        menu.Update();
        panels[currentTab].Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        menu.Draw(spriteBatch, (game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height));
        panels[currentTab].Draw(spriteBatch);
    }

}