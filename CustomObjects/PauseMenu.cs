using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2.UI
{
    public class PauseMenu
    {
        private readonly SpriteFont font;
        private readonly Texture2D overlay;

        public PauseMenu(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            this.font = font;

            overlay = new Texture2D(graphicsDevice, 1, 1);
            overlay.SetData(new[] { Color.Black });
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            string pauseText = "GAME PAUSED";

            Vector2 textSize = font.MeasureString(pauseText);
            Vector2 center = new Vector2((graphicsDevice.Viewport.Width - textSize.X) / 2, (graphicsDevice.Viewport.Height - textSize.Y) / 2);
            
            spriteBatch.Draw(overlay, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White * 0.5f);

            spriteBatch.DrawString(font, pauseText, center, Color.Black);
        }
    }
}