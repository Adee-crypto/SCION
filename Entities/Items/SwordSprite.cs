using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Players;
using Sprint2.Util;
using System;

namespace Sprint2.Entities.Items;

public class SwordSprite
{
    private float rotation;
    private Vector2 offset;

    private static readonly Rectangle SwordSourceRect = new(0, 0, 8, 16);

    public SwordSprite()
    {
        rotation = 0;
        offset = Vector2.Zero;
    }

    public void Update(GameTime gameTime, Player player)
    {
        if (player.HasSword && player.Item == Item.Sword)
        {
            offset = Vector2.Zero;
            if (player.PlayerState == State.Attack)
            {
                rotation = player.Direction.X * MathHelper.PiOver2;
                offset.X = player.Direction.X * player.Collider.Size.X;
                if (player.Direction.X > 0) offset.Y += 1; // one pixel deviation due to rotation
            }
            else
            {
                rotation = MathHelper.Pi;
                offset.X = -(player.Direction.X * player.Collider.Size.X / 2f);
                if (player.Direction.X > 0) offset.X -= 2; // two pixels deviation due to rotation
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 Pos)
    {
        Vector2 drawPos = Pos + offset;
        Vector2 origin = new Vector2(SwordSourceRect.Width, SwordSourceRect.Height) * 0.5f;

        spriteBatch.Draw(
            Assets.SwordTexture,
            drawPos,
            SwordSourceRect,
            Color.White,
            rotation,
            origin,
            scale: 1f,
            SpriteEffects.None,
            layerDepth: 0f
        );
    }
}
