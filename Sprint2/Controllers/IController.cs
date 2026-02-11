namespace Sprint2.Controllers;

interface IController
{
    //Updates the input state and command number
    void Update(ref int command);
}