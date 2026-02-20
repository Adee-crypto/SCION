﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Sprint2.Controllers;
using Sprint2.UI;
using Interfaces;
using System.Linq;


namespace Sprint2;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;

    private IController keyboardController;
    private IMouseController mouseController;
    private (int w, int h) screenSize;

    private bool isPaused;
    public bool IsPaused => isPaused;

    private PauseMenu pauseMenu;

    public Player player0;
    private List<Rectangle> objects;
    private List<Platform> platforms;
    private ProjectileDef def;
    private ProjectileManager projectileManager;

    //for testing
    public Plant testPlant;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        screenSize = ScreenUtil.defaultScreenSize;
        _graphics.PreferredBackBufferWidth = screenSize.w;
        _graphics.PreferredBackBufferHeight = screenSize.h;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        keyboardController = new KeyBoardController();
        mouseController = new MouseController();
        CommandUtil.AttachCommandBindings(this);

        player0 = new Player();

        base.Initialize();
        ResetLevel();
    }

    protected override void LoadContent()
    {
        //Content.RootDirectory = @"E:\vsp\CSE3902Sprint2\Content\bin\DesktopGL"; /* Benny: it is for my desktop use, delete it if bug */
        PlayerUtil.playerTexture = Content.Load<Texture2D>("Link");
        PlayerUtil.arrowTexture = Content.Load<Texture2D>("AimerArrow");
        PlantUtil.spritesheet = Content.Load<Texture2D>("testsheet");
        PlatformUtil.spritesheet = Content.Load<Texture2D>("testsheet");
        UIUtil.buttonTexture = Content.Load<Texture2D>("DefaultButton");
        UIUtil.resetTexture = Content.Load<Texture2D>("ResetButton");
        UIUtil.uiFont = Content.Load<SpriteFont>("UIFont");
        pauseMenu = new PauseMenu(UIUtil.uiFont, GraphicsDevice);

        Vector2 resumePosition = new(screenSize.w / 2 - 100, screenSize.h / 2 - 60);
        Vector2 quitPosition = new(resumePosition.X, resumePosition.Y + 60);
        Vector2 resetPosition = new(16, 16);

        pauseMenu.AddButton(new Button(UIUtil.uiFont, UIUtil.buttonTexture, "Resume", () => TogglePause(), new Vector2(200, 50), resumePosition));
        pauseMenu.AddButton(new Button(UIUtil.uiFont, UIUtil.buttonTexture, "Quit", () => Exit(), new Vector2(200, 50), quitPosition));
        pauseMenu.AddButton(new Button(UIUtil.uiFont, UIUtil.resetTexture, "", () => ResetLevel(), new Vector2(32, 32), resetPosition));

        def = new ProjectileDef("Nuke", Content.Load<Texture2D>("NukeProjectile"), new Vector2(12, 29), 5f, 200f, 98f);
        projectileManager = new ProjectileManager(mouseController, player0, def);
    }

    public void TogglePause() => isPaused = !isPaused;

    public void ResetLevel()
    {
        player0.Reset();
        projectileManager.Reset();
        testPlant = new(Plant.Species.grass, (20, 20)); //POTENTIALLY ADD RESET TO PLANT
        objects = [];
        platforms = [new(Platform.Type.stonebrick, 0, 16*25, 40, 1)];
        isPaused = false;
    }

    protected override void Update(GameTime gameTime)
    {
        mouseController.Update();
        keyboardController.IsPaused = isPaused;
        keyboardController.Update();
        screenSize = (GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        if (isPaused) {
            pauseMenu.Update(mouseController);
        } else {
            objects.Clear();

            objects.AddRange(testPlant.GetPlantObjects());
            objects.AddRange(platforms.Select(p => p.Bounds));

            player0.Update(gameTime, objects);
            projectileManager.Update(gameTime, objects);

            if (player0.IsBreakable)
            {
                testPlant.RemoveCellBelow(new Vector2(player0.Hitbox.Center.X, player0.Hitbox.Bottom));
                player0.IsBreakable = false;
            }

            testPlant.Update(gameTime);

            if (player0.Position.Y > screenSize.h) ResetLevel();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();

        player0.Draw(spriteBatch);
        testPlant.Draw(spriteBatch);
        projectileManager.Draw(spriteBatch);
        platforms.ForEach(p => p.Draw(spriteBatch));

        if (isPaused) pauseMenu.Draw(spriteBatch, screenSize);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
