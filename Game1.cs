﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Sprint2.Controllers;
using Sprint2.UI;
using Interfaces;
using System.Linq;
using System.Security;


namespace Sprint2;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;

    private IController keyboardController;
    private MouseController mouseController;

    private IPlayer player;
    public IPlayer Player => player;

    private PauseMenu pauseMenu;
    private SpriteFont uiFont;

    private bool isPaused;
    public bool IsPaused => isPaused;

    private List<Rectangle> objects;
    private List<Platform> platforms;

    private Plant testPlant;
    private Texture2D arrowTexture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
        spriteBatch = new SpriteBatch(GraphicsDevice);

        keyboardController = new KeyBoardController();
        mouseController = new MouseController();

        ResetLevel();

        CommandUtil.AttachCommandBindings(this);
    }

    protected override void LoadContent()
    {
        //Content.RootDirectory = @"E:\vsp\CSE3902Sprint2\Content\bin\DesktopGL"; /* Benny: it is for my desktop use, delete it if bug */
        LinkUtil.texture = Content.Load<Texture2D>("Link");
        PlantUtil.spritesheet = Content.Load<Texture2D>("testsheet");
        PlatformUtil.spritesheet = Content.Load<Texture2D>("testsheet");
        ButtonUtil.buttonTexture = Content.Load<Texture2D>("DefaultButton");
        ButtonUtil.resetTexture = Content.Load<Texture2D>("ResetButton");
        uiFont = Content.Load<SpriteFont>("UIFont");
        pauseMenu = new PauseMenu(uiFont, GraphicsDevice);
        arrowTexture = Content.Load<Texture2D>("AimerArrow");

        Vector2 resumePosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - 100, GraphicsDevice.Viewport.Height / 2 - 60);
        Vector2 quitPosition = new Vector2(resumePosition.X, resumePosition.Y + 60);
        Vector2 resetPosition = new Vector2(16, 16);

        pauseMenu.AddButton(new Button(uiFont, ButtonUtil.buttonTexture, "Resume", () => TogglePause(), new Vector2(200, 50), resumePosition));
        pauseMenu.AddButton(new Button(uiFont, ButtonUtil.buttonTexture, "Quit", () => Exit(), new Vector2(200, 50), quitPosition));
        pauseMenu.AddButton(new Button(uiFont, ButtonUtil.resetTexture, "", () => ResetLevel(), new Vector2(32, 32), resetPosition));
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
    }

    private void ResetLevel()
    {
        player = new Player();
        player.InitializeAimer(arrowTexture);
        testPlant = new(Plant.Species.grass, (20, 20));
        objects = new List<Rectangle>();
        platforms = new();
        platforms.Add(new Platform(Platform.Type.stonebrick, 0, 16*25, 40, 1));
        if (isPaused) TogglePause();
    }

    public void RestartLevel()
    {
        ResetLevel();
    }

    protected override void Update(GameTime gameTime)
    {
        mouseController.Update();
        keyboardController.IsPaused = isPaused;
        keyboardController.Update();

        if (!isPaused)
        {
            objects.Clear();

            //This is all for testing/display
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                testPlant.Update(gameTime);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                testPlant.ToggleSpecies();
            }

            objects.AddRange(testPlant.GetPlantObjects());
            objects.AddRange(platforms.Select(p => p.Bounds));

            player.Update(gameTime, objects);

            if (player.Position.Y > GraphicsDevice.Viewport.Height) ResetLevel();
        } else
        {
            pauseMenu.Update(mouseController.Current, mouseController.Previous);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();

        player.Draw(spriteBatch);
        testPlant.Draw(spriteBatch);
        foreach (var p in platforms) p.Draw(spriteBatch);

        if (isPaused)
        {
            pauseMenu.Draw(spriteBatch, GraphicsDevice);
        }

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
