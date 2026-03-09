using Microsoft.Xna.Framework;
using Sprint2.Managers;
using System;

namespace Sprint2.Entities;

public class Collider(float gravity, float mass, Vector2 initialPosition, Vector2 initialVelocity, Vector2 size)
{
    //Newton would be proud
    public float Gravity { get; private set; } = gravity;
    public float Mass { get; private set; } = mass;
    public Vector2 InitialPosisiton { get; private set; } = initialPosition;
    public Vector2 Position { get; private set; } = initialPosition;
    public Vector2 Velocity { get; private set; } = initialVelocity;
    public Vector2 Size { get; private set; } = size;
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
        Position = InitialPosisiton;
        Velocity = Vector2.Zero;
    }

    public (bool isCollision, bool isGrounded) Update(float dt, CollisionManager collisionManager) {
        SetVelocityY(Velocity.Y + Gravity * dt);
        return collisionManager.ManageCollision(this, Velocity * dt);
    }

    public bool Intersects(Collider other) {
        return (Left < other.Right || Right > other.Left) && (Top < other.Bottom || Bottom > other.Top);
    }

    public void SetPositionX(float x) => Position = new(x, Position.Y);
    public void SetPositionY(float y) => Position = new(Position.X, y);
    public void SetPosition(Vector2 newPosition) => Position = newPosition;

    public void SetVelocityX(float x) => Velocity = new(x, Velocity.Y);
    public void SetVelocityY(float y) => Velocity = new(Velocity.X, y);
    public void SetVelocity(Vector2 newVelocity) => Velocity = newVelocity;

}