using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Extensions;

namespace Sprint2.Util.Settings;

public abstract class BasePanel : ISettingsPanel
{
    protected Game1 Game { get; }
    
    protected Rectangle PanelBounds { get; set; }
    protected Rectangle ContentBounds { get; set; }

    protected int ScrollOffset { get; set; }
    protected int MaxScroll { get; set; }

    protected MouseState PreviousMouse { get; set; }
    protected KeyboardState PreviousKeyboard { get; set; }

    protected virtual int PanelPadding => 16;
    protected virtual int HeaderHeight => 40;
    protected virtual int FooterPadding => 16;
    protected virtual int ScrollStep => 32;

    protected BasePanel(Game1 game)
    {
        Game = game;
        RebuildLayout();
    }

    public void BuildPanel()
    {
        OnBuildPanel();
        ClampScroll();
    }

    protected virtual void RebuildLayout()
    {
        int w = Game.GraphicsDevice.Viewport.Width;
        int h = Game.GraphicsDevice.Viewport.Height;

        PanelBounds = new Rectangle(
            (int)(w * 0.52f),
            (int)(h * 0.18f),
            (int)(w * 0.40f),
            (int)(h * 0.68f)
        );

        ContentBounds = new Rectangle(
            PanelBounds.X + PanelPadding,
            PanelBounds.Y + HeaderHeight,
            PanelBounds.Width - PanelPadding * 2 - 12,
            PanelBounds.Height - HeaderHeight - FooterPadding
        );
    }

    protected void ClampScroll()
    {
        if (ScrollOffset < 0) ScrollOffset = 0;
        if (ScrollOffset > MaxScroll) ScrollOffset = MaxScroll;
    }

    protected void RecalculateMaxScroll(int contentHeight)
    {
        MaxScroll = Math.Max(0, contentHeight - ContentBounds.Height);
        ClampScroll();
    }

    protected void HandleScroll(MouseState mouse)
    {
        int wheelDiff = mouse.ScrollWheelValue - PreviousMouse.ScrollWheelValue;

        if (ContentBounds.Contains(mouse.Position))
        {
            if (wheelDiff > 0) ScrollOffset -= ScrollStep;
            if (wheelDiff < 0) ScrollOffset += ScrollStep;
            ClampScroll();
        }
    }

    protected virtual void DrawBackground(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Assets.PixelTexture, PanelBounds, Color.White * 0.92f);
    }

    protected abstract void OnBuildPanel();
    protected abstract void HandleInput(MouseState mouse, KeyboardState keyboard);
    protected abstract void DrawContent(SpriteBatch spriteBatch);
    protected abstract void DrawScrollbar(SpriteBatch spriteBatch);

    public virtual void Resize((int w, int h) size)
    {
        RebuildLayout();
        ClampScroll();
        BuildPanel();
    }

    public virtual void Update()
    {
        MouseState mouse = Mouse.GetState();
        KeyboardState keyboard = Keyboard.GetState();

        HandleScroll(mouse);
        HandleInput(mouse, keyboard);

        PreviousMouse = mouse;
        PreviousKeyboard = keyboard;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        DrawBackground(spriteBatch);
        DrawContent(spriteBatch);
        DrawScrollbar(spriteBatch);
    }
}