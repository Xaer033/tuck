using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PositionType
{
    MAIN_TRACK,
    HOME,
    START_PEG,
    GOAL_TRACK,
    GOAL_TRACK_ENTRANCE
}

[System.Serializable]
public struct BoardPosition
{
    public PositionType type { get; private set; }
    public int trackIndex { get; private set; }
    public int ownerIndex { get; private set; }

    //public BoardPosition()
    //{
    //    type = PositionType.HOME;
    //    trackIndex = 0;
    //    ownerIndex = -1;
    //}

    public override string ToString()
    {
        return "Type: " + type.ToString() + ", \nTrackIndex: " + trackIndex + "\nOwnerIndex:" + ownerIndex;
    }

    public static bool IsSame(BoardPosition a, BoardPosition b)
    {
        return  a.type == b.type && 
                a.trackIndex == b.trackIndex && 
                a.ownerIndex == b.ownerIndex;
    }

    public static BoardPosition Create(
        PositionType type, 
        int trackIndex, 
        int playerIndex = -1)
    {
        BoardPosition pos = new BoardPosition();
        pos.type = type;
        pos.trackIndex = trackIndex;
        pos.ownerIndex = playerIndex;
        return pos;
    }
}
