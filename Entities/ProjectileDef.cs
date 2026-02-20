using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2;

public class ProjectileDef
{
    public string Id { get; }
    public Texture2D Texture { get; }
    public Vector2 Size { get; }
    public float Gravity { get; }
    public float LaunchSpeed { get; }
    public float MaxLifetimeSeconds { get; }
    public Vector2 Origin { get; }

    public ProjectileDef(string projectileID, Texture2D projectileTexture, Vector2 projectileSize, float projectileMaxTime = 5f, float projectileLaunchSpeed = 200f, float projectileGravity = 98f)
    {
        Id = projectileID;
        Texture = projectileTexture;
        Size = projectileSize;
        MaxLifetimeSeconds = projectileMaxTime;
        LaunchSpeed = projectileLaunchSpeed;
        Gravity = projectileGravity;

        Origin = new Vector2(projectileTexture.Width / 2f, projectileTexture.Height / 2f);
    }
}