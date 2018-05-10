using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardPiece
{
    public bool justLeftHome;
    public BoardPosition boardPosition;

    public int ownerIndex { get; private set; }
    public int index { get; private set; }
    public Board board { get; private set; }

    public static BoardPiece Create(Board board, BoardPosition initialPos, int ownerIndex, int pieceIndex)
    {
        BoardPiece piece = new BoardPiece();
        piece.board = board;
        piece.ownerIndex = ownerIndex;
        piece.index = pieceIndex;
        board.SetPiecePosition(piece, initialPos);
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
