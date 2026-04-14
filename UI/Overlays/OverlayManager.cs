using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;

namespace Sprint2.UI.Overlays;

public class OverlayManager
{
    private readonly Stack<IOverlay> overlays = new();
    public bool HasOverlays => overlays.Count > 0;

    public void Push(IOverlay overlay)
    {
        overlays.Push(overlay);
        overlay.OnOpen();
    }

    public void Pop()
    {
        if (overlays.Count > 0)
        {
            IOverlay top = overlays.Pop();
            top.OnClose();
        }
    }

    public void Clear()
    {
        while (overlays.Count > 0)
        {
            IOverlay top = overlays.Pop();
            top.OnClose();
        }
    }

    // public void Resize((int w, int h) size)
    // {
    //     foreach (IOverlay overlay in overlays)
    //     {
    //         if (overlay is IOverlay resizable) resizable.Resize(size);
    //     }
    // }

    public IOverlay Peek()
    {
        return overlays.Count > 0 ? overlays.Peek() : null;
    }

    public void Update(GameTime gametime)
    {
        Peek()?.Update(gametime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (overlays.Count == 0) return;

        overlays.Peek().Draw(spriteBatch);
    }
}