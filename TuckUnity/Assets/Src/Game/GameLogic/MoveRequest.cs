using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRequest
{
    public int playerIndex      { get; private set; }
    public int pieceIndex       { get; private set; }
    public int handIndex        { get; private set; }

    public PieceMovementData movement { get; private set; }


    public static MoveRequest Create(int playerIndex, int handIndex, PieceMovementData movementData)
    {
        MoveRequest request = new MoveRequest();
        request.playerIndex = playerIndex;
        request.handIndex = handIndex;
        request.movement = movementData;
        return request;
    }
}
