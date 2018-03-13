using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class PlayerGroup
{
    public const int kMaxPlayerCount = 4;
    
    private int _activePlayerIndex = 0;
    
    public List<PlayerState> playerList { get; private set; }

    public static PlayerGroup Create(List<PlayerState> playerList, CardDeck cardDeck)
    {
        PlayerGroup group = new PlayerGroup();
        group.playerList = playerList;

        group.DealNewHand(0, PlayerHand.kFirstHandSize, cardDeck);


        return group;
    }

    private PlayerGroup() { }
    
    public void DealNewHand(int dealingPlayerIndex, int handSize, CardDeck deck)
    {
        for(int handIndex = 0; handIndex < handSize; ++handIndex)
        {
            for(int playerIndex = 0; playerIndex < playerCount; ++playerIndex)
            {
                if(deck.isEmpty)
                {
                    Debug.LogError("Ran out of cards while trying to deal hand!");
                    return;
                }

                PlayerState player = GetPlayerByIndex(playerIndex);
                CardData cardData = deck.Pop();
                player.hand.SetCard(handIndex, cardData);
            }
        }
    }

    public PlayerState activePlayer
    {
        get { return playerList[_activePlayerIndex]; }
    }

    public int playerCount
    {
        get { return playerList.Count; }
    }

    public void SetActivePlayer(int playerIndex)
    {
        _boundsAssert(playerIndex);
        _activePlayerIndex = playerIndex;
    }

    public PlayerState GetPlayerByIndex(int playerIndex)
    {
        _boundsAssert(playerIndex);
        return playerList[playerIndex];
    }

    public PlayerState GetPlayerById(int playerId)
    {
        int count = playerCount;
        for (int i = 0; i < count; ++i)
        {
            if(playerList[i].id == playerId)
            {
                return playerList[i];
            }
        }

        Debug.LogError("Player id not found: " + playerId);
        return null;
    }
    
    public void SetNextActivePlayer()
    {
        int newIndex = (_activePlayerIndex + 1) % playerCount;
        SetActivePlayer(newIndex);
    }

    public PlayerState GetNextPlayer()
    {
        int newIndex = (_activePlayerIndex + 1) % playerCount;
        return GetPlayerByIndex(newIndex);
    }

    private void _boundsAssert(int index)
    {
        Assert.IsTrue(index >= 0);
        Assert.IsTrue(index < playerList.Count);
    }
}
