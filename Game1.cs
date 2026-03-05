﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Controllers;
using Sprint2.Entities.Players;
using Sprint2.Extensions;
using Sprint2.Screens;
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
    private readonly ScreenManager screenManager = new();
    public ScreenManager ScreenManager => screenManager;

    public Game1()
    {
        _graphics = new(this);
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += new EventHandler<EventArgs>(OnResize);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    private void OnResize(object sender, EventArgs e) {
        //resizing _graphics
        ScreenSize = (Window.ClientBounds.Width, Window.ClientBounds.Height);
        _graphics.PreferredBackBufferWidth = ScreenSize.w;
        _graphics.PreferredBackBufferHeight = ScreenSize.h;
        _graphics.ApplyChanges();

        //resizing anything else
        if (screenManager.Current is IResizableScreen resizable) resizable.Resize(ScreenSize);
    }

    protected override void Initialize()
    {
        spriteBatch = new(GraphicsDevice);

        KeyBindings.AttachKeyBindings(this); // Must be done after Game1's fields are initialized

        base.Initialize();
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

        Assets.PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        Assets.PixelTexture.SetData([Color.White]);

        OnResize(null, null);

        screenManager.SetScreen(new ScreenMainMenu(this, screenManager));
    }

    public void TogglePause()
    {
        if (screenManager.Current is IPausableScreen pausable) pausable.TogglePause();
    }

    public void ResetLevel()
    {
        if (screenManager.Current is IResettableScreen resettable) resettable.Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        if (IsActive) { //prevents input from being processed when game is not active (e.g. alt-tabbed out)
            MouseController.Update();
            bool isPaused = screenManager.Current is IPausableScreen pause && pause.IsPaused;
            KeyboardController.Update(isPaused);
        }

        screenManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        screenManager.Draw(spriteBatch);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
