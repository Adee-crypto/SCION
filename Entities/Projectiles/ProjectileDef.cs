using Microsoft.Xna.Framework;
using Sprint2.Util;

namespace Sprint2.Entities.Projectiles;

public class ProjectileDef(string projectileID, float projectileMaxTime = 5f, float projectileLaunchSpeed = 200f, float projectileGravity = 98f)
{
    //Initialization fields
    public string ProjectileID {get;}= projectileID;
    public float MaxLifetimeSeconds { get; } = projectileMaxTime;
    public float LaunchSpeed { get; } = projectileLaunchSpeed;
    public float Gravity { get; } = projectileGravity;
    
    //timing
    public int FrameIndex {get; private set;}
    public float Age { get; private set; }
    public float Time {get; private set;}
    public Rectangle[] Frames => SourceRects.ProjectileSourceRects[ProjectileID];

    //Assumes width of all frames is the same, and height is the same. Also assumes hitbox is the same as the frame size
    public Rectangle SourceRect { get { return SourceRects.ProjectileSourceRects[ProjectileID][FrameIndex]; } }
    public Vector2 Origin { get { return SourceRect.Size.ToVector2() / 2f; } }
    public Vector2 HitBox { get { return SourceRect.Location.ToVector2(); } }

    public void Update(GameTime gameTime)
    {
        Time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        int indexIncrease = (int)(Age % 1 + Time);
        FrameIndex = (FrameIndex + indexIncrease) % Frames.Length;
        Age += Time;
    }
}