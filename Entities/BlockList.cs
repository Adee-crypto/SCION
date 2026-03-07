using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;
using System.Collections.Generic;

namespace Sprint2.Entities;

public enum BlockType
{
    //plants
    Grass,
    Apple,
    Pineapple,
    //platforms
    Stone,
    StoneBrick,
    CrackedStoneBrick
}

public class BlockList
{
    private readonly Dictionary<(int x, int y), BlockType> data = [];

    public void Clear() => data.Clear();
    public bool Add((int, int) Position, BlockType type) => data.TryAdd(Position, type);
    public IEnumerable<(int, int)> Positions() => data.Keys;
    public bool Contains((int, int) Position) => data.ContainsKey(Position);
    public bool Remove((int, int) Position) => data.Remove(Position);
    public void Union(BlockList other) { foreach (var (pos, type) in other.data) { Add(pos, type); } }

    public IEnumerable<Rectangle> ColliderBounds()
    {
        foreach (var (x, y) in data.Keys)
        {
            yield return new(x * Consts.BlockWidth, y * Consts.BlockWidth, Consts.BlockWidth, Consts.BlockWidth);
        }
    }

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