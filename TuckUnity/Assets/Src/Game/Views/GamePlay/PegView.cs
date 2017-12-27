﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using GhostGen;


public class PegView : UIView
{
    public Image _icon;
    // Debugging things
    public PositionType _positionType;
    public int _trackIndex;
    public int _ownerIndex;
    public Vector3 _worldPosition;
    // -----------

    private BoardPosition _boardPosition;

    public event Action WantRepaint;

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
                _icon.color = _getColor(_boardPosition.type);
            }

            _positionType = _boardPosition.type;
            _trackIndex = _boardPosition.trackIndex;
            _ownerIndex = _boardPosition.ownerIndex;
            _worldPosition = BoardPositionUtil.GetWorldPosition(_boardPosition);
            
            if(WantRepaint != null)
            {
                WantRepaint();
            }
        }
    }


    private Color _getColor(PositionType type)
    {
        switch (type)
        {
            case PositionType.GOAL_TRACK: return Color.blue;
            case PositionType.HOME: return Color.red;
            case PositionType.START_PEG: return Color.yellow;
            case PositionType.MAIN_TRACK: return Color.grey;
            case PositionType.GOAL_TRACK_ENTRANCE: return Color.green;
        }

        Debug.LogError("Not handled top: " + type);
        return Color.black;

    }
}