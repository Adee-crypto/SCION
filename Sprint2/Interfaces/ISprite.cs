using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Sprint2.Sprites.LinkSprite;

namespace Sprint2.Interfaces
{
    internal interface ISprite
    {
        public void setState(LinkAnimationState linkAnimationState);
        public void Update(GameTime gameTime);
        public void Draw();
    }
}
