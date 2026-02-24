﻿using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Controllers;
using Sprint2.Entities;
using Sprint2.Entities.Enemies;
using Sprint2.Entities.Plants;
using Sprint2.UI;
using Sprint2.Util;
using System.Collections.Generic;
using System.Linq;
// using Sprint2.Entities.Plants;


namespace Sprint2;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;

    private KeyBoardController keyboardController;
    private MouseController mouseController;
    private (int w, int h) screenSize;

    private bool isPaused;
    public bool IsPaused => isPaused;

    private PauseMenu pauseMenu;

    public Player player0 { get; private set; }
    private List<Rectangle> objects;
    private List<Platform> platforms;
    private ProjectileManager projectileManager;
    private EnemyDef rangedEnemy;
    private EnemyManager enemyManager;

    //for testing
    public Plant testPlant;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        screenSize = Consts.defaultScreenSize;
        _graphics.PreferredBackBufferWidth = screenSize.w;
        _graphics.PreferredBackBufferHeight = screenSize.h;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        keyboardController = new KeyBoardController();
        mouseController = new MouseController();

        player0 = new Player();

        base.Initialize();
        ResetLevel();

        //Requires all (most) fields of Game1 to be initialized first
        KeyBindings.AttachKeyBindings(this);
    }

    protected override void LoadContent()
    {
        //Content.RootDirectory = @"E:\vsp\CSE3902Sprint2\Content\bin\DesktopGL"; /* Benny: it is for my desktop use, delete it if bug */
        Assets.playerTexture = Content.Load<Texture2D>("Link");
        Assets.arrowTexture = Content.Load<Texture2D>("AimerArrow");
        Assets.plantSpritesheet = Content.Load<Texture2D>("testsheet");
        Assets.platformSpritesheet = Content.Load<Texture2D>("testsheet");
        Assets.buttonTexture = Content.Load<Texture2D>("DefaultButton");
        Assets.resetTexture = Content.Load<Texture2D>("ResetButton");
        Assets.uiFont = Content.Load<SpriteFont>("UIFont");
        Assets.voidspawnTexture = Content.Load<Texture2D>("VoidSpawns");
        pauseMenu = new PauseMenu(Assets.uiFont, GraphicsDevice);

        Vector2 resumePosition = new(screenSize.w / 2 - 100, screenSize.h / 2 - 60);
        Vector2 quitPosition = new(resumePosition.X, resumePosition.Y + 60);
        Vector2 resetPosition = new(16, 16);

        pauseMenu.AddButton(new Button(Assets.uiFont, Assets.buttonTexture, "Resume", TogglePause, new Vector2(200, 50), resumePosition));
        pauseMenu.AddButton(new Button(Assets.uiFont, Assets.buttonTexture, "Quit", Exit, new Vector2(200, 50), quitPosition));
        pauseMenu.AddButton(new Button(Assets.uiFont, Assets.resetTexture, "", ResetLevel, new Vector2(32, 32), resetPosition));

        rangedEnemy = new EnemyDef("Void Spawn", Assets.voidspawnTexture, 100f, 98f, 96f, 128f, 96f);
        projectileManager = new ProjectileManager(mouseController, player0);
        enemyManager = new EnemyManager();
    }

    public void TogglePause() => isPaused = !isPaused;

    public void ResetLevel()
    {
        player0.Reset();
        enemyManager.Reset();
        projectileManager.Reset();
        enemyManager.Spawn(rangedEnemy, new Vector2(16 * 40, 16 * 24));
        testPlant = new(Species.grass, (20, 20)); //POTENTIALLY ADD RESET TO PLANT
        objects = [];
        platforms = [new(Platform.Type.stonebrick, 0, 16 * 25, 50, 1)];
        isPaused = false;
    }

    protected override void Update(GameTime gameTime)
    {
        mouseController.Update();
        keyboardController.IsPaused = isPaused;
        keyboardController.Update();
        screenSize = (GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        if (isPaused)
        {
            pauseMenu.Update(mouseController);
        }
        else
        {
            objects.Clear();

            objects.AddRange(testPlant.GetPlantObjects());
            objects.AddRange(platforms.Select(p => p.Bounds));

            projectileManager.Update(gameTime, objects); //MUST BE CALLED BEFORE PLAYER UPDATE TO GET VELOCITY
            player0.Update(gameTime, objects);
            enemyManager.Update(gameTime, objects, player0, projectileManager);

            if (player0.IsBreakable)
            {
                if (testPlant.RemoveCellBelow(new Vector2(player0.Hitbox.Center.X, player0.Hitbox.Bottom)))
                {
                    player0.GetSeed();
                }
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
        enemyManager.Draw(spriteBatch);
        platforms.ForEach(p => p.Draw(spriteBatch));

        if (isPaused) pauseMenu.Draw(spriteBatch, screenSize);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
