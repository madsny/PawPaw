using System.Collections.Concurrent;
using System.Threading;
using PawPaw.Core.Commands;

namespace PawPaw.Core
{
    public class PostWritingEngine
    {
        private readonly ICommandHandler _commandHandler;
        private static readonly ConcurrentQueue<ICommand> CommandQueue = new ConcurrentQueue<ICommand>();
        private volatile bool _run;

        public PostWritingEngine(ICommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        internal void Enqueue(ICommand command)
        {   
            CommandQueue.Enqueue(command);
        }

        public void Run()
        {
            _run = true;
            while (_run)
            {
                ProcessQueue();
                Thread.Sleep(1000);
            }
        }

        private void ProcessQueue()
        {
            while (!CommandQueue.IsEmpty)
            {
                ICommand command;
                if (CommandQueue.TryDequeue(out command))
                {
                    command.Accept(_commandHandler);
                }
            }
            _commandHandler.Commit();
        }

        public void Stop()
        {
            _run = false;
        }
    }
}
