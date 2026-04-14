using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Screens;
using Sprint2.UI.Settings;
using Sprint2.Util;

namespace Sprint2.UI.Overlays;

<<<<<<< HEAD:UI/Overlays/SettingsOverlay.cs
public class SettingsOverlay : IOverlay, IResizable
=======
public class ScreenSettings : IScreen//, IResizableScreen
>>>>>>> 59edc3d (resizing logic simplified, fully works for ingame objects, some UI still needs fixing depending on what we want to do with it):Screens/ScreenSettings.cs
{
    private readonly Game1 game;
    private readonly OverlayManager overlayManager;
    private readonly Menu menu;
    private SettingsTab currentTab = SettingsTab.General;
    private readonly Dictionary<SettingsTab, ISettingsPanel> panels;
    public ISettingsPanel CurrentPanel => panels[currentTab];
    
    public SettingsOverlay(Game1 game, OverlayManager overlayManager)
    {
        this.game = game;
        this.overlayManager = overlayManager;

        menu = new(Assets.UiFont) { Title = "Settings: General" };

        panels = new Dictionary<SettingsTab, ISettingsPanel>
        {
            { SettingsTab.General, new GeneralPanel(game) },
            { SettingsTab.Graphics, new GraphicsPanel(game) },
            { SettingsTab.Controls, new ControlsPanel(game) },
            { SettingsTab.Dev, new DevPanel(game) }
        };
    }

    private bool IsOverMainMenu => game.ScreenManager.Current is ScreenMainMenu;

    public void OnOpen()
    {
        menu.DimBackground = !IsOverMainMenu;
        BuildMenu();
        foreach (ISettingsPanel panel in panels.Values) panel.BuildPanel();
    }

    public void OnClose() { }

    private void SetTab(SettingsTab tab, string title)
    {
        currentTab = tab;
        menu.Title = title;
    }

    private void BuildMenu()
    {
        menu.ClearButtons();

        Vector2 buttonSize = new(200, 50);
        float spacer = 18f;

        float x = (Consts.DefaultScreenSize.w - buttonSize.X) / 8;
        float y = Consts.DefaultScreenSize.h * 0.3f;

        menu.AddButton(new(
            Assets.UiFont, 
            Assets.ButtonTexture, 
            "General", 
            () => SetTab(SettingsTab.General, "Settings: General"),
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 0)
        ));
        menu.AddButton(new(
            Assets.UiFont, 
            Assets.ButtonTexture, 
            "Graphics", 
            () => SetTab(SettingsTab.Graphics, "Settings: Graphics"),
            buttonSize, 
            new Vector2(x, y + (buttonSize.Y + spacer) * 1)
        ));
        menu.AddButton(new(
            Assets.UiFont, 
            Assets.ButtonTexture, 
            "Controls", 
            () => SetTab(SettingsTab.Controls, "Settings: Controls"), 
            buttonSize, 
            new Vector2(x, y + (buttonSize.Y + spacer) * 2)
        ));
        menu.AddButton(new(
            Assets.UiFont, 
            Assets.ButtonTexture, 
            "Dev",
            () => SetTab(SettingsTab.Dev, "Settings: Dev"), 
            buttonSize, 
            new Vector2(x, y + (buttonSize.Y + spacer) * 2)
        ));
        menu.AddButton(new(
            Assets.UiFont, 
            Assets.ButtonTexture, 
            "Back", 
            () => overlayManager.Pop(), 
            buttonSize, 
            new Vector2(x, y + (buttonSize.Y + spacer) * 4)
        ));
    }

<<<<<<< HEAD:UI/Overlays/SettingsOverlay.cs
    public void Resize((int w, int h) size)
    {
        BuildMenu();
        foreach (ISettingsPanel panel in panels.Values) panel.Resize(size);
    }
=======
    public void OnExit() { }
>>>>>>> 59edc3d (resizing logic simplified, fully works for ingame objects, some UI still needs fixing depending on what we want to do with it):Screens/ScreenSettings.cs

    public void Update(GameTime gameTime)
    {
        menu.Update();
        CurrentPanel.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
<<<<<<< HEAD:UI/Overlays/SettingsOverlay.cs
        Rectangle backdrop = new(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        
        if (IsOverMainMenu) spriteBatch.Draw(Assets.PixelTexture, backdrop, Color.White);

        menu.Draw(spriteBatch, (game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height));
        CurrentPanel.Draw(spriteBatch);
=======
        menu.Draw(spriteBatch, game.VirtualScreenSize);
        panels[currentTab].Draw(spriteBatch);
>>>>>>> 59edc3d (resizing logic simplified, fully works for ingame objects, some UI still needs fixing depending on what we want to do with it):Screens/ScreenSettings.cs
    }

}