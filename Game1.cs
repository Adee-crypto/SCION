﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    private (int w, int h) screenSize = ScreenUtil.defaultScreenSize;

    private bool isPaused;
    public bool IsPaused => isPaused;

    private PauseMenu pauseMenu;
    private SpriteFont uiFont;


    public Player Player0 {get;set;}
    private List<Rectangle> objects;
    private List<Platform> platforms;

    //for testing
    private Plant testPlant;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = screenSize.w;
        _graphics.PreferredBackBufferHeight = screenSize.h;
        _graphics.ApplyChanges();
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
        LinkUtil.linkTexture = Content.Load<Texture2D>("Link");
        LinkUtil.arrowTexture = Content.Load<Texture2D>("AimerArrow");
        PlantUtil.spritesheet = Content.Load<Texture2D>("testsheet");
        PlatformUtil.spritesheet = Content.Load<Texture2D>("testsheet");
        ButtonUtil.buttonTexture = Content.Load<Texture2D>("DefaultButton");
        ButtonUtil.resetTexture = Content.Load<Texture2D>("ResetButton");
        uiFont = Content.Load<SpriteFont>("UIFont");
        pauseMenu = new PauseMenu(uiFont, GraphicsDevice);

        Vector2 resumePosition = new(screenSize.w / 2 - 100, screenSize.h / 2 - 60);
        Vector2 quitPosition = new(resumePosition.X, resumePosition.Y + 60);
        Vector2 resetPosition = new(16, 16);

        pauseMenu.AddButton(new Button(uiFont, ButtonUtil.buttonTexture, "Resume", () => TogglePause(), new Vector2(200, 50), resumePosition));
        pauseMenu.AddButton(new Button(uiFont, ButtonUtil.buttonTexture, "Quit", () => Exit(), new Vector2(200, 50), quitPosition));
        pauseMenu.AddButton(new Button(uiFont, ButtonUtil.resetTexture, "", () => ResetLevel(), new Vector2(32, 32), resetPosition));
    }

    public void TogglePause() => isPaused = !isPaused;

    public void ResetLevel()
    {
        Player0 = new(); //ADD RESET METHOD TO PLAYER
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

            Player0.Update(gameTime, objects);

            if (Player0.Position.Y > screenSize.h) ResetLevel();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();

        Player0.Draw(spriteBatch);
        testPlant.Draw(spriteBatch);
        platforms.ForEach(p => p.Draw(spriteBatch));

        if (isPaused) pauseMenu.Draw(spriteBatch, screenSize);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
