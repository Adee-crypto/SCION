using Microsoft.Xna.Framework;
using System;

namespace Sprint2.Entities;

public class Collider
{
    //Newton would be proud
    public float Gravity { get; private set; }
    public float Mass { get; private set; }
    public Vector2 InitialPosisiton { get; private set; }
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get; private set; }
    public Vector2 Size { get; private set; }
    public Vector2 Momentum { get { return Velocity * Mass; } set { Velocity = value / Mass; } }

    //Dimensions & helpful values
    public float Angle => MathF.Atan2(Velocity.Y, Velocity.X);
    public Vector2 Center => Position + Size / 2f;
    public Rectangle Hitbox => new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); //probably make this not int at some point

    public Collider(float gravity, float mass, Vector2 initialPosition, Vector2 initialVelocity, Vector2 size)
    {
        Gravity = gravity;
        Mass = mass;
        InitialPosisiton = initialPosition;
        Position = initialPosition;
        Velocity = initialVelocity;
        Size = size;
    }

    public void Reset()
    {
        Position = InitialPosisiton;
        Velocity = Vector2.Zero;
    }

    public void SetPositionX(float x) => Position = new(x, Position.Y);
    public void SetPositionY(float y) => Position = new(Position.X, y);
    public void SetPosition(Vector2 newPosition) => Position = newPosition;

    public void SetVelocityX(float x) => Velocity = new(x, Velocity.Y);
    public void SetVelocityY(float y) => Velocity = new(Velocity.X, y);
    public void SetVelocity(Vector2 newVelocity) => Velocity = newVelocity;

}