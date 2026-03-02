using Sprint2.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.UI;
using Sprint2.Util;

namespace Sprint2.Screens;

public class ScreenArcade : IScreen
{
    private readonly Game1 game;
    private readonly ScreenManager screenManager;
    private readonly IMouseController mouseController;
    private readonly Menu menu;
    public ScreenArcade(Game1 game, ScreenManager screenManager, IMouseController mouseController)
    {
        this.game = game;
        this.screenManager = screenManager;
        this.mouseController = mouseController;

        menu = new Menu(Assets.UiFont, game.GraphicsDevice) { Title = "Arcade Mode", DimBackground = true };
    }

    public void OnEnter()
    {
        menu.ClearButtons();

        Vector2 buttonSize = new(300, 75);
        float spacer = 18f;

        float x = (game.GraphicsDevice.Viewport.Width - buttonSize.X) / 2;
        float y = game.GraphicsDevice.Viewport.Height * 0.4f;

        menu.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Start\n(WIP)", () => {}, buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 0)));
        menu.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Back", () => screenManager.SetScreen(new ScreenMainMenu(game, screenManager, mouseController)), buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 1)));
    }

    public void OnExit() {}

    public void Update(GameTime gameTime)
    {
        menu.Update(mouseController);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        menu.Draw(spriteBatch, (game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height));
    }

}