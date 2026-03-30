using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.UI;
using Sprint2.Util;

namespace Sprint2.Screens;

public class PauseOverlay : IResizableScreen
{
    private readonly Game1 game;
    private readonly Menu menu;
    private readonly Vector2 buttonSize = new(300, 75);
    private readonly float spacer = 18f;
    public PauseOverlay(Game1 game)
    {
        this.game = game;

        menu = new(Assets.UiFont) { Title = "GAME PAUSED", DimBackground = true };
        BuildMenu();
    }

    private void BuildMenu()
    {
        menu.ClearButtons();

        float x = (game.GraphicsDevice.Viewport.Width - buttonSize.X) / 2;
        float y = game.GraphicsDevice.Viewport.Height * 0.4f;

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
            "Main Menu",
            () => game.ScreenManager.SetScreen(new ScreenMainMenu(game, game.ScreenManager)),
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 1)
        ));
        menu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Quit Game",
            game.Exit,
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 2)
        ));
    }

    public void Resize((int w, int h) size)
    {
        BuildMenu();
    }

    public void Update(GameTime gameTime)
    {
        menu.Update();
    }

    public void Draw(SpriteBatch spriteBatch, (int w, int h) size)
    {
        menu.Draw(spriteBatch, size);
    }

}