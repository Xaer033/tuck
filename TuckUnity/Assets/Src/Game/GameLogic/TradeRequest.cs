using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TradeRequest
{
    public int playerIndex { get; private set; }
    public int teamIndex { get; private set; }
    public int handSlot { get; private set; }

    public static TradeRequest Create(int playerIndex, int teamIndex, int handSlot)
    {
        TradeRequest trade = new TradeRequest();
        trade.playerIndex = playerIndex;
        trade.teamIndex = teamIndex;
        trade.handSlot = handSlot;
        return trade;
    }
}
