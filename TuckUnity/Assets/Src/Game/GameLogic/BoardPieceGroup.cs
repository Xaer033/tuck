using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class BoardPieceGroup
{
    public const int kPiecesPerPlayer = 4;

    private List<BoardPiece> _pieceList = new List<BoardPiece>(kPiecesPerPlayer);

    public List<BoardPiece> pieceList
    {
        get { return _pieceList; }
    }

    public Board board { get; private set; }

    public static BoardPieceGroup Create(Board board, int ownerIndex)
    {
        BoardPieceGroup group = new BoardPieceGroup();
        group.board = board;
        for(int i = 0; i < kPiecesPerPlayer; ++i)
        {
            BoardPosition initialPosition = BoardPosition.Create(PositionType.HOME, i, ownerIndex);
            BoardPiece piece = BoardPiece.Create(board, initialPosition, ownerIndex);
            group._pieceList.Add(piece);
        }
        return group;
    }

    public BoardPiece GetPiece(int pieceIndex)
    {
        if(pieceIndex < 0 || pieceIndex >= kPiecesPerPlayer)
        {
            Debug.LogError("Index: " + pieceIndex + " is out of range!");
            return null;
        }

        return _pieceList[pieceIndex];
    }

}
