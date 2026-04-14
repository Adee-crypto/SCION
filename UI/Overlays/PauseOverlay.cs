using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Screens;
using Sprint2.Util;

namespace Sprint2.UI.Overlays;

<<<<<<< HEAD:UI/Overlays/PauseOverlay.cs
public class PauseOverlay : IOverlay, IResizable
=======
public class PauseOverlay// : IResizableScreen
>>>>>>> 59edc3d (resizing logic simplified, fully works for ingame objects, some UI still needs fixing depending on what we want to do with it):UI/PauseOverlay.cs
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

<<<<<<< HEAD:UI/Overlays/PauseOverlay.cs
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

=======
>>>>>>> 59edc3d (resizing logic simplified, fully works for ingame objects, some UI still needs fixing depending on what we want to do with it):UI/PauseOverlay.cs
    public void Update(GameTime gameTime)
    {
        menu.Update();
    }

<<<<<<< HEAD:UI/Overlays/PauseOverlay.cs
    public void Draw(SpriteBatch spriteBatch)
=======
    public void Draw(SpriteBatch spriteBatch, Vector2 size)
>>>>>>> 59edc3d (resizing logic simplified, fully works for ingame objects, some UI still needs fixing depending on what we want to do with it):UI/PauseOverlay.cs
    {
        menu.Draw(spriteBatch, (game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height));
    }

}