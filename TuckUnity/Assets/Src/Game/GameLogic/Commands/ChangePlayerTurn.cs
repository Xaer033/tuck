namespace GameCommands
{
    public class ChangePlayerTurn : ICommand
    {
        private int oldPlayerIndex;
        private PlayerGroup group;

        public static ChangePlayerTurn Create(PlayerGroup group)
        {
            ChangePlayerTurn command = new ChangePlayerTurn();
            command.group = group;
            return command;
        }

        public bool isLinked
        {
            get { return false; }
        }

        public void Execute()
        {
            oldPlayerIndex = group.activePlayer.index;
            group.SetNextActivePlayer();
        }

        public void Undo()
        {
            group.SetActivePlayer(oldPlayerIndex);
        }
    }
}