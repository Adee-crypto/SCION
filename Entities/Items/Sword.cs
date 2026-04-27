using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Players;
using System;

namespace Sprint2.Entities.Items;

public class Sword
{
    private SwordSprite swordSprite;
    private Collider collider;
    private Player player;
    private float bobTimer;

    private const float BobSpeed = 3f;
    private const float BobAmplitude = 3f;

    public Sword(Player player)
    {
        swordSprite = new();
        collider = new(Vector2.Zero) { Size = new Vector2(8, 16)};
        this.player = player;
        bobTimer = 0;
    }

    public void Spawn(Vector2 spawnPosition)
    {
        collider.SetInitialPosition(spawnPosition);
        collider.SetPosition(spawnPosition);
    }

    public void Reset()
    {
        swordSprite = new();
        collider.Reset();
        bobTimer = 0;
    }

    public void Update(GameTime gameTime, Player player)
    {
        if (!player.HasSword)
        {
            bobTimer += (float)gameTime.ElapsedGameTime.TotalSeconds * BobSpeed;
            collider.SetPosition(new Vector2(collider.InitialPosition.X, collider.InitialPosition.Y + (float)Math.Sin(bobTimer) * BobAmplitude));

            if (collider.Intersects(player.Collider)) player.PickUpSword();          
        }

        swordSprite.Update(gameTime, player);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!player.HasSword) swordSprite.Draw(spriteBatch, collider.Position);
        else if (player.HasSword && player.Item == Item.Sword) swordSprite.Draw(spriteBatch, player.Collider.Center);
    }
}