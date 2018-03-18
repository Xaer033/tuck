using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GhostGen;


public class PieceView : UIView
{
    public Image _icon;

    // Debugging things
    public PositionType _positionType;
    public int _trackIndex;
    public int _ownerIndex;
    // -----------
    
    private BoardPiece _piece;

    public BoardPosition boardPosition
    {
        get
        {
            return _piece.boardPosition;
        }
    }

    public BoardPiece piece
    {
        get
        {
            return _piece;
        }
        set
        {
            if(_piece != value)
            {
                _piece = value;
                invalidateFlag = InvalidationFlag.DYNAMIC_DATA;                        
            }
        }
    }
    protected override void OnViewUpdate()
    {
        base.OnViewUpdate();

        if(IsInvalid(InvalidationFlag.DYNAMIC_DATA))
        {
            if(_icon != null)
            {
                _icon.color = PlayerUtil.GetColor(piece.ownerIndex);
            }

            transform.localPosition = BoardPositionUtil.GetViewPosition(boardPosition);
            // DebugInfo
            _positionType = boardPosition.type;
            _trackIndex = boardPosition.trackIndex;
            _ownerIndex = piece.ownerIndex;
        }
    }
}
