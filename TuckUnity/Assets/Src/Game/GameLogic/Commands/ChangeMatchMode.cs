using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatchMode : ICommand
{
    private TuckMatchState matchState;
    private GameMatchMode oldMatchMode;
    private GameMatchMode newMatchMode;

    public static ChangeMatchMode Create(TuckMatchState matchState, GameMatchMode newMode)
    {
        ChangeMatchMode command = new ChangeMatchMode();
        command.matchState = matchState;
        command.newMatchMode = newMode;
        return command;
    }

    public bool isLinked
    {
        get { return true; }
    }

    public void Execute()
    {
        oldMatchMode = matchState.gameMatchMode;
        matchState.gameMatchMode = newMatchMode;
    }

    public void Undo()
    {
        matchState.gameMatchMode = oldMatchMode;
    }
}
