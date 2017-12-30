//using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
//using UnityEngine;

//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;

[System.Serializable]
public class SerializedPlayerList : System.Object
{
    public PlayerStateSerializable[] list = new PlayerStateSerializable[PlayerGroup.kMaxPlayerCount];
}

[System.Serializable]
public class PlayerStateSerializable : System.Object
{
    public int index = -1;
    public int id = -1;
    public string name;

    static public PlayerStateSerializable Create(PlayerState state)
    {
        var serial = new PlayerStateSerializable();
        
        serial.index = state.index;
        serial.name = state.name;
        serial.id = state.id;
        return serial;
    }
    
}

public class PlayerState
{
    public const int    kMaxCardsPerTurn = 1;
    public const int    kFirstHandSize = 5;
    public const int    kNonFirstHandSize = 4;

    public int          index               { get; private set; }
    public string       name                { get; private set; }
    public int          id                  { get; private set; }

    public int          partnerIndex        { get; private set; }

    //public int          score               { get; set; }
    public int          numCardsPlayed      { get { return cardsPlayedStack.Count; } }

    public PlayerHand   hand                { get; private set; }
    public Stack<CardData> cardsPlayedStack { get; private set; }
    

    public static PlayerState Create(int playerIndex, string name, int id = -1)
    {
        PlayerState player = new PlayerState();
        player.hand = PlayerHand.Create(kFirstHandSize);
        player.cardsPlayedStack = new Stack<CardData>();
        player.index = playerIndex;
        player.name = name;
        player.id = id;
        //player.score = 0;
        return player;
    }

    public static PlayerState Create(PlayerStateSerializable serialState)
    {
        return PlayerState.Create(serialState.index, serialState.name, serialState.id);
    }
}
