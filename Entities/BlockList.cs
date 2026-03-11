using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;
using System.Collections.Generic;
using static Sprint2.Entities.BlockType;

namespace Sprint2.Entities;

public enum BlockType
{
    None,
    //plants
    Grass,
    Apple,
    Pineapple,
    Sandbox,
    //platforms
    Dirt,
    Stone,
    StoneBrick,
    CrackedStoneBrick
}

public class BlockList
{
    public static bool IsBreakable(BlockType b) => b switch
        {Grass or Apple or Pineapple or Sandbox or Dirt => true, _ => false };

    //instance data
    private readonly Dictionary<(int x, int y), BlockType> data = [];
    public void Clear() => data.Clear();
    public bool Add((int, int) Position, BlockType type) => data.TryAdd(Position, type);
    public IEnumerable<(int, int)> Positions() => data.Keys;
    public bool Contains((int, int) Position) => data.ContainsKey(Position);
    public BlockType TypeAt((int, int) Position) => data[Position];
    public bool Remove((int, int) Position) => data.Remove(Position);
    public BlockList Union(BlockList other) { foreach (var (pos, type) in other.data) { Add(pos, type); } ; return this;}

    public void Draw(SpriteBatch batch)
    {
        foreach (var ((x, y), type) in data)
        {
            batch.Draw(Assets.BlockSpriteSheet, new Vector2(x, y) * Consts.BlockWidth, SourceRects.BlockSourceRects[type], Color.White);
        }
    }

    public void Draw(SpriteBatch batch, Color color)
    {
        foreach (var ((x, y), type) in data)
        {
            batch.Draw(Assets.BlockSpriteSheet, new Vector2(x, y) * Consts.BlockWidth, SourceRects.BlockSourceRects[type], color);
        }
    }
}