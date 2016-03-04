using System;
using System.Threading;
using PawPaw.Core;
using PawPaw.ElasticSearch;

namespace PawPaw.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new PostWritingEngine(new CommandHandler());
            var thread = new Thread(() => engine.Run());
            thread.Start();
            var userProvider = new CmdUserProvider();
            var choiceMaker = new ChoiceMaker(new AdminService(), new PostReader(), new PostWritingService(userProvider, engine), userProvider);
            choiceMaker.RunRunRun();

            Console.WriteLine("Thank you, I'm content");
            engine.Stop();
            thread.Join();
            Thread.Sleep(1000);
        }
    }
}
