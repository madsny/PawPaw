namespace PawPaw.Core.Commands
{
    interface ICommand
    {
        void Accept(ICommandHandler commandHandler);
    }
}
