using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sprint2.Controllers;

public static class MouseController// : IMouseController
{
    private static MouseState currentMouseState;
    private static MouseState previousMouseState;

    public static Point RawMousePos => currentMouseState.Position;
    private static Matrix transform;
    public static void SetTransform(Matrix transform) => MouseController.transform = transform;  
    public static Point VirtualMousePos => Vector2.Transform(RawMousePos.ToVector2(), transform).ToPoint();

    public static void Update()
    {
        previousMouseState = currentMouseState;
        currentMouseState = Mouse.GetState();
    }

    public static bool IsLeftClick()
    {
        return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
    }

    public static bool IsRightClick()
    {
        return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
    }

    public static bool IsLeftClickHeld()
    {
        return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed;
    }

    public static bool IsRightClickHeld()
    {
        return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Pressed;
    }
}
