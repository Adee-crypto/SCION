using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Players;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.UI;
using Sprint2.Util;

namespace Sprint2.Screens;

public class ScreenArcade : IScreen, IResizableScreen, IResettableScreen, IPlayerProvider, IPausableScreen
{
    private enum ArcadeState
    {
        Menu,
        Playing,
        GameOver
    }
    private readonly Game1 game;
    private readonly ScreenManager screenManager;
    private readonly Menu menu;
    private readonly Menu gameOverMenu;
    private readonly Vector2 buttonSize = new(300, 75);
    private readonly float spacer = 18f;
    private ArcadeState state;
    private LevelManager levelManager;
    private bool isPaused;
    public bool IsPaused => isPaused;
    private PauseOverlay pause;
    private Player player;
    public IPlayer CurrentPlayer => state == ArcadeState.Playing ? player : null;
    public ScreenArcade(Game1 game, ScreenManager screenManager)
    {
        this.game = game;
        this.screenManager = screenManager;

        menu = new(Assets.UiFont) { Title = "Arcade Mode", DimBackground = true };
        gameOverMenu = new(Assets.UiFont) { Title = "Game Over", DimBackground = true };
    }

    public void OnEnter()
    {
        state = ArcadeState.Menu;

        player = new Player();
        levelManager = new LevelManager();

        pause = new PauseOverlay(game);
        BuildMenu();
        BuildGameOverMenu();
    }

    public void OnExit() { }

    private void BuildMenu()
    {
        menu.ClearButtons();

        float x = (game.GraphicsDevice.Viewport.Width - buttonSize.X) / 2;
        float y = game.GraphicsDevice.Viewport.Height * 0.4f;

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

    private void StartArcadeRun()
    {
        levelManager.StartArcade(player);
        levelManager.Resize((game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height));
        state = ArcadeState.Playing;
    }

    public void Resize((int w, int h) size)
    {
        BuildMenu();
        BuildGameOverMenu();
        pause.Resize(size);
        levelManager?.Resize(size);
    }

    public void TogglePause() => isPaused = !isPaused;

    public void Reset()
    {
        levelManager?.Reset();
    }

    public void Update(GameTime gameTime)
    {
        if (isPaused)
        {
            pause.Update(gameTime);
            return;
        }

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
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var size = (game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

        if (state == ArcadeState.Playing) levelManager.Draw(spriteBatch);
        else if (state == ArcadeState.Menu) menu.Draw(spriteBatch, size);
        else gameOverMenu.Draw(spriteBatch, size);

        if (isPaused) pause.Draw(spriteBatch, size);
    }

}