using Microsoft.Xna.Framework;
using Sprint2.Entities.Projectiles;
using Sprint2.Managers;
using Sprint2.Util;
using System;

namespace Sprint2.Entities;

public enum ColliderType {
    None,
    Player,
    Enemy,
    Projectile,
}

public class Collider(Vector2 initialPosition, Vector2 initialVelocity=new(), ColliderType type=ColliderType.None)
{
    public ColliderType Type {get;}= type;
    //Newton would be proud
    public float Gravity { get; private set; } = Consts.defaultGravity;
    public float Mass { get; init; } = 1;
    public Vector2 InitialPosition { private get; set; } = initialPosition;
    public Vector2 Position { get; private set; } = initialPosition;
    public Vector2 InitialVelocity { private get; set; } = initialVelocity;
    public Vector2 Velocity { get; private set; } = initialVelocity;
    public Vector2 Size { get; init; }
    public Vector2 Momentum { get => Velocity * Mass; set { Velocity = value / Mass; } }

    //Helpful values
    public Vector2 Center => Position + Size / 2f;
    public float Left => Position.X;
    public float Right => Position.X+Size.X;
    public float Top => Position.Y;
    public float Bottom => Position.Y+Size.Y;
    public float Angle => MathF.Atan2(Velocity.Y, Velocity.X);

    public void Reset()
    {
        Position = InitialPosition;
        Velocity = Vector2.Zero;
    }

    public (bool isCollision, bool isGrounded) Update(float dt, CollisionManager collisionManager) {
        SetVelocityY(Velocity.Y + Gravity * dt);
        return collisionManager.ManageCollision(this, Velocity * dt); //maybe change this to return *type* of other collider too
    }

    public bool Intersects(Collider other) {
        return (2 * Math.Abs(Center.X - other.Center.X) < (Size.X + other.Size.X)) && (2 * Math.Abs(Center.Y - other.Center.Y) < (Size.Y + other.Size.Y));
    }

    public void SetPositionX(float x) => Position = new(x, Position.Y);
    public void SetPositionY(float y) => Position = new(Position.X, y);
    public void SetPosition(Vector2 newPosition) => Position = newPosition;

    public void SetVelocityX(float x) => Velocity = new(x, Velocity.Y);
    public void SetVelocityY(float y) => Velocity = new(Velocity.X, y);
    public void SetVelocity(Vector2 newVelocity) => Velocity = newVelocity;

}