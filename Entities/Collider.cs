using System;
using Interfaces;
using Microsoft.Xna.Framework;

namespace Sprint2.Entities;

public class Collider
{
    private Vector2 initialPos;
    public Vector2 InitialPos => initialPos;
    public Vector2 Position;
    public Vector2 Velocity;
    public float Angle => MathF.Atan2(Velocity.Y, Velocity.X);
    //im so tired of these warnings
    // private Vector2 position;
    // public Vector2 Position {get => position; set => position = value;}
    // private Vector2 velocity;
    // public Vector2 Velocity {get => velocity; set => velocity = value;}
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
}