using Interfaces;
using Microsoft.Xna.Framework.Input;


namespace Sprint2.Controllers;

public class MouseController : IMouseController
{
    private MouseState currentMouseState;
    private MouseState previousMouseState;

    public bool IsPaused { get; set; }

    public void Update()
    {
        previousMouseState = currentMouseState;
        currentMouseState = Mouse.GetState();
    }

    public bool IsLeftClick()
    {
        return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
    }

    public bool IsRightClick()
    {
        return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
    }

    public bool IsLeftClickHeld()
    {
        return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed;
    }

    public bool IsRightClickHeld()
    {
        return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Pressed;
    }
}
