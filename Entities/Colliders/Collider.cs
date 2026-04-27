using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;
using System;
using static Sprint2.Managers.BlockManager.Block;

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
    public void AddAcceleration(Vector2 delta) => Acceleration += delta; //used by ColliderMovement
    public void ClearAcceleration() => Acceleration = Vector2.Zero;      //used by ColliderMovement
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

    // Forwarding wrappers � keep call sites in Player, Enemy, Projectile unchanged.
    // Logic lives in ColliderMovement for cohesion.
    public ((int, int)? collisionCoords, bool isGrounded, SurfaceType surface) UpdateMovement(
        float dt, CollisionManager collisionManager)
        => ColliderMovement.UpdateMovement(this, dt, collisionManager);

    public void UpdatePlayerVelocity(bool isGrounded, SurfaceType surface, float dt)
        => ColliderMovement.UpdatePlayerVelocity(this, isGrounded, surface, dt);

    public bool Intersects(Collider other)
    {
        return (2 * Math.Abs(Center.X - other.Center.X) < (Size.X + other.Size.X))
            && (2 * Math.Abs(Center.Y - other.Center.Y) < (Size.Y + other.Size.Y));
    }

    public void KnockBack(Collider other)
    {
        int direction = Velocity.X < 0 ? -1 : 1;
        other.Momentum += new Vector2(direction * 100f, -100f);
    }
}

/// <summary>
/// Handles movement integration and surface friction for a Collider.
/// Extracted from Collider to keep that class focused on state and geometry.
/// </summary>
public static class ColliderMovement
{
    public static ((int, int)? collisionCoords, bool isGrounded, SurfaceType surface) UpdateMovement(
        Collider collider, float dt, CollisionManager collisionManager)
    {
        collider.AddAcceleration(Vector2.UnitY * collider.Gravity);
        collider.SetVelocity(collider.Velocity + collider.Acceleration * dt);
        collider.ClearAcceleration();
        return collisionManager.ManageBlockCollision(collider, collider.Velocity * dt);
    }

    public static void UpdatePlayerVelocity(Collider collider, bool isGrounded, SurfaceType surface, float dt)
    {
        if (isGrounded)
            ApplySurfaceFriction(collider, surface, dt);
        else
            collider.AddAcceleration(-collider.Velocity * Tunables.AirResistance.Value);
    }

    private static void ApplySurfaceFriction(Collider collider, SurfaceType surface, float dt)
    {
        switch (surface)
        {
            case SurfaceType.Bouncy:
                ApplyBouncySurface(collider, dt);
                break;
            case SurfaceType.Slippery:
                ApplyGroundFriction(collider, 0.4f);
                break;
            case SurfaceType.Sticky:
                ApplyGroundFriction(collider, 8f);
                break;
            default: // SurfaceType.Normal
                ApplyGroundFriction(collider);
                break;
        }
    }

    private static void ApplyBouncySurface(Collider collider, float dt)
    {
        const float bounciness = 0.65f;     // 0 = no bounce, 1 = perfect bounce
        const float bounceThreshold = 15f;  // stop bouncing below this speed
        if (Math.Abs(collider.Velocity.Y) > bounceThreshold)
            collider.SetVelocityY(-collider.Velocity.Y * bounciness);
        ApplyGroundFriction(collider, dt);
    }

    private static void ApplyGroundFriction(Collider collider, float multiplier = 1)
    {
        if (collider.Velocity.X < 0)
            collider.AddAcceleration(Vector2.UnitX*Math.Min(Tunables.GroundResistance.Value, 0f)*multiplier);
        else
            collider.AddAcceleration(-Vector2.UnitX*Math.Min(Tunables.GroundResistance.Value, 0f)*multiplier);
    }
}