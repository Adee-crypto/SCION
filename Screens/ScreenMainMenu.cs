using Sprint2.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.UI;
using Sprint2.Util;

namespace Sprint2.Screens;

public class ScreenMainMenu : IScreen
{
    private readonly Game1 game;
    private readonly ScreenManager screenManager;
    private readonly Menu menu;
    public ScreenMainMenu(Game1 game, ScreenManager screenManager)
    {
        this.game = game;
        this.screenManager = screenManager;

        menu = new Menu(Assets.UiFont, game.GraphicsDevice) { Title = "Main Menu", DimBackground = false };
    }

    public void OnEnter()
    {
        menu.ClearButtons();

        Vector2 buttonSize = new(200, 50);
        float spacer = 18f;

        float x = (game.GraphicsDevice.Viewport.Width - buttonSize.X) / 2;
        float y = game.GraphicsDevice.Viewport.Height * 0.3f;

        menu.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Story Mode", () => screenManager.SetScreen(new ScreenStory(game, screenManager)), buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 0)));
        menu.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Arcade Mode", () => screenManager.SetScreen(new ScreenArcade(game, screenManager)), buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 1)));
        //menu.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Settings Mode", () => screenManager.SetScreen(new ScreenSettings(game, screenManager, mouseController)), buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 2))); // TO BE IMPLEMENTED
        menu.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Quit Game", () => game.Exit(), buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 3)));
    }

    public void OnExit() {}

    public void Update(GameTime gameTime)
    {
        menu.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        menu.Draw(spriteBatch, (game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height));
    }

}