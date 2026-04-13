using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;
using static Sprint2.Managers.BlockManager.Block.BlockType;

namespace Sprint2.Managers;

public class BlockManager {

    public record struct Block(Block.BlockType Type, Color Color)
    {
        public Block(BlockType type) : this(type, Color.White) {}
        
        public enum BlockType {
            //plants
            Grass,
            Apple,
            Pineapple,
            Sandbox,
            Cherry,
            Gravebind,
            Catalyst,
            //platforms
            Dirt,
            Stone,
            StoneBrick,
            CrackedStoneBrick,
            //void
            
            Void
        }
        
        public readonly bool IsBreakable => Type switch
            {Grass or Apple or Pineapple or Sandbox or Dirt => true, _ => false };
    }
    
    private readonly Dictionary<(int x, int y), Block> blocks = [];
    public void Reset() => blocks.Clear();
    public bool HasBlockAt((int, int) pos) => blocks.ContainsKey(pos);
    public Block BlockAt((int, int) pos) => blocks[pos];
    public void SetColor((int, int) pos, Color c) => blocks[pos] = new(BlockAt(pos).Type, c);
    public void Set((int, int) pos, Block.BlockType type) => blocks[pos] = new(type);
    public bool Add((int, int) pos, Block.BlockType type) => blocks.TryAdd(pos, new(type));
    public bool Add((int, int) pos, Block.BlockType type, Color c) => blocks.TryAdd(pos, new(type, c));
    public bool Remove((int, int) pos) => blocks.Remove(pos);
    
    public void Infect((int, int) pos) => Set(pos, Void);

    /// <summary>used when player tries to dig a block</summary>
    public Block? TryBreakAt((int, int) pos) {
        if (HasBlockAt(pos)) {
            Block b = BlockAt(pos);
            if (b.IsBreakable) {
                Remove(pos);
                return b;
            }
        }
        return null;
    }

    /// <summary> Used only for player digging </summary>
    public Block? TryDigBelow(Vector2 coords) {
        var (x, y) = Funcs.GridCoords(coords);
        y++; //want the cell *below* midpoint of bottom edge of player

        var output = TryBreakAt((x, y));
        if (output is null) {
            bool breakLeft = coords.X % Consts.BlockWidth < Consts.BlockWidth / 2f;
            return TryBreakAt((x + (breakLeft ? -1 : 1), y));
        }
        return output;
    }

    /// <summary>splat</summary>
    public void AddRectangleArray((Block.BlockType type, int x, int y, int w, int h) data) {
        AddRectangleArray(data.type, data.x, data.y, data.w, data.h);
    }

    public void AddRectangleArray(Block.BlockType type, int x, int y, int w, int h) {
        for (int j = 0; j < h; j++) {
            for (int i = 0; i < w; i++) {
                Add((x + i, y + j), type);
            }
        }
    }

    public void Draw(SpriteBatch batch)
    {
        foreach (var ((x, y), block) in blocks)
        {
            batch.Draw(Assets.BlockPlayerSpriteSheet, new Vector2(x, y) * Consts.BlockWidth, SourceRects.BlockSourceRects[block.Type], block.Color);
        }
    }
}