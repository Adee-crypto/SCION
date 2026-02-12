namespace Sprint2.Interfaces
{
    public interface ICommand
    {
        public void Execute(int index);
        public void Unexecute();
    }
}
