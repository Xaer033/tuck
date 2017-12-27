using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class BoardPieceGroup
{
    public const int kPiecesPerPlayer = 4;

    private BoardPiece[] _pieceList = new BoardPiece[kPiecesPerPlayer];

    public static BoardPieceGroup Create(int ownerIndex)
    {
        BoardPieceGroup group = new BoardPieceGroup();
        for(int i = 0; i < kPiecesPerPlayer; ++i)
        {
            BoardPosition initialPosition = BoardPosition.Create(PositionType.HOME, i, ownerIndex);
           group._pieceList[i] = BoardPiece.Create(initialPosition, ownerIndex);
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
