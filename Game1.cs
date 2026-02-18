﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Sprint2.Controllers;
using Sprint2.UI;
using Interfaces;
using System.Dynamic;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.ComponentModel.Design;
using System.Linq;


namespace Sprint2;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;
    private IController keyboardController;
    private MouseController mouseController;
    private Player player; //SWITCH THESE TO IPlayer LATER
    public Player Player => player;
    private PauseMenu pauseMenu;
    private SpriteFont uiFont;
    private bool isPaused;
    public bool IsPaused => isPaused;
    private Plant testPlant;
    private List<Rectangle> objects;
    private List<Platform> platforms;


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
        player = new Player((20, 0));
        testPlant = new(Plant.Species.grass, (20, 20));
        objects = [];
        platforms = new();
        platforms.Add(new Platform(Platform.Type.stonebrick, 0, 16*25, 40, 1));

        CommandUtil.AttachCommandBindings(this);
    }

    protected override void LoadContent()
    {
        LinkUtil.linkTexture = Content.Load<Texture2D>("Link");
        PlantUtil.spritesheet = Content.Load<Texture2D>("testsheet");
        PlatformUtil.spritesheet = Content.Load<Texture2D>("testsheet");
        ButtonUtil.buttonTexture = Content.Load<Texture2D>("DefaultButton");
        uiFont = Content.Load<SpriteFont>("UIFont");
        pauseMenu = new PauseMenu(uiFont, GraphicsDevice);

        Vector2 resumePosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - 100, GraphicsDevice.Viewport.Height / 2 - 60);
        Vector2 quitPosition = new Vector2(resumePosition.X, resumePosition.Y + 60);
        pauseMenu.AddButton(new Button(uiFont, ButtonUtil.buttonTexture, "Resume", () => TogglePause(), new Vector2(200, 50), resumePosition));
        pauseMenu.AddButton(new Button(uiFont, ButtonUtil.buttonTexture, "Quit", () => Exit(), new Vector2(200, 50), quitPosition));
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
    }

    protected override void Update(GameTime gameTime)
    {
        mouseController.Update();

        if (!isPaused)
        {
            keyboardController.Update();
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

        testPlant.Draw(spriteBatch);
        player.Draw(spriteBatch);

        foreach (var p in platforms) p.Draw(spriteBatch);

        if (isPaused)
        {
            pauseMenu.Draw(spriteBatch, GraphicsDevice);
        }

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
