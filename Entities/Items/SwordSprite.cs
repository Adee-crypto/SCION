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
            if (player.PlayerState == State.Attack)
            {
                if (player.Direction.X >= 0) rotation = MathHelper.PiOver2;
                else rotation = -MathHelper.PiOver2;
                offset = player.Direction * player.Collider.Size.X;
            }
            else
            {
                rotation = 0;
                offset = -(player.Direction * player.Collider.Size.X / 2f);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 Pos)
    {
        Vector2 drawPos = Pos + offset;
        Vector2 origin = new(SwordSourceRect.Width / 2f, SwordSourceRect.Height / 2f);

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
