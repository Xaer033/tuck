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
        for (int i = 0; i < group.playerCount; ++i)
        {
            for (int j = 0; j < PlayerState.kFirstHandSize; ++j)
            {
                CardData cardData = cardDeck.Pop();
                PlayerState playerState = group.GetPlayerByIndex(i);
                playerState.hand.SetCard(j, cardData);
            }
        }

        return group;
    }

    private PlayerGroup() { }
    

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
