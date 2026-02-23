using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2;

public class ProjectileDef(string projectileID, Texture2D projectileTexture, Rectangle sourceRect,Vector2 hitBox, float projectileMaxTime = 5f, float projectileLaunchSpeed = 200f, float projectileGravity = 98f)
{
    public string Id { get; } = projectileID;
    public Texture2D Texture { get; } = projectileTexture;
    public Rectangle SourceRect {get;} = sourceRect;
    public Vector2 HitBox { get; } = hitBox;
    public float Gravity { get; } = projectileGravity;
    public float LaunchSpeed { get; } = projectileLaunchSpeed;
    public float MaxLifetimeSeconds { get; } = projectileMaxTime;
    public Vector2 Origin { get; } = new Vector2(sourceRect.Width / 2f, sourceRect.Height / 2f);
}