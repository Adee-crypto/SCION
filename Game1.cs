using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Sprint2.Controllers;
using Sprint2.Extensions;
using Sprint2.Screens;
using Sprint2.UI;
using Sprint2.UI.Overlays;
using Sprint2.Util;
using System;


namespace Sprint2;

public class Game1 : Game
{
    //graphics
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;
    private (int w, int h) ScreenSize {get;set;} = Consts.DefaultScreenSize;
    public Vector2 RawScreenSizeVec => new(ScreenSize.w, ScreenSize.h);
    public Vector2 VirtualScreenSize => Vector2.Transform(new(ScreenSize.w, ScreenSize.h), Matrix.Invert(transform)); //MAKE PRIVATE
    private Matrix transform;
    private float scale;

    //game state & data
    private readonly ScreenManager screenManager = new();
    private readonly OverlayManager overlayManager = new();
    public ScreenManager ScreenManager => screenManager;
    public OverlayManager OverlayManager => overlayManager;

    public Game1()
    {
        _graphics = new(this);
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += new EventHandler<EventArgs>(OnResize);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    private void OnResize(object sender, EventArgs e)
    {
        //resizing _graphics
        ScreenSize = (Window.ClientBounds.Width, Window.ClientBounds.Height);
        _graphics.PreferredBackBufferWidth = ScreenSize.w;
        _graphics.PreferredBackBufferHeight = ScreenSize.h;
        _graphics.ApplyChanges();
        //recalculate transformation matrix with letterboxing
        scale = Math.Min(ScreenSize.w / Consts.LevelSize.X, ScreenSize.h / Consts.LevelSize.Y)*0.9f;
        transform = Matrix.CreateTranslation(new(-Consts.LevelSize/2, 0)) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(new(RawScreenSizeVec/2, 0));
        MouseController.SetTransform(Matrix.Invert(transform));
    }

    protected override void Initialize()
    {
        spriteBatch = new(GraphicsDevice);

        KeyBindings.AttachKeyBindings(this); // Must be done after Game1's fields are initialized

        MediaPlayer.Volume = 0.2f;
        MediaPlayer.IsRepeating = true;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        try
        {
            Assets.PlayerTexture = Content.Load<Texture2D>("Link");
            Assets.ArrowTexture = Content.Load<Texture2D>("AimerArrow2");
            Assets.SwordTexture = Content.Load<Texture2D>("SwordItem");
            Assets.BlockSpriteSheet = Content.Load<Texture2D>("testsheet");
            Assets.BlockPlayerSpriteSheet = Content.Load<Texture2D>("SPRITESHEET");
            Assets.ButtonTexture = Content.Load<Texture2D>("DefaultButton");
            Assets.ResetTexture = Content.Load<Texture2D>("ResetButton");
            Assets.UiFont = Content.Load<SpriteFont>("UIFont");
            Assets.VoidspawnTexture = Content.Load<Texture2D>("VoidSpawns");

            Assets.PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            Assets.PixelTexture.SetData([Color.White]);

            Assets.GrassSound = Content.Load<SoundEffect>("Woowo");
            Assets.BackgroundMusic = Content.Load<Song>("backgroundambience");

            Levels.StoryLevelRegistry.LoadLevelData();  

            OnResize(null, null);

            screenManager.SetScreen(new ScreenMainMenu(this, screenManager));
        }
        catch (Exception ex)
        {
            Console.WriteLine("=== LOAD CONTENT ERROR ===");
            Console.WriteLine(ex.ToString());
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            throw;   // This will stop in debugger so you can see the stack trace
        }
    }

    public void TogglePause()
    {
        if (screenManager.Current is not IPausableScreen pausable) return;

        if (overlayManager.HasOverlays)
        {
            if (overlayManager.Peek() is PauseOverlay)
            {
                overlayManager.Pop();
                pausable.TogglePause();
            } else
            {
                overlayManager.Pop();
            }
            
            return;
        }

        pausable.TogglePause();
        overlayManager.Push(new PauseOverlay(this, overlayManager));
    }

    public void ResetLevel()
    {
        if (screenManager.Current is IResettableScreen resettable) resettable.Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        if (IsActive)
        { //prevents input from being processed when game is not active (e.g. alt-tabbed out)
            MouseController.Update();
            bool isPaused = screenManager.Current is IPausableScreen pause && pause.IsPaused;
            KeyboardController.Update(isPaused);
        }

        screenManager.Update(gameTime);
        overlayManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transform);
        screenManager.Draw(spriteBatch);
        overlayManager.Draw(spriteBatch);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
