using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeCards : ICommand
{
    public List<TradeRequest> tradeList { get; private set; }
    public PlayerGroup playerGroup { get; private set; }
    public TeamCollection teamCollection { get; private set; }

    private List<TradeRequest>[] _teamTradeEscrow = new List<TradeRequest>[TeamCollection.kTeamCount];


    public static TradeCards Create(List<TradeRequest> tradeList, PlayerGroup group, TeamCollection teamCollection)
    {
        TradeCards command = new TradeCards();
        command.tradeList = tradeList;
        command.playerGroup = group;
        command.teamCollection = teamCollection;
        
        command._teamTradeEscrow[0] = new List<TradeRequest>();
        command._teamTradeEscrow[1] = new List<TradeRequest>();

        foreach (TradeRequest request in tradeList)
        {
            command._teamTradeEscrow[request.teamIndex].Add(request);
        }

        return command;
    }

    public bool isLinked
    {
        get
        {
            return false;
        }
    }

    public void Execute()
    {
        for(int i = 0; i < teamCollection.teams.Count; ++i )
        {
            TradeRequest req1 = _teamTradeEscrow[i][0];
            TradeRequest req2 = _teamTradeEscrow[i][1];
            _swap(req1, req2);
        }
    }

    public void Undo()
    {
        for (int i = 0; i < teamCollection.teams.Count; ++i)
        {
            TradeRequest req1 = _teamTradeEscrow[i][1];
            TradeRequest req2 = _teamTradeEscrow[i][0];
            _swap(req1, req2);
        }
    }

    private void _swap(TradeRequest req1, TradeRequest req2)
    {
        PlayerState p1 = playerGroup.GetPlayerByIndex(req1.playerIndex);
        PlayerState p2 = playerGroup.GetPlayerByIndex(req2.playerIndex);

        CardData c1 = p1.hand.GetCard(req1.handSlot);
        CardData c2 = p2.hand.GetCard(req2.handSlot);

        p1.hand.SetCard(req1.handSlot, c2);
        p2.hand.SetCard(req2.handSlot, c1);
    }
}
