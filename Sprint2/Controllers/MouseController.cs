using Microsoft.Xna.Framework.Input;

namespace Sprint2.Controllers;

class MouseController(int windowWidth, int windowHeight) : IController
{
    MouseState mouseState;
    int quadWidth = windowWidth / 2, quadHeight = windowHeight / 2;

    public void Update(ref int command)
    {
        mouseState = Mouse.GetState();
        if (mouseState.RightButton == ButtonState.Pressed) {
            command = 0;
        } else if (mouseState.LeftButton == ButtonState.Pressed && 
                   mouseState.X >= 0 && mouseState.X <= windowWidth &&
                   mouseState.Y >= 0 && mouseState.Y <= windowHeight) {
            command = 1;
            if (mouseState.X > quadWidth)
            {
                command += 1;
            }
            if (mouseState.Y > quadHeight)
            {
                command += 2;
            }
        }
    }
}