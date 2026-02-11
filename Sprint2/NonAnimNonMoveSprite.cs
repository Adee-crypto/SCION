using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
class NonAnimNonMoveSprite(Texture2D spriteSheet, Rectangle data, Vector2 position) : ISprite {
    public Texture2D texture = spriteSheet;
    private Rectangle textureData = data;
    private Vector2 pos = position;

    //Add sprite to batch
    public void Draw(SpriteBatch batch)
    {
        //Draw centered on pos
        batch.Draw(texture, pos - new Vector2(textureData.Width, textureData.Height)/2, textureData, Color.White);
    }

    //Calculate new frame based on time
    public void Update(GameTime gameTime) {}
}