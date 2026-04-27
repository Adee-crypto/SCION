// using System.Numerics;
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
        Won,
        Freeze
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
    private (int x, int y) currentLevelCoords = (0, 0);
    private (int x, int y) pendingLevelCoords;
    private (int x, int y) pendingDeltaLevelCoords;

    public bool IsPaused => isPaused;
    public IPlayer CurrentPlayer => state == StoryState.Playing ? player : null;
    
    // BEGIN DEBUG
    private KeyboardState prevKeyboard;
    private float levelSwapFadeAlpha;
    private bool levelSwapFading;
    private const float LevelSwapSpeed = 1.25f;
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
        currentLevelCoords = (0,3);
        StartStoryLevel(currentLevelCoords);
    }


    private void StartStoryLevel((int, int) coords) => StartStoryLevel(coords, Vector2.Zero);

    private void StartStoryLevel((int, int) levelCoords, Vector2 playerCoords)
    {
        if (StoryLevelRegistry.Contains(levelCoords)) {
            state = StoryState.Playing;
            currentLevelCoords = levelCoords;
            levelManager.StartStory(levelCoords, player, playerCoords);
        } else {
            state = StoryState.GameOver;
            player.Kill();
        }
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

    public void ShiftLevel(int x, int y)
    {
        pendingLevelCoords = (currentLevelCoords.x + x, currentLevelCoords.y + y);
        pendingDeltaLevelCoords = (x, y);
        StartTransition();
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
                if (player.Collider.Position.X < 0) ShiftLevel(-1, 0);
                else if (player.Collider.Position.X > Consts.DefaultScreenSize.w) ShiftLevel(1, 0);
                else if (player.Collider.Position.Y > Consts.DefaultScreenSize.h) ShiftLevel(0, 1);
                else if (player.Collider.Position.Y < 0) ShiftLevel(0, -1);
                levelManager.Update(gameTime);
                if (levelManager.IsGameOver) {
                    state = StoryState.GameOver;
                    return;
                }
                break;
            case StoryState.GameOver:
                gameOverMenu.Update();
                break;
            case StoryState.Won:
                winMenu.Update();
                break;
            case StoryState.Freeze:
                levelManager.Frozen = true;
                break;
        }

        // BEGIN DEBUG: Keybound Level switching logic
        
        var keyboard = Keyboard.GetState();
        if (keyboard.IsKeyDown(Keys.J) && prevKeyboard.IsKeyUp(Keys.J)) {
            StartStoryLevel((currentLevelCoords.x-1, currentLevelCoords.y));
        } else if (keyboard.IsKeyDown(Keys.L) && prevKeyboard.IsKeyUp(Keys.L)) {
            StartStoryLevel((currentLevelCoords.x+1, currentLevelCoords.y));
        } else if (keyboard.IsKeyDown(Keys.I) && prevKeyboard.IsKeyUp(Keys.I)) {
            StartStoryLevel((currentLevelCoords.x, currentLevelCoords.y-1));
        } else if (keyboard.IsKeyDown(Keys.K) && prevKeyboard.IsKeyUp(Keys.K)) {
            StartStoryLevel((currentLevelCoords.x, currentLevelCoords.y+1));
        }

        if (levelSwapFading)
        {
            levelSwapFadeAlpha += LevelSwapSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (levelSwapFadeAlpha >= 1f)
            {
                levelSwapFadeAlpha = 0f;
                levelSwapFading = false;
                state = StoryState.Playing;
                levelManager.Frozen = false;
                StartStoryLevel(pendingLevelCoords, player.Collider.Position - new Vector2(pendingDeltaLevelCoords.x, pendingDeltaLevelCoords.y)*Consts.LevelSize);
            }
        }

        prevKeyboard = keyboard;
        // END DEBUG
    }

    public void StartTransition()
    {
        state = StoryState.Freeze;
        if (!levelSwapFading)
        {
            levelSwapFading = true;
            levelSwapFadeAlpha = 0f;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {

        // BEGIN DEBUG

        foreach (var delta in Consts.orthoDirs) {
            var AdjCoord = (delta.Item1+currentLevelCoords.x, delta.Item2+currentLevelCoords.y);
            if (StoryLevelRegistry.Contains(AdjCoord)) {
                if (delta.Item1 == 0) {
                    spriteBatch.Draw(Assets.PixelTexture, new Rectangle((int) (-2*Consts.LevelSize.X), (int) (Consts.LevelSize.Y*delta.Item2), (int) Consts.LevelSize.X*5, (int) Consts.LevelSize.Y), Color.LightPink);
                } else {
                    spriteBatch.Draw(Assets.PixelTexture, new Rectangle((int) (Consts.LevelSize.X*delta.Item1), (int) (-2*Consts.LevelSize.Y), (int) Consts.LevelSize.X, (int) Consts.LevelSize.Y*5), Color.LightPink);
                }
            }
        }
        foreach (var delta in Consts.orthoDirs) {
            var AdjCoord = (delta.Item1+currentLevelCoords.x,delta.Item2+currentLevelCoords.y);
            if (!StoryLevelRegistry.Contains(AdjCoord)) {
                if (delta.Item1 == 0) {
                    spriteBatch.Draw(Assets.PixelTexture, new Rectangle((int) (-2*Consts.LevelSize.X), (int) (Consts.LevelSize.Y*delta.Item2), (int) Consts.LevelSize.X*5, (int) Consts.LevelSize.Y), Color.Black);
                } else {
                    spriteBatch.Draw(Assets.PixelTexture, new Rectangle((int) (Consts.LevelSize.X*delta.Item1), (int) (-2*Consts.LevelSize.Y), (int) Consts.LevelSize.X, (int) Consts.LevelSize.Y*5), Color.Black);
                }
            }
        }

        if (levelSwapFading && levelSwapFadeAlpha > 0f)
        {
            var viewport = game.VirtualScreenSize;
            var fullscreenRect = new Rectangle((int) (-2*viewport.X), (int) (-2*viewport.Y), (int) (5*viewport.X),(int) (5*viewport.Y));

            spriteBatch.Draw(Assets.PixelTexture, fullscreenRect, Color.White * levelSwapFadeAlpha);
        }
        // END DEBUG
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
                winMenu.Draw(spriteBatch, game.VirtualScreenSize);
                break;
        }

    }

}