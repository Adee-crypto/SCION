using System;
using Sprint2.Extensions;
using Microsoft.Xna.Framework;

namespace Sprint2.Entities;

public class Collider
{
    private Vector2 initialPos;
    public Vector2 InitialPos => initialPos;
    //public Vector2 Position;
    //public Vector2 Velocity;
    public float Angle => MathF.Atan2(Velocity.Y, Velocity.X);
    private Vector2 position;
    public Vector2 Position {get => position; private set => position = value;}
     private Vector2 velocity;
    public Vector2 Velocity {get => velocity; private set => velocity = value;}
    private Vector2 Size;
    public Vector2 Center => Position + Size / 2f;
    public Rectangle Hitbox => new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); //probably make this not int at some point

    public Collider(Vector2 pos, Vector2 size) {
        initialPos = pos;
        Size = size;
        Reset();
    }

    public void Reset() {
        Position = initialPos;
        Velocity = Vector2.Zero;
    }

    public void SetVelocityX(float x)
    {
        Velocity = new Vector2(x, Velocity.Y);
    }

    public void SetVelocityY(float y)
    {
        Velocity = new Vector2(Velocity.X, y);
    }

    public void SetVelocity(float x, float y)
    {
        Velocity = new Vector2(x, y);
    }

    public void SetVelocity(Vector2 newVelocity)
    {
        Velocity = newVelocity;
    }

    public void SetPositionX(float x)
    {
        Position = new Vector2(x, Position.Y);
    }

    public void SetPositionY(float y)
    {
        Position = new Vector2(Position.X, y);
    }

    public void SetPosition(Vector2 newPosition)
    {
        Position = newPosition;
    }
}