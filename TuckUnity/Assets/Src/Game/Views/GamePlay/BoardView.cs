﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GhostGen;

public class BoardView : UIView
{ 
    public Transform _pegsGroup;
    public Transform _piecesGroup;

    private Board _board;
    private List<PegView> _pegList = new List<PegView>();
    private List<PieceView> _pieceList = new List<PieceView>();

    public void SetBoard(Board board)
    {
        _board = board;

        List<BoardPosition> boardPosList = _board.GetBoardPositionList();
        foreach (BoardPosition position in boardPosList)
        {
            PegView pegView = Singleton.instance.cardResourceBank.CreatePegView(position, _pegsGroup);
            _pegList.Add(pegView);
        }

        foreach(BoardPieceGroup group in _board.pieceGroupList)
        {
            foreach (BoardPiece piece in group.pieceList)
            {
                PieceView pieceView = Singleton.instance.cardResourceBank.CreatePieceView(piece.boardPosition, _piecesGroup);
                _pieceList.Add(pieceView);
            }
        }
        invalidateFlag = InvalidationFlag.STATIC_DATA;
    }
    

    protected override void OnViewUpdate()
    {
        base.OnViewUpdate();

        if(IsInvalid(InvalidationFlag.STATIC_DATA) && _board != null)
        {
           
        }
    }

}
