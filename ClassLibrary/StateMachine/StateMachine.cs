using System;
using System.Linq;

namespace Automaton
{
    public class CommandsChangedArgs
    {
        public Command[] Commands { get; }
        public CommandsChangedArgs(Command[] commands) { Commands = commands; }
    }

    public class StateMachine
    {
        private Command[] _availableCommand = new Command[0];
        private Command[] AvailableCommands
        {
            get { return _availableCommand; }
            set
            {
                _availableCommand = value;
                CommandsChangedEvent?.Invoke(this, new CommandsChangedArgs(value));
            }
        }

        public delegate void CommandsChangedEventHandler(object sender, CommandsChangedArgs e);

        public event CommandsChangedEventHandler CommandsChangedEvent;

        public StateMachine AddAvailableCommands(params Command[] commands)
        {
            AvailableCommands = AvailableCommands.Concat(commands).Distinct().ToArray();
            return this;
        }
        public StateMachine RemoveAvailableCommands(params Command[] commands)
        {
            AvailableCommands = AvailableCommands.Except(commands).ToArray();
            return this;
        }
        public StateMachine ResetAvailableCommands(params Command[] commands)
        {
            AvailableCommands = commands;
            return this;
        }

        public Command[] GetAvailableCommands()
         => AvailableCommands;

        public bool Can(Command command)
            => AvailableCommands.Contains(command);
    }
}
