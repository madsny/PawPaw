using System;

namespace PawPaw.Cmd
{

    public class Choice
    {
        public Choice(string description, Func<string> func)
        {
            Description = description;
            Func = func;
        }

        public string Description { get; set; }
        public Func<string> Func { get; set; }
    }
}
