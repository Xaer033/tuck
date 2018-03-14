using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardPiece
{
    public bool justLeftHome;
    public BoardPosition boardPosition;

    public int ownerIndex { get; private set; }
    public Board board { get; private set; }


    public static BoardPiece Create(Board board, BoardPosition initialPos, int ownerIndex)
    {
        BoardPiece piece = new BoardPiece();
        piece.board = board;
        piece.boardPosition = initialPos;
        piece.ownerIndex = ownerIndex;
        return piece;
    }

    public bool isSafe
    {
        get
        {
            return justLeftHome
                || boardPosition.type == PositionType.GOAL_TRACK
                || boardPosition.type == PositionType.HOME;
        }
    }
	
}
