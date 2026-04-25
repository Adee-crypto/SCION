using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;
using System;

namespace Sprint2.Entities;

public enum ColliderType
{
    None,
    Player,
    Enemy,
    Projectile,
    Sword
}

public class Collider(Vector2 initialPosition, Vector2 initialVelocity = new(), ColliderType type = ColliderType.None)
{
    //Init fields
    public ColliderType Type { get; } = type;
    public float Gravity { get; set; } = Tunables.DefaultGravity.Value;
    public Vector2 Size { get; init; }
    public float Mass { get; init; } = 1;

    //Dynamics
    public Vector2 InitialPosition { get; private set; } = initialPosition;
    public void SetInitialPosition(Vector2 initialPosition) { InitialPosition = initialPosition; } //this is explicit to discourage use
    private Vector2 InitialVelocity = initialVelocity;
    public void SetInitialVelocity(Vector2 initialVelocity) { InitialVelocity = initialVelocity; }
    public Vector2 Position { get; private set; } = initialPosition;
    public void SetPosition(Vector2 newPosition) => Position = newPosition; //this is explicit to discourage use
    public Vector2 Velocity { get; private set; } = initialVelocity;
    public void SetVelocityX(float x) => Velocity = new(x, Velocity.Y); //this is explicit to discourage use
    public void SetVelocityY(float y) => Velocity = new(Velocity.X, y); //this is explicit to discourage use
    public void SetVelocity(Vector2 newVelocity) => Velocity = newVelocity; //this is explicit to discourage use
    public Vector2 Acceleration { get; private set; }
    public Vector2 Force { get => Acceleration * Mass; set { Acceleration = value / Mass; } }
    public Vector2 Momentum { get => Velocity * Mass; set { Velocity = value / Mass; } }

    //Helpful values
    public Vector2 Center => Position + Size / 2f;
    public float Left => Position.X;
    public float Right => Position.X + Size.X;
    public float Top => Position.Y;
    public float Bottom => Position.Y + Size.Y;
    public float Angle => MathF.Atan2(Velocity.Y, Velocity.X);

    public void Reset()
    {
        Position = InitialPosition;
        Velocity = InitialVelocity;
        Acceleration = Vector2.Zero;
    }

    public ((int, int)? collisionCoords, bool isGrounded, SurfaceType surface) UpdateMovement(float dt, CollisionManager collisionManager)
    {
        //add all acceleration to velocity and clear it
        Acceleration += Vector2.UnitY * Gravity;
        Velocity += Acceleration * dt;
        Acceleration = Vector2.Zero;
        //update position
        return collisionManager.ManageBlockCollision(this, Velocity * dt);
    }

    public void UpdatePlayerVelocity(bool isGrounded, SurfaceType surface, float dt)
    {
        if (isGrounded)
        {
            switch (surface)
            {
                case SurfaceType.Bouncy:
                    // Only added vertical bounce
                    const float bounciness = 0.65f;          // 0 = no bounce, 1 = perfect bounce
                    const float bounceThreshold = 15f;        //stop bouncing below this speed
                    if (Math.Abs(Velocity.Y) > bounceThreshold)
                        SetVelocityY(-Velocity.Y * bounciness);
                    // Normal horizontal friction
                    ApplyGroundFriction(dt);
                    break;

                case SurfaceType.Slippery:
                    // No Work needed to reduce horizontal velocity
                    break;

                case SurfaceType.Sticky:
                    
                    const float stickyFrictionMultiplier = 4f;  // multiplier on top of normal friction
                    float stickyResistance = Tunables.GroundResistance.Value * stickyFrictionMultiplier;
                    if (Velocity.X < 0) SetVelocityX(Math.Min(Velocity.X + stickyResistance * dt, 0f));
                    else SetVelocityX(Math.Max(Velocity.X - stickyResistance * dt, 0f));
                    break;
                    
                    

                default: // SurfaceType.Normal
                    ApplyGroundFriction(dt);
                    break;
            }
        }
        else
        {
            // Air resistance
            Acceleration -= Velocity * Tunables.AirResistance.Value;
        }
    }


    private void ApplyGroundFriction(float dt)
    {
        if (Velocity.X < 0) SetVelocityX(Math.Min(Velocity.X + Tunables.GroundResistance.Value * dt, 0f));
        else SetVelocityX(Math.Max(Velocity.X - Tunables.GroundResistance.Value * dt, 0f));
    }

    public bool Intersects(Collider other)
    {
        return (2 * Math.Abs(Center.X - other.Center.X) < (Size.X + other.Size.X)) && (2 * Math.Abs(Center.Y - other.Center.Y) < (Size.Y + other.Size.Y));
    }

    public void KnockBack(Collider other)
    {
        int direction = Velocity.X < 0 ? -1 : 1;
        other.Momentum += new Vector2(direction * 100f, -100f);
    }
}