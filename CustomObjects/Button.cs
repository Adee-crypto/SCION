using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint2.UI
{
    public class Button
    {
        private readonly SpriteFont font;
        private readonly Texture2D texture;
        private readonly string text;
        private readonly Action onClick;
        private readonly Rectangle bounds;
        private readonly Vector2 textPos;

        public Button(SpriteFont textFont, Texture2D buttonTexture, string buttonText, Action command, Vector2 size, Vector2 pos)
        {
            font = textFont;
            texture = buttonTexture;
            text = buttonText;
            onClick = command;
            bounds = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
            textPos = new Vector2(pos.X + (size.X - font.MeasureString(buttonText).X) / 2, pos.Y + (size.Y - font.MeasureString(buttonText).Y) / 2);
        }

        public void Update(MouseState mouse, MouseState prevMouse)
        {
            bool hover = bounds.Contains(mouse.Position);
            bool click = hover && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released;

            if (click)
            {
                onClick?.Invoke();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);
            spriteBatch.DrawString(font, text, textPos, Color.Black);
        }
    }
}