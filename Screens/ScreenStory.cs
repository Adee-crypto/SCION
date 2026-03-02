using Sprint2.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.UI;
using Sprint2.Util;

namespace Sprint2.Screens;

public class ScreenStory : IScreen
{
    private readonly Game1 game;
    private readonly ScreenManager screenManager;
    private readonly Menu menu;
    public ScreenStory(Game1 game, ScreenManager screenManager)
    {
        this.game = game;
        this.screenManager = screenManager;

        menu = new(Assets.UiFont) { Title = "Story Mode", DimBackground = true };
    }

    public void OnEnter()
    {
        menu.ClearButtons();

        Vector2 buttonSize = new(300, 75);
        float spacer = 18f;

        float x = (game.GraphicsDevice.Viewport.Width - buttonSize.X) / 2;
        float y = game.GraphicsDevice.Viewport.Height * 0.4f;

        menu.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "New Game\n(WIP)", () => {}, buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 0)));
        menu.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Load Game\n(WIP)", () => {}, buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 1)));
        menu.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Back", () => screenManager.SetScreen(new ScreenMainMenu(game, screenManager)), buttonSize, new Vector2(x, y + (buttonSize.Y + spacer) * 2)));
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