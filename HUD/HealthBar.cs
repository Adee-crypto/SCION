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

    public void Resize((int w, int h) screenSize)
    {
        float changeWidth = screenSize.w / (float)Consts.DefaultScreenSize.w;
        float changeHeight = screenSize.h / (float)Consts.DefaultScreenSize.h;

        size = new(initialSize.X * changeWidth, initialSize.Y * changeHeight);
        position = new(initialPosition.X * changeWidth, initialPosition.Y * changeHeight);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw border
        spriteBatch.Draw(Assets.PixelTexture, new Rectangle((int)position.X - 2, (int)position.Y - 2, (int)size.X + 4, (int)size.Y + 4), Color.Black);
        
        // Draw background
        spriteBatch.Draw(Assets.PixelTexture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), Color.DarkGray);

        // Calculate health ratio
        float healthRatio = player.Health / (float)player.MaxHealth;

        // Draw foreground based on health ratio
        spriteBatch.Draw(Assets.PixelTexture, new Rectangle((int)position.X, (int)position.Y, (int)(size.X * healthRatio), (int)size.Y), Color.LimeGreen);
    }
}