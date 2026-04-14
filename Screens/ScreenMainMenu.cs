using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.UI;
using Sprint2.UI.Overlays;
using Sprint2.Util;

namespace Sprint2.Screens;

<<<<<<< HEAD
public class ScreenMainMenu : IScreen, IResizable
=======
public class ScreenMainMenu : IScreen//, IResizableScreen
>>>>>>> 59edc3d (resizing logic simplified, fully works for ingame objects, some UI still needs fixing depending on what we want to do with it)
{
    private readonly Game1 game;
    private readonly ScreenManager screenManager;
    private readonly Menu menu;

    public ScreenMainMenu(Game1 game, ScreenManager screenManager)
    {
        this.game = game;
        this.screenManager = screenManager;

        menu = new(Assets.UiFont) { Title = "Main Menu", DimBackground = false };
    }

    public void OnEnter()
    {
        BuildMenu();
    }

    public void OnExit() { }

    private void BuildMenu() {
        menu.ClearButtons();

        Vector2 buttonSize = new(200, 50);
        float spacer = 18f;

        float x = (Consts.DefaultScreenSize.w - buttonSize.X) / 2f;
        float y = Consts.DefaultScreenSize.h * 0.3f;

        menu.AddButton(new(
            Assets.UiFont, 
            Assets.ButtonTexture, 
            "Story Mode", 
            () => screenManager.SetScreen(new ScreenStory(game, screenManager)), 
            buttonSize, 
            new Vector2(x, y + (buttonSize.Y + spacer) * 0)
        ));
        menu.AddButton(new(
            Assets.UiFont, 
            Assets.ButtonTexture, 
            "Arcade Mode", 
            () => screenManager.SetScreen(new ScreenArcade(game, screenManager)), 
            buttonSize, 
            new Vector2(x, y + (buttonSize.Y + spacer) * 1)
        ));
        menu.AddButton(new(
            Assets.UiFont, 
            Assets.ButtonTexture, 
            "Settings", 
            () => game.OverlayManager.Push(new SettingsOverlay(game, game.OverlayManager)), 
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

<<<<<<< HEAD
    public void Resize((int w, int h) size) => OnEnter();
=======
    public void OnExit() { }
>>>>>>> 59edc3d (resizing logic simplified, fully works for ingame objects, some UI still needs fixing depending on what we want to do with it)

    public void Update(GameTime gameTime)
    {
        menu.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        menu.Draw(spriteBatch, game.VirtualScreenSize);
    }

}