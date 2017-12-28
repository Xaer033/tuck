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
    public Vector3 _worldPosition;
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
                _icon.color = _getColor(_boardPosition.ownerIndex);
            }

            _positionType = _boardPosition.type;
            _trackIndex = _boardPosition.trackIndex;
            _ownerIndex = _boardPosition.ownerIndex;
            _worldPosition = BoardPositionUtil.GetWorldPosition(_boardPosition);
            transform.localPosition = _worldPosition;
        }
    }


    private Color _getColor(int ownerIndex)
    {
        switch (ownerIndex)
        {
            case 0: return Color.green;
            case 1: return new Color(1.0f, 0.75f, 0.26f);
            case 2: return new Color(1.0f, 0.1f, 0.1f);
            case 3: return new Color(0.3f, 0.1f, 1.0f);
        }

        Debug.LogError("Not handled ownerIndex: " + ownerIndex);
        return Color.black;

    }
}
