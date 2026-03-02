﻿using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Controllers;
using Sprint2.Entities;
using Sprint2.Entities.Enemies;
using Sprint2.Entities.Plants;
using Sprint2.Entities.Players;
using Sprint2.UI;
using Sprint2.Util;
using System.Collections.Generic;
using System.Linq;


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

    public Player Player { get; private set; }
    private List<Rectangle> objects;
    private List<Platform> platforms;
    private ProjectileManager projectileManager;
    private EnemyDef rangedEnemy;
    private EnemyManager enemyManager;

    //for testing
    public Plant testPlant { get; private set; }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        screenSize = Consts.DefaultScreenSize;
        _graphics.PreferredBackBufferWidth = screenSize.w;
        _graphics.PreferredBackBufferHeight = screenSize.h;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        spriteBatch = new(GraphicsDevice);

        //controllers
        keyboardController = new();
        mouseController = new();


        //specific objects (prob will all be deleted and added to level, maybe not player tho)
        Player = new();
        
        //managers
        projectileManager = new(mouseController, Player);
        enemyManager = new();
        
        base.Initialize();
        ResetLevel();

        //Requires all (most) fields of Game1 to be initialized first
        KeyBindings.AttachKeyBindings(this);
    }

    protected override void LoadContent()
    {
        //Content.RootDirectory = @"E:\vsp\CSE3902Sprint2\Content\bin\DesktopGL"; /* Benny: it is for my desktop use, delete it if bug */
        Assets.PlayerTexture = Content.Load<Texture2D>("Link");
        Assets.ArrowTexture = Content.Load<Texture2D>("AimerArrow2");
        Assets.PlantSpritesheet = Content.Load<Texture2D>("testsheet");
        Assets.PlatformSpritesheet = Content.Load<Texture2D>("testsheet");
        Assets.ButtonTexture = Content.Load<Texture2D>("DefaultButton");
        Assets.ResetTexture = Content.Load<Texture2D>("ResetButton");
        Assets.UiFont = Content.Load<SpriteFont>("UIFont");
        Assets.VoidspawnTexture = Content.Load<Texture2D>("VoidSpawns");
        pauseMenu = new PauseMenu(Assets.UiFont, GraphicsDevice);

        Vector2 resumePosition = new(screenSize.w / 2 - 100, screenSize.h / 2 - 60);
        Vector2 quitPosition = new(resumePosition.X, resumePosition.Y + 60);
        Vector2 resetPosition = new(16, 16);

        pauseMenu.AddButton(new Button(Assets.UiFont, Assets.ButtonTexture, "Resume", TogglePause, new Vector2(200, 50), resumePosition));
        pauseMenu.AddButton(new Button(Assets.UiFont, Assets.ButtonTexture, "Quit", Exit, new Vector2(200, 50), quitPosition));
        pauseMenu.AddButton(new Button(Assets.UiFont, Assets.ResetTexture, "", ResetLevel, new Vector2(32, 32), resetPosition));

        rangedEnemy = new EnemyDef("Void Spawn", Assets.VoidspawnTexture, 100f, 98f, 96f, 128f, 96f);
    }

    public void TogglePause() => isPaused = !isPaused;

    public void ResetLevel()
    {
        Player.Reset();
        enemyManager.Reset();
        projectileManager.Reset();
        enemyManager.Spawn(rangedEnemy, new Vector2(16 * 40, 16 * 24));
        testPlant = new ApplePlant((20, 20)); //POTENTIALLY ADD RESET TO PLANT
        objects = [];
        platforms = [new(Platform.Type.stonebrick, 0, 16 * 25, 50, 1)];
        isPaused = false;
    }

    protected override void Update(GameTime gameTime)
    {
        if (IsActive) { //prevents input from being processed when game is not active (e.g. alt-tabbed out)
            mouseController.IsPaused = isPaused;
            mouseController.Update();
            keyboardController.IsPaused = isPaused;
            keyboardController.Update();
        }
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
            Player.Update(gameTime, objects);
            enemyManager.Update(gameTime, objects, Player, projectileManager);

            if (Player.IsBreakable)
            {
                if (testPlant.TryRemoveCellBelow(new Vector2(Player.Collider.Hitbox.Center.X, Player.Collider.Hitbox.Bottom)))
                {
                    Player.GetSeed();
                }
                Player.IsBreakable = false;
            }

            testPlant.Update(gameTime);

            if (Player.Collider.Position.Y > screenSize.h) ResetLevel();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        testPlant.Draw(spriteBatch);
        projectileManager.Draw(spriteBatch);
        enemyManager.Draw(spriteBatch);
        platforms.ForEach(p => p.Draw(spriteBatch));
        Player.Draw(spriteBatch);

        if (isPaused) pauseMenu.Draw(spriteBatch, screenSize);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
