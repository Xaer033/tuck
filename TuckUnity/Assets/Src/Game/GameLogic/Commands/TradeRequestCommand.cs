using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeRequestCommand : ICommand
{
    public List<TradeRequest>   tradeList   { get; private set; }
    public TradeRequest         request     { get; private set; }
    
    public static TradeRequestCommand Create(List<TradeRequest> tradeList, TradeRequest request)
    {
        TradeRequestCommand command = new TradeRequestCommand();
        command.tradeList = tradeList;
        command.request = request;

        return command;
    }

    public bool isLinked
    {
        get { return false; }
    }

    public void Execute()
    {
        tradeList.Add(request);
    }

    public void Undo()
    {
        tradeList.Remove(request);
    }
}
