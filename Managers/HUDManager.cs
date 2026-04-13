using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Players;
using Sprint2.HUD;

namespace Sprint2.Managers;

public class HUDManager
{
    private readonly Player player;
    private readonly HealthBar healthBar;

    public HUDManager(Player player)
    {
        this.player = player;
        healthBar = new(player, new(10, 10), new(100, 20));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        healthBar.Draw(spriteBatch);
    }

    public void Resize(Vector2 size)
    {
        healthBar.Resize(size);
    }

    /*public void Update(GameTime gameTime)
    {
        // Currently, the health bar is the only HUD element that needs updating.
        // If we add more elements in the future, we can update them here as well.
    }*/
}