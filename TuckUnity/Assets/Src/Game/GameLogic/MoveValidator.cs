using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class MoveValidator
{
    private Board _board;

    public static MoveValidator Create(Board board)
    {
        MoveValidator validator = new MoveValidator();
        validator._board = board;
        return validator;
    }

    List<BoardPosition> GetValidPositions(BoardPiece piece, CardData card)
    {
        Assert.IsNotNull(piece);
        Assert.IsNotNull(card);

        var result = new List<BoardPosition>();
        for(int i = 0; i < card.pieceMovementList.Length; ++i)
        {
            PieceMovementData moveData = card.pieceMovementList[i];
            string moveType = moveData.type;
            switch(moveType)
            {
                case MoveType.FORWARDS:
                    handleForwardMovement(piece, moveData, ref result);
                    break;
            }
        }
        //Lots of work to do here!
        return result;   
    }
    
    private void handleForwardMovement(
        BoardPiece piece, 
        PieceMovementData movementData, 
        ref List<BoardPosition> result)
    {   
        var boardPositions = _board.GetBoardPositionList();

        int start = piece.boardPosition.trackIndex;
        int end = BoardPositionUtil.GetWrappedMainTrackIndex(piece.boardPosition.trackIndex + movementData.value);
        BoardPosition startingPeg = _board.GetStartingPosition(piece.ownerIndex);

        int adjustedEnd;

        switch(piece.boardPosition.type)
        {
            case PositionType.HOME: return; // Can't move forward when piece is at home!
            case PositionType.GOAL_TRACK:
            case PositionType.GOAL_TRACK_ENTRANCE:

                break;
        }

    }
    
}
