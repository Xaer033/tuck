using System.Collections.Generic;
using GameCommands;

public class TuckMatchCore 
{
    public TuckMatchState matchState { get; private set; }


    private CommandFactory _commandFactory = new CommandFactory();
    
    public static TuckMatchCore Create(
        List<PlayerState> playerList, 
        CardDeck cardDeck)
    { 
        TuckMatchCore core = new TuckMatchCore();

        // Also no commands for starting player hands
        core.matchState = TuckMatchState.Create(playerList, cardDeck);
        return core;
    }

    private TuckMatchCore() { }

    public void ClearCommands()
    {
        _commandFactory.Clear();
    }

    public void AddTradeRequest(TradeRequest request)
    {
        ICommand command = AddTradeRequestCommand.Create(matchState.escrow, request);
        _commandFactory.Execute(command);
    }

    public void ApplyTrade()
    {
        ICommand command = TradeCardsCommand.Create(matchState.escrow);
        _commandFactory.Execute(command);
    }

    public void ApplyNextPlayerTurn()
    {
        ICommand command = ChangePlayerTurn.Create(matchState.playerGroup);
        _commandFactory.Execute(command);
    }

    public void ApplyChangeMatchMode(GameMatchMode newMode)
    {
        ICommand command = ChangeMatchMode.Create(matchState, newMode);
        _commandFactory.Execute(command);
    }

    public bool Undo()
    {
        return _commandFactory.Undo();
    }

    public bool Redo()
    {
        return _commandFactory.Redo();
    }
}
