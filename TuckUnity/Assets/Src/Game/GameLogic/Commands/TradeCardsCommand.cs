namespace GameCommands
{
    public class TradeCardsCommand : ICommand
    {
        private TradeEscrow escrow;

        public static TradeCardsCommand Create(TradeEscrow escrow)
        {
            TradeCardsCommand command = new TradeCardsCommand();
            command.escrow = escrow;
            return command;
        }

        public bool isLinked
        {
            get { return false;  }
        }

        public void Execute()
        {
            escrow.ApplyTrade();
        }

        public void Undo()
        {
            escrow.UndoTrade();
        }
    }
}
