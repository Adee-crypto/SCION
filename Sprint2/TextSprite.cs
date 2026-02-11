using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
class TextSprite(SpriteFont font, string text, Vector2 position) : ISprite {
    private string text = text;
    private Vector2 pos = position;
    private SpriteFont font = font;

    //Add sprite to batch
    public void Draw(SpriteBatch batch)
    {
        //Draw centered on pos
        batch.DrawString(font, text, pos, Color.White);
    }

    //Calculate new frame based on time
    public void Update(GameTime gameTime) {}
}