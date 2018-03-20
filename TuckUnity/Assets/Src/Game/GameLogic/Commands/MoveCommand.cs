using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    public Board        board       { get; private set; }
    public PlayerGroup  playerGroup { get; private set; }

    public static MoveCommand Create(Board board, PlayerGroup group)
    {
        MoveCommand command = new MoveCommand();
        command.board = board;
        command.playerGroup = group;
        return command;
    }

    public bool isLinked
    {
        get { return false; }
    }

    public void Execute()
    {

    }

    public void Undo()
    {

    }
}
