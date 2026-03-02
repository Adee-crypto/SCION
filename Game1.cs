﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Controllers;
using Sprint2.Entities.Players;
using Sprint2.UI;
using Sprint2.Util;


namespace Sprint2;

public class Game1 : Game
{
    //graphics
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;
    private (int w, int h) ScreenSize { get; set; } = Consts.DefaultScreenSize;
    
    //game state & data
    public bool IsPaused {get; private set;}
    private Menu pauseMenu;
    private Level level;
    public Player Player {get; private set;}

    public Game1()
    {
        //graphics & resizing
        _graphics = new(this);
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += new EventHandler<EventArgs>(ResizeGraphics);
        ResizeGraphics(null, null);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    private void ResizeGraphics(object sender, EventArgs e) {
        ScreenSize = (Window.ClientBounds.Width, Window.ClientBounds.Height);
        _graphics.PreferredBackBufferWidth = ScreenSize.w;
        _graphics.PreferredBackBufferHeight = ScreenSize.h;
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

        pauseMenu = new(Assets.UiFont, GraphicsDevice) { Title = "Game Paused", DimBackground = true };

        Vector2 resumePosition = new(ScreenSize.w / 2 - 100, ScreenSize.h / 2 - 60);
        Vector2 quitPosition = new(resumePosition.X, resumePosition.Y + 60);
        Vector2 resetPosition = new(16, 16);

        pauseMenu.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Resume", TogglePause, new(200, 50), resumePosition));
        pauseMenu.AddButton(new(Assets.UiFont, Assets.ButtonTexture, "Quit", Exit, new(200, 50), quitPosition));
        pauseMenu.AddButton(new(Assets.UiFont, Assets.ResetTexture, "", ResetLevel, new(32, 32), resetPosition));
    }

    public void TogglePause() => IsPaused = !IsPaused;

    public void ResetLevel()
    {
        level.Reset();
        // IsPaused = false; //idk if we want this or not
    }

    protected override void Update(GameTime gameTime)
    {
        ScreenSize = (GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        if (IsActive) { //prevents input from being processed when game is not active (e.g. alt-tabbed out)
            MouseController.Update();
            KeyboardController.Update(IsPaused);
        }

        if (IsPaused) {
            pauseMenu.Update();
        } else {
            level.Update(gameTime, ScreenSize);
        }

        base.Update(gameTime);
        //screenManager.Update(gameTime); // Not yet finished
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        level.Draw(spriteBatch);
        if (IsPaused) pauseMenu.Draw(spriteBatch, ScreenSize);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
