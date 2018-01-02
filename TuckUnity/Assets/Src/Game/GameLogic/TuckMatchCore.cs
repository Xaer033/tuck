using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public void ApplyTrade(List<TradeRequest> tradeList)
    {
        ICommand command = TradeCards.Create(
            tradeList,
            matchState.playerGroup,
            matchState.teams);

        _commandFactory.Execute(command);
    }
}
