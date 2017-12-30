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

    private BoardPosition _boardPosition;
    
    public BoardPosition boardPosition
    {
        get
        {
            return _boardPosition;
        }
        set
        {
            if(!BoardPosition.IsSame(_boardPosition, value))
            {
                _boardPosition = value;
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
                _icon.color = PlayerUtil.GetColor(_boardPosition.ownerIndex);
            }

            transform.localPosition = BoardPositionUtil.GetViewPosition(_boardPosition);
            // DebugInfo
            _positionType = _boardPosition.type;
            _trackIndex = _boardPosition.trackIndex;
            _ownerIndex = _boardPosition.ownerIndex;
        }
    }
}
