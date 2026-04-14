using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sprint2.Entities.Players;
using Sprint2.Extensions;
using Sprint2.Levels;
using Sprint2.Managers;
using Sprint2.UI;
using Sprint2.Util;

namespace Sprint2.Screens;

public class ScreenStory : IScreen, IResettableScreen, IPausableScreen, IPlayerProvider//, IResizableScreen
{
    private enum StoryState
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

    private StoryState state;
    private LevelManager levelManager;
    private Player player;
    private bool isPaused;
    private (int, int) currentLevelCoords;

    public bool IsPaused => isPaused;
    public IPlayer CurrentPlayer => state == StoryState.Playing ? player : null;
    
    // BEGIN DEBUG
    private KeyboardState prevKeyboard;
    private float levelSwapFadeAlpha;
    private bool levelSwapFading;
    private const float LevelSwapSpeed = 1.25f;
    private (int x, int y) pendingLevelCoords;
    // END DEBUG

    public ScreenStory(Game1 game, ScreenManager screenManager)
    {
        this.game = game;
        this.screenManager = screenManager;
        
        // BEGIN DEBUG
        prevKeyboard = Keyboard.GetState();
        // END DEBUG

        menu = new(Assets.UiFont) { Title = "Story Mode", DimBackground = true };
        gameOverMenu = new(Assets.UiFont) { Title = "Game Over", DimBackground = true };
        winMenu = new(Assets.UiFont) { Title = "You Won!\nWhat would you like\nto do next?", DimBackground = true };
    }

    public void OnEnter()
    {
        state = StoryState.Menu;

        player = new Player();
        levelManager = new LevelManager();

        currentLevelCoords = (0, 0);

        BuildMenu();
        BuildGameOverMenu();
        BuildWinMenu();
    }

    public void OnExit() { }

    public void BuildMenu()
    {
        menu.ClearButtons();

        float x = (Consts.DefaultScreenSize.w - buttonSize.X) / 2f;
        float y = Consts.DefaultScreenSize.h * 0.4f;

        menu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "New Game",
            StartNewGame,
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 0)
        ));
        menu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Load Game\n(WIP)",
            () => { },
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 1)
        ));
        menu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Back",
            () => screenManager.SetScreen(new ScreenMainMenu(game, screenManager)),
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 2)
        ));
    }

    public void BuildGameOverMenu()
    {
        gameOverMenu.ClearButtons();

        float x = (game.GraphicsDevice.Viewport.Width - buttonSize.X) / 2;
        float y = game.GraphicsDevice.Viewport.Height * 0.4f;

        gameOverMenu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Retry Level",
            () => StartStoryLevel(currentLevelCoords),
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
            () => StartStoryLevel(currentLevelCoords),
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

    private void StartNewGame()
    {
        StoryLevelRegistry.LoadLevelData();
        currentLevelCoords = (0,0);
        StartStoryLevel(currentLevelCoords);
    }

    private void StartStoryLevel((int, int) coords)
    {
        state = StoryState.Playing;
        currentLevelCoords = coords;
        levelManager.StartStory(player, coords);
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
            case StoryState.Menu:
                menu.Update();
                break;
            case StoryState.Playing:
                if (player.LevelChangeCoords != (0, 0)) {
                    var nextLevelCoords = (currentLevelCoords.Item1+player.LevelChangeCoords.Item1, currentLevelCoords.Item2+player.LevelChangeCoords.Item2);
                    var nextDef = StoryLevelRegistry.Get(nextLevelCoords);
                    if (nextDef is StoryLevelDef next) {
                        levelManager.StartStory(player, nextLevelCoords);
                        currentLevelCoords = nextLevelCoords;
                    } else {
                        state = StoryState.GameOver;
                        gameOverMenu.Update();
                    }
                }
                levelManager.Update(gameTime);
                if (levelManager.IsGameOver) {
                    state = StoryState.GameOver;
                    gameOverMenu.Update();}
                break;
            case StoryState.GameOver:
                gameOverMenu.Update();
                break;
            case StoryState.Won:
                winMenu.Update();
                break;
        }

        // BEGIN DEBUG: Keybound Level switching logic
        var keyboard = Keyboard.GetState();
        if (keyboard.IsKeyDown(Keys.OemPeriod) && prevKeyboard.IsKeyUp(Keys.OemPeriod)) 
        {
            if (!levelSwapFading)
            {
                pendingLevelCoords = StoryLevelRegistry.LevelCoords[(StoryLevelRegistry.LevelCoords.IndexOf(currentLevelCoords) + 1) % StoryLevelRegistry.LevelCoords.Count];

                levelSwapFading = true;
                levelSwapFadeAlpha = 0f;
            }
        }

        if (levelSwapFading)
        {
            levelSwapFadeAlpha += LevelSwapSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (levelSwapFadeAlpha >= 1f)
            {
                levelSwapFadeAlpha = 0f;
                levelSwapFading = false;

                currentLevelCoords = pendingLevelCoords;
                StartStoryLevel(currentLevelCoords);
            }
        }

        prevKeyboard = keyboard;
        // END DEBUG
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        switch (state)
        {
            case StoryState.Playing:
                levelManager.Draw(spriteBatch);
                break;
            case StoryState.Menu:
                menu.Draw(spriteBatch, game.VirtualScreenSize);
                break;
            case StoryState.GameOver:
                gameOverMenu.Draw(spriteBatch, game.VirtualScreenSize);
                break;
            case StoryState.Won:
                winMenu.Update();
                break;
        }

        // BEGIN DEBUG
        if (levelSwapFading && levelSwapFadeAlpha > 0f)
        {
            var viewport = game.VirtualScreenSize;
            var fullscreenRect = new Rectangle(0, 0, (int)viewport.X, (int)viewport.Y);

            spriteBatch.Draw(Assets.PixelTexture, fullscreenRect, Color.White * levelSwapFadeAlpha);
        }
        // END DEBUG

    }

}