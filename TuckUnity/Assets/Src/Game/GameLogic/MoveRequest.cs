using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveRequest
{
    public int playerIndex      { get; private set; }
    public int handIndex        { get; private set; }

    public List<PiecePathData> piecePathList { get; private set; }

    public class PiecePathData
    {
        public int pieceIndex;
        public MovePath path;
    }

    public static MoveRequest Create(int playerIndex, int handIndex, List<PiecePathData> pieceMoveList)
    {
        MoveRequest request = new MoveRequest();
        request.playerIndex = playerIndex;
        request.handIndex = handIndex;
        request.piecePathList = pieceMoveList;

        return request;
    }
}
