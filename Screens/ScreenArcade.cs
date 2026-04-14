using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Sprint2.Entities.Players;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.UI;
using Sprint2.Util;

namespace Sprint2.Screens;

public class ScreenArcade : IScreen, IResettableScreen, IPausableScreen, IPlayerProvider
{
    private enum ArcadeState
    {
        Menu,
        Playing,
        GameOver,
        Won
    }
    private readonly Game1 game;
    private readonly ScreenManager screenManager;
    private readonly Menu menu;
    private readonly Menu gameOverMenu;
    private readonly Menu winMenu;
    private readonly Vector2 buttonSize = new(300, 75);
    private readonly float spacer = 18f;
    private ArcadeState state;
    private LevelManager levelManager;
    private bool isPaused;
    public bool IsPaused => isPaused;
    private Player player;
    public IPlayer CurrentPlayer => state == ArcadeState.Playing ? player : null;
    public ScreenArcade(Game1 game, ScreenManager screenManager)
    {
        this.game = game;
        this.screenManager = screenManager;

        menu = new(Assets.UiFont) { Title = "Arcade Mode", DimBackground = true };
        gameOverMenu = new(Assets.UiFont) { Title = "Game Over", DimBackground = true };
        winMenu = new(Assets.UiFont) { Title = "You Won!\nWhat would you like\nto do next?", DimBackground = true };
    }

    public void OnEnter()
    {
        state = ArcadeState.Menu;

        player = new Player();
        levelManager = new LevelManager();

        BuildMenu();
        BuildGameOverMenu();
        BuildWinMenu();
    }

    public void OnExit() { }

    private void BuildMenu()
    {
        menu.ClearButtons();

        float x = (Consts.DefaultScreenSize.w - buttonSize.X) / 2f;
        float y = Consts.DefaultScreenSize.h * 0.4f;

        menu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Start",
            StartArcadeRun,
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 0)
        ));
        menu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Back",
            () => screenManager.SetScreen(new ScreenMainMenu(game, screenManager)),
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 1)
        ));
    }

    private void BuildGameOverMenu()
    {
        gameOverMenu.ClearButtons();

        float x = (game.GraphicsDevice.Viewport.Width - buttonSize.X) / 2;
        float y = game.GraphicsDevice.Viewport.Height * 0.4f;

        gameOverMenu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Play Again",
            StartArcadeRun,
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 0)
        ));
        gameOverMenu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Main Menu",
            () => screenManager.SetScreen(new ScreenMainMenu(game, screenManager)),
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 1)
        ));
        gameOverMenu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Quit Game",
            game.Exit,
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 2)
        ));
    }

    private void BuildWinMenu()
    {
        winMenu.ClearButtons();

        float x = (game.GraphicsDevice.Viewport.Width - buttonSize.X) / 2;
        float y = game.GraphicsDevice.Viewport.Height * 0.4f;

        winMenu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Play Again",
            StartArcadeRun,
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 0)
        ));
        winMenu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Main Menu",
            () => screenManager.SetScreen(new ScreenMainMenu(game, screenManager)),
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 1)
        ));
        winMenu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Quit Game",
            game.Exit,
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 2)
        ));
    }

    private void StartArcadeRun()
    {
        levelManager.StartArcade(player);
        state = ArcadeState.Playing;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused) MediaPlayer.Pause();
        else MediaPlayer.Resume();
    }

    public void Reset()
    {
        levelManager?.Reset();
    }

    public void Update(GameTime gameTime)
    {
        if (isPaused) return;

        switch (state)
        {
            case ArcadeState.Menu:
                menu.Update();
                break;
            case ArcadeState.Playing:
                levelManager.Update(gameTime);
                if (levelManager.IsGameOver) state = ArcadeState.GameOver;
                break;
            case ArcadeState.GameOver:
                gameOverMenu.Update();
                break;
            case ArcadeState.Won:
                winMenu.Update();
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        switch (state)
        {
            case ArcadeState.Menu:
                menu.Draw(spriteBatch, game.VirtualScreenSize);
                break;
            case ArcadeState.Playing:
                levelManager.Draw(spriteBatch);
                break;
            case ArcadeState.GameOver:
                gameOverMenu.Draw(spriteBatch, game.VirtualScreenSize);
                break;
            case ArcadeState.Won:
                winMenu.Draw(spriteBatch, game.VirtualScreenSize);
                break;
        }
    }

}