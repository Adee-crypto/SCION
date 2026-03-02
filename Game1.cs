﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Controllers;
using Sprint2.Entities;
using Sprint2.Entities.Enemies;
using Sprint2.Entities.Plants;
using Sprint2.Entities.Players;
using Sprint2.Managers;
using Sprint2.UI;
using Sprint2.Util;
using Sprint2.Screens;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


namespace Sprint2;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;
    private (int w, int h) screenSize;

    public bool IsPaused {get; private set;}

    private Menu pauseMenu;

    private Level level;
    public Player Player {get; private set;}

    public Game1()
    {
        _graphics = new(this);
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

        //specific objects (prob will all be deleted and added to level, maybe not player tho)
        Player = new();
        level = new(Player);
        
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
        Assets.PauseMenuTexture = new Texture2D(GraphicsDevice, 1, 1);
        Assets.PauseMenuTexture.SetData([Color.White]);

        //screenManager.SetScreen(new ScreenMainMenu(this, screenManager, mouseController)); // Not yet finished
        pauseMenu = new Menu(Assets.UiFont, GraphicsDevice) { Title = "Game Paused", DimBackground = true };

        Vector2 resumePosition = new(screenSize.w / 2 - 100, screenSize.h / 2 - 60);
        Vector2 quitPosition = new(resumePosition.X, resumePosition.Y + 60);
        Vector2 resetPosition = new(16, 16);

        pauseMenu.AddButton(new Button(Assets.UiFont, Assets.ButtonTexture, "Resume", TogglePause, new Vector2(200, 50), resumePosition));
        pauseMenu.AddButton(new Button(Assets.UiFont, Assets.ButtonTexture, "Quit", Exit, new Vector2(200, 50), quitPosition));
        pauseMenu.AddButton(new Button(Assets.UiFont, Assets.ResetTexture, "", ResetLevel, new Vector2(32, 32), resetPosition));
    }

    public void TogglePause() => IsPaused = !IsPaused;

    public void ResetLevel()
    {
        level.Reset();
        // IsPaused = false;
    }

    protected override void Update(GameTime gameTime)
    {
        //screenManager.Update(gameTime); // Not yet finished

        if (IsActive) { //prevents input from being processed when game is not active (e.g. alt-tabbed out)
            MouseController.IsPaused = IsPaused;
            MouseController.Update();
            KeyboardController.IsPaused = IsPaused;
            KeyboardController.Update();
        }
        screenSize = (GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        if (IsPaused)
        {
            pauseMenu.Update();
        }
        else
        {
            level.Update(gameTime, screenSize);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        level.Draw(spriteBatch);
        if (IsPaused) pauseMenu.Draw(spriteBatch, screenSize);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
