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
    public Vector2 Force { get => Acceleration * Mass; set { Acceleration = value / Mass; } }
    public Vector2 Momentum { get => Velocity * Mass; set { Velocity = value / Mass; } }

    //states
    public bool IsGrounded { get; private set; }
    public bool SetGrounded(bool isGrounded) => IsGrounded = isGrounded; //explicit to discourage use
    public float BlockFrictionCoefficient { get; private set; }
    public void SetBlockFrictionCoefficient(float k) => BlockFrictionCoefficient = k; //explicit to discourage use


    //Helpful values
    public Vector2 Center => Position + Size / 2f;
    public float Left => Position.X;
    public float Right => Position.X + Size.X;
    public float Top => Position.Y;
    public float Bottom => Position.Y + Size.Y;
    public float Angle => MathF.Atan2(Velocity.Y, Velocity.X);
    public int DirectionX { get => Velocity.X < 0 ? -1 : 1; }

    public void Reset()
    {
        Position = InitialPosition;
        IsGrounded = false;
        Velocity = InitialVelocity;
        Acceleration = Vector2.Zero;
    }

    public bool Intersects(Collider other)
    {
        return (2 * Math.Abs(Center.X - other.Center.X) < (Size.X + other.Size.X))
            && (2 * Math.Abs(Center.Y - other.Center.Y) < (Size.Y + other.Size.Y));
    }

    public void KnockBack(Collider other)
    {
        other.Momentum += 100f * new Vector2(DirectionX, -1);
    }

    public (int, int)? Update(CollisionManager collisionManager, float dt) {
        //gravity
        Acceleration += Vector2.UnitY * Gravity;
        
        //friction
        if (IsGrounded) {
            //constant dynamic ground friction
            float deltaV = dt * Tunables.GroundResistance.Value * BlockFrictionCoefficient;
            if (deltaV > Math.Abs(Velocity.X)) {
                Velocity *= Vector2.UnitY;
            } else {    
                Velocity -= DirectionX * Vector2.UnitX * deltaV;
            }
        } else {
            //air resistance proportional to velocity
            Acceleration -= Velocity * Tunables.AirResistance.Value;
        }

        Velocity += Acceleration * dt;
        
        var (collisionCoords, surface) = collisionManager.ManageBlockCollision(this, Velocity * dt);
        BlockFrictionCoefficient = surface switch {SurfaceType.Slippery => 0.2f, SurfaceType.Sticky => 6f, _ => 1f};

        Acceleration = new();
        return collisionCoords;
    }
}