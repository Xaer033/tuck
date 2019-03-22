using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class MoveValidator
{
    private Board _board;
    private Stack<int> _invalidPathStack = new Stack<int>(2);

    public static MoveValidator Create(Board board)
    {
        MoveValidator validator = new MoveValidator()
        {
            _board = board
        };
        return validator;
    }

    public bool GetPieceHitList(MovePath path, bool isSplitStomp, ref List<BoardPiece> hitList)
    {
        BoardPosition pos;
        BoardPiece victim;

        // Eat everything in the path V--V
        if(isSplitStomp)
        {
            path.Reset();
            while(path.GetNext(out pos))
            {
                if(_board.IsPositionOccupied(pos, out victim))
                {                    
                    hitList.Add(victim);
                }
            }
        }
        else // Only eat last victim in path V--V
        {
            pos = path.end;
            if(_board.IsPositionOccupied(pos, out victim))
            {
                hitList.Add(victim);
            }
        }
        return hitList.Count > 0;
    }

    public bool GetValidPaths(BoardPiece piece, CardData card, ref List<MovePath> result)
    {
        Assert.IsNotNull(piece);
        Assert.IsNotNull(card);
        Assert.IsNotNull(result);

        for(int i = 0; i < card.pieceMovementList.Length; ++i)
        {
            PieceMovementData moveData = card.pieceMovementList[i];

            // TODO: Convert everything to use State instead creating it on the fly here. TEMP
            PieceMovementState state = PieceMovementState.Create(moveData);
            GetValidPaths(piece, state, ref result);
        }
        return result.Count > 0;   
    }

    public bool GetValidPaths(BoardPiece piece, PieceMovementState moveState, ref List<MovePath> result)
    {
        Assert.IsNotNull(piece);
        Assert.IsNotNull(moveState);
        Assert.IsNotNull(result);

        bool hasPath = false;
        PieceMovementData moveData = moveState.data;
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

            case MoveType.SWAP:
                hasPath = handleSwapMovement(piece, moveData, ref result);
                break;

            case MoveType.SPLIT_STOMP:
                hasPath = handleSplitStompMovement(piece, moveData, ref result);
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

        BoardPosition homePosition = piece.boardPosition;
        BoardPosition startingPeg = _board.GetStartingPosition(ownerIndex);

        BoardPiece blockingPiece;
        if(_board.IsPositionOccupied(startingPeg, out blockingPiece))
        {
            startingPegValid = !blockingPiece.justLeftHome;
        }

        if(startingPegValid)
        {
            MovePath path = new MovePath();
            path.Add(homePosition);
            path.Add(startingPeg);
            result.Add(path);
        }

        return result.Count > 0;
    }

    private bool handleSplitStompMovement(
        BoardPiece piece,
        PieceMovementData movementData,
        ref List<MovePath> result)
    {
        bool hasPath = false;

        return hasPath;
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

    private bool handleSwapMovement(
        BoardPiece piece,
        PieceMovementData movementData,
        ref List<MovePath> result)
    {
        bool hasPath = false;

        List<BoardPieceGroup> groupList = _board.GetPieceGroupList();
        for(int i = 0; i < groupList.Count; ++i)
        {
            BoardPieceGroup group = groupList[i];
            for(int j = 0; j < group.pieceList.Count; ++j)
            {
                BoardPiece groupPiece = group.pieceList[j];
                if(!groupPiece.isSafe && groupPiece.ownerIndex != piece.ownerIndex)
                {
                    MovePath path = new MovePath();
                    path.Add(groupPiece.boardPosition);
                    result.Add(path);
                    hasPath = true;
                }
            }
        }

        return hasPath;
    }

    private bool recurseGetPath(
        int count, 
        int pathIndex, 
        int playerIndex, 
        BoardPosition currentPosition, 
        bool forward, 
        ref List<MovePath> result)
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

        recurseGetPath(distance, 0, piece.ownerIndex, piece.boardPosition, forward, ref result);

        // Post process
        while(_invalidPathStack.Count > 0)
        {
            int invalidPath = _invalidPathStack.Pop();
            result.RemoveAt(invalidPath);
        }

        int resultCount = result.Count;
        //for(int i = 0; i < resultCount; ++i)
        //{
        //    result[resultCount - 1].RemoveAt(0);
        //}
        return resultCount > 0;
    }

}
