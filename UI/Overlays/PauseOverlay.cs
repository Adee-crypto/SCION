using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Screens;
using Sprint2.Util;

namespace Sprint2.UI.Overlays;

public class PauseOverlay : IOverlay
{
    private readonly Game1 game;
    private readonly OverlayManager overlayManager;
    private readonly Menu menu;
    private readonly Vector2 buttonSize = new(300, 75);
    private readonly float spacer = 18f;
    public PauseOverlay(Game1 game, OverlayManager overlayManager)
    {
        this.game = game;
        this.overlayManager = overlayManager;

        menu = new(Assets.UiFont) { Title = "GAME PAUSED", DimBackground = true };
    }

    public void OnOpen()
    {
        BuildMenu();
    }

    public void OnClose() { }

    private void BuildMenu()
    {
        menu.ClearButtons();

        float x = (Consts.DefaultScreenSize.w - buttonSize.X) / 2f;
        float y = Consts.DefaultScreenSize.h * 0.4f;

        menu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Resume",
            game.TogglePause,
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 0)
        ));
        menu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Settings",
            () => overlayManager.Push(new SettingsOverlay(game, overlayManager)),
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 1)
        ));
        menu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Main Menu",
            ReturnToMainMenu,
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 2)
        ));
        menu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Quit Game",
            game.Exit,
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 3)
        ));
    }

    private void ReturnToMainMenu()
    {
        if (game.ScreenManager.Current is IPausableScreen pausable && pausable.IsPaused) pausable.TogglePause();

        overlayManager.Clear();
        game.ScreenManager.SetScreen(new ScreenMainMenu(game, game.ScreenManager));
    }

    public void Resize((int w, int h) size)
    {
        BuildMenu();
    }

    public void Update(GameTime gameTime)
    {
        menu.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        menu.Draw(spriteBatch, game.VirtualScreenSize);
    }

}