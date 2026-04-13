using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;
using System.Collections.Generic;
using System.Linq;

namespace Sprint2.UI;

public class Menu(SpriteFont font)
{
    private readonly SpriteFont font = font;
    private readonly List<Button> buttons = [];

    public string Title { get; set; } = "";
    public bool DimBackground { get; set; } = true;

    public void AddButton(Button button) => buttons.Add(button);
    public void ClearButtons() => buttons.Clear();
    public void Update() => buttons.ForEach(b => b.Update());

    public void Draw(SpriteBatch spriteBatch, Vector2 screenSize)
    {
        if (DimBackground) spriteBatch.Draw(Assets.PixelTexture, new Rectangle(Vector2.Zero.ToPoint(), screenSize.ToPoint()), Color.Black * 0.5f);

        if (!string.IsNullOrWhiteSpace(Title))
        {
            float spacer = 20;
            Vector2 textSize = font.MeasureString(Title);
            if (buttons.Count > 0) {
                var topButton = buttons.MinBy(b => b.Position.Y);
                spacer = (topButton.Position.Y - topButton.Bounds.Height - 20f) > 0 ? (topButton.Position.Y - topButton.Bounds.Height - 20f) : 0f;
            }
            Vector2 titlePos = new((Consts.DefaultScreenSize.w - textSize.X) / 2, spacer);
            spriteBatch.DrawString(font, Title, titlePos, Color.Black);
        }

        buttons.ForEach(b => b.Draw(spriteBatch));
    }
}