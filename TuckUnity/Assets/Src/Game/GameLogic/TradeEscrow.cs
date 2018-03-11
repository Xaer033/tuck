using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeEscrow 
{
    private List<TradeRequest>[] _sortedEscrow = new List<TradeRequest>[TeamCollection.kTeamCount];
    private List<TradeRequest> _escrow = new List<TradeRequest>();
    private PlayerGroup _playerGroup;
    private Dictionary<int, CardData> _assetStash = new Dictionary<int, CardData>(PlayerGroup.kMaxPlayerCount);
    
    public static TradeEscrow Create(TeamCollection teamCollection, PlayerGroup playerGroup)
    {
        TradeEscrow command = new TradeEscrow();
        command._playerGroup = playerGroup;

        for(int i = 0; i < teamCollection.teams.Count; ++i)
        {
            command._sortedEscrow[i] = new List<TradeRequest>();
        }

        return command;
    }

    public bool hasAllAssets
    {
        get { return _escrow.Count == _playerGroup.playerCount; }
    }

    public bool HasAsset(TradeRequest request)
    {
        return _escrow.Contains(request);
    }

    public void AddRequest(TradeRequest request)
    {
        //PlayerState player = _playerGroup.GetPlayerByIndex(request.playerIndex);
        //CardData card = player.hand.PopCard(request.handSlot);
        //_assetStash[player.index] = card;

        _escrow.Add(request);
    }
    public void RemoveRequest(TradeRequest request)
    {
        _escrow.Remove(request);
    }

    public void ApplyTrade()
    {
        foreach (TradeRequest request in _escrow)
        {
            _sortedEscrow[request.teamIndex].Add(request);
        }

        for (int i = 0; i < _sortedEscrow.Length; ++i)
        {
            TradeRequest req1 = _sortedEscrow[i][0];
            TradeRequest req2 = _sortedEscrow[i][1];
            _swap(req1, req2);
        }
    }

    public void UndoTrade()
    {
        for (int i = 0; i < _sortedEscrow.Length; ++i)
        {
            TradeRequest req1 = _sortedEscrow[i][1];
            TradeRequest req2 = _sortedEscrow[i][0];
            _swap(req1, req2);
        }
    }

    private void _swap(TradeRequest req1, TradeRequest req2)
    {
        PlayerState p1 = _playerGroup.GetPlayerByIndex(req1.playerIndex);
        PlayerState p2 = _playerGroup.GetPlayerByIndex(req2.playerIndex);

        CardData c1 = p1.hand.GetCard(req1.handSlot);
        CardData c2 = p2.hand.GetCard(req2.handSlot);

        p1.hand.SetCard(req1.handSlot, c2);
        p2.hand.SetCard(req2.handSlot, c1);
    }

}
