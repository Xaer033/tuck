using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTradeRequestCommand : ICommand
{
    public TradeEscrow   escrow     { get; private set; }
    public TradeRequest  request    { get; private set; }
    
    public static AddTradeRequestCommand Create(TradeEscrow escrow, TradeRequest request)
    {
        AddTradeRequestCommand command = new AddTradeRequestCommand();
        command.escrow = escrow;
        command.request = request;

        return command;
    }

    public bool isLinked
    {
        get { return false; }
    }

    public void Execute()
    {
        escrow.AddRequest(request);
    }

    public void Undo()
    {
        escrow.RemoveRequest(request);
    }
}
