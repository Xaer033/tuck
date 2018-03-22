namespace GameCommands
{
    public class AddTradeRequestCommand : ICommand
    {
        private TradeEscrow escrow;
        private TradeRequest request;

        public static AddTradeRequestCommand Create(TradeEscrow escrow, TradeRequest request)
        {
            AddTradeRequestCommand command = new AddTradeRequestCommand();
            command.escrow = escrow;
            command.request = request;

            return command;
        }

        public bool isLinked
        {
            get { return false; }
        }

        public void Execute()
        {
            escrow.AddAsset(request);
        }

        public void Undo()
        {
            escrow.RemoveAsset(request);
        }
    }
}
