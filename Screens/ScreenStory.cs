using Sprint2.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.UI;
using Sprint2.Util;
using Sprint2.Managers;
using Sprint2.Entities.Players;

namespace Sprint2.Screens;

public class ScreenStory : IScreen, IResizableScreen, IResettableScreen, IPlayerProvider
{
    private enum StoryState
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
    private StoryState state;
    private LevelManager levelManager;
    private Player player;
    private int currentLevelIndex;
    public IPlayer CurrentPlayer => state == StoryState.Playing ? player : null;

    public ScreenStory(Game1 game, ScreenManager screenManager)
    {
        this.game = game;
        this.screenManager = screenManager;

        menu = new(Assets.UiFont) { Title = "Story Mode", DimBackground = true };
        gameOverMenu = new(Assets.UiFont) { Title = "Game Over", DimBackground = true };
    }

    public void OnEnter()
    {
        state = StoryState.Menu;

        player = new Player();
        levelManager = new LevelManager();

        currentLevelIndex = 0;

        BuildMenu();
        BuildGameOverMenu();
    }

    public void OnExit() {}

    public void BuildMenu()
    {
        menu.ClearButtons();

        float x = (game.GraphicsDevice.Viewport.Width - buttonSize.X) / 2;
        float y = game.GraphicsDevice.Viewport.Height * 0.4f;

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
            () => {}, 
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
            () => StartStoryLevel(currentLevelIndex),
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 0)
        ));
        gameOverMenu.AddButton(new(
            Assets.UiFont,
            Assets.ButtonTexture,
            "Back",
            () =>
            {
                state = StoryState.Menu;
                BuildMenu();
            },
            buttonSize,
            new Vector2(x, y + (buttonSize.Y + spacer) * 1)
        ));
    }

    private void StartNewGame()
    {
        currentLevelIndex = 0;
        StartStoryLevel(currentLevelIndex);
    }

    private void StartStoryLevel(int index)
    {
        levelManager.StartStory(player, index);
        levelManager.Resize((game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height));
        state = StoryState.Playing;
    }

    public void Resize((int w, int h) size)
    {
        levelManager?.Resize(size);
    }

    public void Reset()
    {
        levelManager?.Reset();
    }

    public void Update(GameTime gameTime)
    {
        switch (state)
        {
            case StoryState.Menu:
                menu.Update();
                break;
            case StoryState.Playing:
                levelManager.Update(gameTime);
                if (levelManager.IsGameOver) state = StoryState.GameOver;
                break;
            case StoryState.GameOver:
                gameOverMenu.Update();
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (state == StoryState.Playing)
        {
            levelManager.Draw(spriteBatch);
            return;
        }

        var size = (game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

        if (state == StoryState.Menu) menu.Draw(spriteBatch, size);
        else gameOverMenu.Draw(spriteBatch, size);
    }

}