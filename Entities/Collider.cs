using System;
using Microsoft.Xna.Framework;

namespace Sprint2.Entities;

public class Collider {
    //Newton would be proud
    public float Mass { get; set; }
    public Vector2 InitialPos { get; private set; }
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get => Momentum / Mass; }
    public Vector2 Momentum { get; private set; }

    //Dimensions & helpful values
    private Vector2 Size;
    public float Angle => MathF.Atan2(Velocity.Y, Velocity.X);
    public Vector2 Center => Position + Size / 2f;
    public Rectangle Hitbox => new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); //probably make this not int at some point

    public Collider(Vector2 pos, Vector2 size, float mass=1) {
        InitialPos = pos;
        Size = size;
        Mass = mass;
        Reset();
    }

    public void Reset() {
        Position = InitialPos;
        Momentum = Vector2.Zero;
    }

    public void SetPositionX(float x) => Position = new (x, Position.Y);
    public void SetPositionY(float y) => Position = new (Position.X, y);
    public void SetPosition(Vector2 newPosition) => Position = newPosition;

    public void SetMomentumX(float x) => Momentum = new(x, Momentum.Y);
    public void SetMomentumY(float y) => Momentum = new(Momentum.X, y);
    public void SetMomentum(Vector2 newMomentum) => Momentum = newMomentum;

}