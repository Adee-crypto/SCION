﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Sprint2.Controllers;
using Sprint2.Player;
using Interfaces;
using System.Dynamic;


namespace Sprint2;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;
    private IController keyboardController;
    // private IController mouseController; //this doesn't exist yet
    public IPlayer player;
    public Plant testPlant;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        keyboardController = new KeyBoardController(this);
        // mouseController = new MouseController(this); //this doesn't exist yet
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        LinkUtil.linkTexture = Content.Load<Texture2D>("Link");
        PlantUtil.spritesheet = Content.Load<Texture2D>("testsheet");
        player = new Link();
        testPlant = new(Plant.Species.grass, (20, 20));
    }

    protected override void Update(GameTime gameTime)
    {
        keyboardController.Update();
        // mouseController.Update();
        player.Update(gameTime);
        if (Keyboard.GetState().IsKeyDown(Keys.D1))
        {
            testPlant.Update(gameTime);
        }
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
