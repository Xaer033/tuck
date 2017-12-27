using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardPiece
{
    public BoardPosition boardPosition;
    public bool justLeftHome;

    public int ownerIndex { get; private set; }
    public Board board { get; private set; }


    public static BoardPiece Create(BoardPosition initialPos, int ownerIndex)
    {
        BoardPiece piece = new BoardPiece();
        piece.boardPosition = initialPos;
        piece.ownerIndex = ownerIndex;
        return piece;
    }

    public bool isSafe
    {
        get
        {
            return !justLeftHome;// || boardPosition
        }
    }
	
}
