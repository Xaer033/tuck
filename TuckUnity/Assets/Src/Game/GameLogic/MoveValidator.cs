using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MoveValidator
{
    private Board _board;
    private Stack<int> _invalidPathStack = new Stack<int>(2);

    public static MoveValidator Create(Board board)
    {
        MoveValidator validator = new MoveValidator();
        validator._board = board;
        return validator;
    }

    public List<MovePath> GetValidPositions(BoardPiece piece, CardData card)
    {
        Assert.IsNotNull(piece);
        Assert.IsNotNull(card);

        var result = new List<MovePath>();
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
        ref List<MovePath> result)
    {
        int forwardValue = movementData.value;

        switch(piece.boardPosition.type)
        {
            case PositionType.HOME: return; // Can't move forward when piece is at home!
            case PositionType.GOAL_TRACK:
            case PositionType.GOAL_TRACK_ENTRANCE:
            case PositionType.MAIN_TRACK:
                getPath(forwardValue, piece, true, ref result);
                break;
        }
    }

    private bool recurseGetPath(int count, int pathIndex, int playerIndex, BoardPosition currentPosition, bool forward, ref List<MovePath> result)
    {
        if(pathIndex >=0 || pathIndex < result.Count)
        {
            result[pathIndex].Add(currentPosition);
        }

        if(count > 0)
        {
            List<BoardPosition> nextPositions = new List<BoardPosition>(2);
            if(_board.GetNextPositions(currentPosition, playerIndex, forward, ref nextPositions))
            {
                if(nextPositions.Count == 0)
                {
                    return false;
                }

                if(nextPositions.Count > 1)
                {
                    result.Add(MovePath.Clone(result[result.Count - 1]));
                }

                for(int i= 0; i < nextPositions.Count; ++i)
                {
                    recurseGetPath(count - 1, pathIndex + i, playerIndex, nextPositions[i], forward, ref result);
                }
            }
            else
            {
                _invalidPathStack.Push(pathIndex);
            }
        }

        return true;
    }

    private bool getPath(int distance, BoardPiece piece, bool forward, ref List<MovePath> result)
    {
        _invalidPathStack.Clear();
        result.Add(new MovePath());

        recurseGetPath(distance, piece.ownerIndex, 0, piece.boardPosition, true, ref result);

        // Post process
        while(_invalidPathStack.Count > 0)
        {
            int invalidPath = _invalidPathStack.Pop();
            result.RemoveAt(invalidPath);
        }

        for(int i = 0; i < result.Count; ++i)
        {
            result[i].RemoveAt(0);
        }
        return true;
    }

}
