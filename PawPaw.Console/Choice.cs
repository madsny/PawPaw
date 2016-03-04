using System;

namespace PawPaw.Cmd
{

    public class Choice
    {
        public Choice(string description, Action action)
        {
            Description = description;
            Action = action;
        }

        public string Description { get; set; }
        public Action Action { get; set; }
    }
}
