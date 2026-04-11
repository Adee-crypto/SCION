using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;

namespace Sprint2.Entities.Items;

public class SwordSprite
{
    private const float HoldDistanceX = 14f;
    private const float HoldOffsetY = 4f;
    private const float AttackLunge = 5f;
    private static readonly Rectangle SwordSourceRect = new(0, 0, 8, 16);

    public void Draw(SpriteBatch spriteBatch, Vector2 playerPos, Vector2 direction, bool isAttacking)
    {
        float rotation;
        if (direction.X >= 0) rotation = MathHelper.PiOver2;
        else rotation = -MathHelper.PiOver2;

        float lunge;
        if (isAttacking) lunge = AttackLunge;        
        else lunge = 0f;
        
        float offsetX;
        if (direction.X >= 0) offsetX = HoldDistanceX + lunge;        
        else offsetX = -(HoldDistanceX + lunge);
        

        Vector2 drawPos = playerPos + new Vector2(offsetX, HoldOffsetY);
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
