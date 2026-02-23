using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2;

public class ProjectileDef(string projectileID, float projectileMaxTime = 5f, float projectileLaunchSpeed = 200f, float projectileGravity = 98f)
{
    //Drawing and time
    public Rectangle[] frames => ProjectileUtil.projectileFrames[projectileID];
    public int frameIndex = 0;
    public float age = 0;
    public float time = 0;

    //Assumes width of all frames is the same, and height is the same. Also assumes hitbox is the same as the frame size
    public string projectileID = projectileID;
    public float Gravity {get;} = projectileGravity;
    public float LaunchSpeed {get;} = projectileLaunchSpeed;
    public float MaxLifetimeSeconds {get;} = projectileMaxTime;

    public Rectangle SourceRect {get { return ProjectileUtil.projectileFrames[projectileID][frameIndex]; } }
    public Vector2 Origin {get { return SourceRect.Size.ToVector2() / 2f; } }
    public Vector2 HitBox {get { return SourceRect.Location.ToVector2(); } }

    public void Update(GameTime gameTime) {
        time = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
        int indexIncrease = (int) (age % 1 + time);
        frameIndex = (frameIndex + indexIncrease) % frames.Length;

        age += time;
    }
}