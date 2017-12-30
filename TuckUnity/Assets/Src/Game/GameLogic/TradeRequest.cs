using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeRequest
{
    public int playerIndex { get; private set; }
    public int partnerIndex { get; private set; }
    public int handSlotIndex { get; private set; }

	public static TradeRequest Create(int playerIndex, int partnerIndex, int handSlotIndex)
    {
        TradeRequest trade = new TradeRequest();
        trade.playerIndex = playerIndex;
        trade.partnerIndex = partnerIndex;
        trade.handSlotIndex = handSlotIndex;
        return trade;
    }
}
