using Microsoft.Xna.Framework.Input;
using Interfaces;
using System.Numerics;


namespace Sprint2.Controllers;

public class MouseController : IController // To be adjusted later.
{
    private MouseState currentMouseState = new MouseState();
    private MouseState previousMouseState = new MouseState();
    public MouseState Current => currentMouseState;
    public MouseState Previous => previousMouseState;
    public bool IsPaused { get; set; } 

    public void Update()
    {
        previousMouseState = currentMouseState;
        currentMouseState = Mouse.GetState();
    }

    public Vector2 MousePosition()
    {
        return new Vector2(currentMouseState.X, currentMouseState.Y);
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
