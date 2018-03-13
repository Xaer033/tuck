using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        var result = new List<BoardPosition>();

        //Lots of work to do here!
        return result;   
    }
    
}
