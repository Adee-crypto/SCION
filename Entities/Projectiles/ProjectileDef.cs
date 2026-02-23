using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2;

public class ProjectileDef(string projectileID, Texture2D projectileTexture, Vector2 projectileSize, float projectileMaxTime = 5f, float projectileLaunchSpeed = 200f, float projectileGravity = 98f)
{
    public string Id { get; } = projectileID;
    public Texture2D Texture { get; } = projectileTexture;
    public Vector2 Size { get; } = projectileSize;
    public float Gravity { get; } = projectileGravity;
    public float LaunchSpeed { get; } = projectileLaunchSpeed;
    public float MaxLifetimeSeconds { get; } = projectileMaxTime;
    public Vector2 Origin { get; } = new Vector2(projectileTexture.Width / 2f, projectileTexture.Height / 2f);
}