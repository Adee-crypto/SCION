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
    public void Resize((int, int) size) => buttons.ForEach(b => b.Resize(size));

    public void Draw(SpriteBatch spriteBatch, (int w, int h) screenSize)
    {
        if (DimBackground) spriteBatch.Draw(Assets.PixelTexture, new Rectangle(0, 0, screenSize.w, screenSize.h), Color.Black * 0.5f);

        if (!string.IsNullOrWhiteSpace(Title))
        {
            Vector2 textSize = font.MeasureString(Title);
            var topButton = buttons.MinBy(b => b.Position.Y);
            float spacer = (topButton.Position.Y - topButton.Bounds.Height - 20f) > 0 ? (topButton.Position.Y - topButton.Bounds.Height - 20f) : 0f;
            Vector2 titlePos = new((screenSize.w - textSize.X) / 2, spacer);
            spriteBatch.DrawString(font, Title, titlePos, Color.Black);
        }

        buttons.ForEach(b => b.Draw(spriteBatch));
    }
}