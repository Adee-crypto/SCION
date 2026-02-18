using Microsoft.Xna.Framework.Input;
using Interfaces;


namespace Sprint2.Controllers
{
    public class MouseController //: IController // To be adjusted later.
    {
        private MouseState currentMouseState = new MouseState();
        private MouseState previousMouseState = new MouseState();
        public MouseState Current => currentMouseState;
        public MouseState Previous => previousMouseState;

        public void Update()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }
    }
}
