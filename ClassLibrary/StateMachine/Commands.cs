namespace Automaton
{
    public sealed class KeyStates
    {
        public static Command[] Home = { Command.REGISTER_PATIENT, Command.START_EXAMS, Command.POWER_OFF };
        public static Command[] ExamsInProgress = { Command.ABORT_EXAMS, Command.UP, Command.DOWN };
    }

    public enum Command
    {
        POWER_OFF,
        REGISTER_PATIENT,
        START_EXAMS,
        UP,
        DOWN,
        ABORT_EXAMS,
    }
}
