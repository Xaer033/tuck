using System;
using System.Collections.Generic;
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

    public bool GetValidPaths(BoardPiece piece, CardData card, ref List<MovePath> result)
    {
        Assert.IsNotNull(piece);
        Assert.IsNotNull(card);
        Assert.IsNotNull(result);

        for(int i = 0; i < card.pieceMovementList.Length; ++i)
        {
            PieceMovementData moveData = card.pieceMovementList[i];
            GetValidPaths(piece, moveData, ref result);
        }
        //Lots of work to do here!
        return result.Count > 0;   
    }

    public bool GetValidPaths(BoardPiece piece, PieceMovementData moveData, ref List<MovePath> result)
    {
        Assert.IsNotNull(piece);
        Assert.IsNotNull(moveData);
        Assert.IsNotNull(result);

        bool hasPath = false;

        string moveType = moveData.type;
        switch(moveType)
        {
            case MoveType.FORWARDS:
            case MoveType.BACKWARDS:
                hasPath = handleNormalMovement(piece, moveData, ref result);
                break;

            case MoveType.LEAVE_BASE:
                hasPath = handleHomeMovement(piece, moveData, ref result);
                break;
        }

        return hasPath;
    }

    private bool handleHomeMovement(
        BoardPiece piece,
        PieceMovementData movementData,
        ref List<MovePath> result)
    {
        if(piece.boardPosition.type != PositionType.HOME)
        {
            return false;
        }
        
        int ownerIndex = piece.ownerIndex;
        bool startingPegValid = true;

        BoardPosition startingPeg = _board.GetStartingPosition(ownerIndex);

        BoardPiece blockingPiece;
        if(_board.IsPositionOccupied(startingPeg, out blockingPiece))
        {
            startingPegValid = !blockingPiece.justLeftHome;
        }

        if(startingPegValid)
        {
            MovePath path = new MovePath();
            path.Add(startingPeg);
            result.Add(path);
        }

        return result.Count > 0;
    }

    private bool handleNormalMovement(
        BoardPiece piece, 
        PieceMovementData movementData, 
        ref List<MovePath> result)
    {
        bool hasPath = false;
        int movementDistance = movementData.value;
        
        if(piece.boardPosition.type == PositionType.HOME)
        {
            hasPath = false;
        }
        else
        {
            hasPath = getPathList(movementDistance, piece, movementData.type == MoveType.FORWARDS, ref result);
        }
        
        return hasPath;
    }

    private bool recurseGetPath(int count, int pathIndex, int playerIndex, BoardPosition currentPosition, bool forward, ref List<MovePath> result)
    {

        if(pathIndex >= 0 || pathIndex < result.Count)
        {
            result[pathIndex].Add(currentPosition);
        }

        int distance = Math.Abs(count);
        if(distance > 0)
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
                    recurseGetPath(distance - 1, pathIndex + i, playerIndex, nextPositions[i], forward, ref result);
                }
            }
            else
            {
                _invalidPathStack.Push(pathIndex);
            }
        }

        return true;
    }

    private bool getPathList(int distance, BoardPiece piece, bool forward, ref List<MovePath> result)
    {
        _invalidPathStack.Clear();
        result.Add(new MovePath());

        recurseGetPath(distance, piece.ownerIndex, 0, piece.boardPosition, forward, ref result);

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
        return result.Count > 0;
    }

}
