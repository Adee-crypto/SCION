﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Sprint2.Controllers;
using Interfaces;
using System.Dynamic;
using System.Security.AccessControl;


namespace Sprint2;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;
    private IController keyboardController;
    // private IController mouseController; //this doesn't exist yet
    private IPlayer player;
    public IPlayer Player => player;
    private Plant testPlant;
    private List<Rectangle> objects;


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
        // mouseController = new MouseController(this); //this doesn't exist yet
        player = new Player();
        testPlant = new(Plant.Species.grass, (20, 20));
        objects = new List<Rectangle>();

        CommandUtil.keyCommandBindings = new()
        {
            {[Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.W, Keys.S, Keys.A, Keys.D], new Commands.LinkMoveCommand(this)},
            {[Keys.Z, Keys.N], new Commands.LinkAttackCommand(this)},
            //{[Keys.D1], new Commands.LinkItemCommand(this)},
            //{[Keys.E], new Commands.LinkDamagedCommand(this)}
            {[Keys.Q], new Commands.QuitCommand(this)}
        };
    }

    protected override void LoadContent()
    {
        LinkUtil.linkTexture = Content.Load<Texture2D>("Link");
        PlantUtil.spritesheet = Content.Load<Texture2D>("testsheet");
    }

    protected override void Update(GameTime gameTime)
    {
        keyboardController.Update();
        // mouseController.Update();

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

        player.Update(gameTime, objects);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();

        testPlant.Draw(spriteBatch);
        player.Draw(spriteBatch);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
