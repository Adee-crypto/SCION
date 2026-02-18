using Microsoft.Xna.Framework.Input;
using Interfaces;


namespace Sprint2.Controllers;

public class MouseController : IMouseController
{
    private MouseState currentMouseState = new MouseState();
    private MouseState previousMouseState = new MouseState();

    public bool IsPaused { get; set; } 

    public void Update()
    {
        previousMouseState = currentMouseState;
        currentMouseState = Mouse.GetState();
    }

    public bool IsLeftClick()
    {
        return previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed;
    }

    public bool IsRightClick()
    {
        return previousMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Pressed;
    }
}
