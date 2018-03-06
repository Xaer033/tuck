using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeCardsCommand : ICommand
{
    public TradeEscrow escrow { get; set; }

    public static TradeCardsCommand Create(TradeEscrow escrow)
    {
        TradeCardsCommand command = new TradeCardsCommand();
        command.escrow = escrow;
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
        escrow.ApplyTrade();
    }

    public void Undo()
    {
        escrow.UndoTrade();
    }
}
