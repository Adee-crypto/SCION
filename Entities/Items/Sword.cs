using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Players;
using Sprint2.Util;

namespace Sprint2.Entities.Items;

public class Sword
{
    private Player player;
    public Vector2 Posistion { get; set; }
    public bool IsPickedUp { get; private set; }
    private const float PickupRadius = 12f;
    private readonly SwordSprite swordSprite = new();

    private float bobTimer;
    private const float BobSpeed = 2.5f;   
    private const float BobAmplitude = 3f;

    private static readonly Rectangle SwordSourceRect = new(0, 0, 8, 16);

    public Sword(Player player)
    {
        this.player = player;
        IsPickedUp = false;
        bobTimer = 0f;
    }

    public void Spawn(Vector2 spawnPosition)
    {
        Posistion = spawnPosition;
    }

    public void Reset()
    {

    }

    public void Update(GameTime gameTime, Player player)
    {
        if (IsPickedUp) return;

        bobTimer += (float)gameTime.ElapsedGameTime.TotalSeconds * BobSpeed;

        float dist = Vector2.Distance(player.Collider.Center, Posistion);
        if (dist <= PickupRadius)
        {
            IsPickedUp = true;
            player.PickUpSword();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsPickedUp) 
        { 
            swordSprite.Draw(spriteBatch, player.Collider.Center, player.Direction, player.PlayerState == State.Attack); 
        }
        else
        {
            float bobOffset = (float)System.Math.Sin(bobTimer) * BobAmplitude;
            Vector2 drawPos = Posistion + new Vector2(0, bobOffset);
            Vector2 origin = new(SwordSourceRect.Width / 2f, SwordSourceRect.Height / 2f);

            spriteBatch.Draw(
                Assets.SwordTexture,
                drawPos,
                SwordSourceRect,
                Color.White,
                rotation: 0f,
                origin,
                scale: 1f,
                SpriteEffects.None,
                layerDepth: 0f
            );
        }
    }
}