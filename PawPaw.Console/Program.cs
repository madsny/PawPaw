using System;
using System.Collections.Generic;
using System.Threading;
using Nest;
using PawPaw.ElasticSearch;
using PawPaw.ElasticSearch.Models;

namespace PawPaw.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var indexer = new Indexer();
            
            var choiceMaker = new ChoiceMaker(indexer);
            choiceMaker.RunRunRun();

            Console.WriteLine("Thank you, I'm content");
            Thread.Sleep(1000);
        }
    }
}
