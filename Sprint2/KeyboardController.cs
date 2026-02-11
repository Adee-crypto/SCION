using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

class KeyboardController : IController
{
    KeyboardState keyboardState;

    private Dictionary<Keys, int> CommandKeyBinds = new()
    {
        {Keys.D0, 0},
        {Keys.NumPad0, 0},
        {Keys.D1, 1},
        {Keys.NumPad1, 1},
        {Keys.D2, 2},
        {Keys.NumPad2, 2},
        {Keys.D3, 3},
        {Keys.NumPad3, 3},
        {Keys.D4, 4},
        {Keys.NumPad4, 4}
    };

    public void Update(ref int command)
    {
        keyboardState = Keyboard.GetState();
        foreach (KeyValuePair<Keys, int> pair in CommandKeyBinds)
        {
            if (keyboardState.IsKeyDown(pair.Key)) {
                command = pair.Value;
                break;
            }
        }
    }
}