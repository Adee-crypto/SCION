using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Controllers;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.UI.Settings;

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
        (int w, int h) = Game.VirtualScreenSize.ToPoint();

        PanelBounds = new Rectangle(
            (int)(w * 0.27f),
            (int)(h * 0.27f),
            (int)(w * 0.40f),
            (int)(h * 0.56f)
        );

        ContentBounds = new Rectangle(
            PanelBounds.X + PanelPadding,
            PanelBounds.Y + PanelPadding,
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

        if (ContentBounds.Contains(MouseController.VirtualMousePos))
        {
            if (wheelDiff > 0) ScrollOffset -= ScrollStep;
            if (wheelDiff < 0) ScrollOffset += ScrollStep;
            ClampScroll();
        }
    }

    protected virtual void DrawBackground(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Assets.PixelTexture, PanelBounds, Color.Gray * 0.12f);
        spriteBatch.Draw(
            Assets.PixelTexture,
            new Rectangle(ContentBounds.X, ContentBounds.Y - 8, ContentBounds.Width, 2),
            Color.Black * 0.25f
        );
    }

    protected abstract void OnBuildPanel();
    protected abstract void HandleInput(MouseState mouse, KeyboardState keyboard);
    protected abstract void DrawContent(SpriteBatch spriteBatch);
    protected abstract void DrawScrollbar(SpriteBatch spriteBatch);

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