using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Players;
using Sprint2.Util;

namespace Sprint2.HUD;

public class HealthBar
{
    private readonly Player player;
    private readonly Vector2 initialPosition;
    private readonly Vector2 initialSize;
    private Vector2 position;
    private Vector2 size;

    public HealthBar(Player player, Vector2 position, Vector2 size)
    {
        this.player = player;
        initialPosition = position;
        this.position = position;
        initialSize = size;
        this.size = size;
    }

    public void Resize(Vector2 screenSize)
    {
        Vector2 change = screenSize / Consts.DefaultScreenSizeVec;
        size = initialSize * change;
        position = initialPosition * change;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw border
        spriteBatch.Draw(Assets.PixelTexture, new Rectangle((int)position.X - 2, (int)position.Y - 2, (int)size.X + 4, (int)size.Y + 4), Color.Black);

        // Draw background
        spriteBatch.Draw(Assets.PixelTexture, new Rectangle(position.ToPoint(), size.ToPoint()), Color.DarkGray);

        // Calculate health ratio
        float healthRatio = player.Health / (float)player.MaxHealth;

        // Draw foreground based on health ratio
        spriteBatch.Draw(Assets.PixelTexture, new Rectangle((int)position.X, (int)position.Y, (int)(size.X * healthRatio), (int)size.Y), Color.LimeGreen);
    }
}