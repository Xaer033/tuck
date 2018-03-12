using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeEscrow 
{
    private List<TradeRequest> _escrow = new List<TradeRequest>();
    private Dictionary<int, CardData> _assetStash = new Dictionary<int, CardData>(PlayerGroup.kMaxPlayerCount);
    private PlayerGroup _playerGroup;
    private TeamCollection _teamCollection;
    
    public static TradeEscrow Create(TeamCollection teamCollection, PlayerGroup playerGroup)
    {
        TradeEscrow command = new TradeEscrow();
        command._playerGroup = playerGroup;
        command._teamCollection = teamCollection;
        command.transactionCompleted = false;

        return command;
    }

    public bool transactionCompleted
    {
        get; private set;
    }

    public bool hasAllAssets
    {
        get { return !transactionCompleted && _escrow.Count == _playerGroup.playerCount; }
    }

    public bool HasAsset(TradeRequest request)
    {
        return _escrow.Contains(request);
    }

    public bool HasAssetFromPlayer(int playerIndex)
    {
        foreach(TradeRequest request in _escrow)
        {
            if(request.playerIndex == playerIndex)
            {
                return true;
            }
        }
        return false;
    }

    public void AddAsset(TradeRequest request)
    {
        PlayerState player = _playerGroup.GetPlayerByIndex(request.playerIndex);
        CardData card = player.hand.PopCard(request.handSlot);
        PlayerState partnerPlayer = _teamCollection.GetPartner(request.teamIndex, request.playerIndex);

        _assetStash[partnerPlayer.index] = card;
        _escrow.Add(request);
    }

    public void RemoveAsset(TradeRequest request)
    {
        PlayerState player = _playerGroup.GetPlayerByIndex(request.playerIndex);
        PlayerState partnerPlayer = _teamCollection.GetPartner(request.teamIndex, request.playerIndex);
        CardData card = _assetStash[partnerPlayer.index];
        player.hand.SetCard(request.handSlot, card);

        _assetStash.Remove(partnerPlayer.index);
        _escrow.Remove(request);
    }

    public void ApplyTrade()
    {
        foreach(TradeRequest request in _escrow)
        {
            _applyTrade(request);
        }
        transactionCompleted = true;
    }

    public void UndoTrade()
    {
        transactionCompleted = false;

        foreach(TradeRequest request in _escrow)
        {
            _undoTrade(request);
        }
    }
    
    private void _applyTrade(TradeRequest request)
    {
        PlayerState player = _playerGroup.GetPlayerByIndex(request.playerIndex);
        CardData card = _assetStash[request.playerIndex];
        player.hand.SetCard(request.handSlot, card);
    }

    private void _undoTrade(TradeRequest request)
    {
        PlayerState player = _playerGroup.GetPlayerByIndex(request.playerIndex);
        CardData card = player.hand.PopCard(request.handSlot);
        _assetStash[request.playerIndex] = card;
    }
}
