using GhostGen;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

public class BoardView : UIView
{
    public const float  kScale          = 300.0f;
    public const int    kPegStepSize    = 27;

    public Camera       _camera;
    public Transform[]  _viewList;
    public Transform    _pegsGroup;
    public Transform    _piecesGroup;

    private Board _board;
    private List<PegView>   _pegList = new List<PegView>();
    private List<PieceView> _pieceList = new List<PieceView>();
    private int _viewIndex = 0;
    private Sequence _cameraTween;

    public void SetBoard(Board board)
    {
        _board = board;

        List<BoardPosition> boardPosList = _board.GetBoardPositionList();
        foreach (BoardPosition position in boardPosList)
        {
            PegView pegView = Singleton.instance.cardResourceBank.CreatePegView(position, _pegsGroup);
            _pegList.Add(pegView);
        }

        List<BoardPieceGroup> pieceGroupList = _board.GetPieceGroupList();
        foreach(BoardPieceGroup group in pieceGroupList)
        {
            foreach (BoardPiece piece in group.pieceList)
            {
                PieceView pieceView = Singleton.instance.cardResourceBank.CreatePieceView(piece.boardPosition, _piecesGroup);
                _pieceList.Add(pieceView);
            }
        }
        invalidateFlag = InvalidationFlag.STATIC_DATA;
    }

    public int viewIndex
    {
        get { return _viewIndex; }
        set
        {
            if(_viewIndex != value)
            {
                _viewIndex = value;
                if(_cameraTween != null)
                {
                    _cameraTween.Kill();
                    _cameraTween = null;
                }
                invalidateFlag = InvalidationFlag.DYNAMIC_DATA;
            }
        }
    }
    

    protected override void OnViewUpdate()
    {
        base.OnViewUpdate();

        if(IsInvalid(InvalidationFlag.STATIC_DATA) && _board != null)
        {
           
        }

        if(IsInvalid(InvalidationFlag.DYNAMIC_DATA))
        {
            Transform viewTransform = getViewTransform(viewIndex);
            if(_camera && viewTransform && _cameraTween == null)
            {
                tweenCameraTo(viewTransform);
            }
        }
    }

    private Transform getViewTransform(int index)
    {
        Assert.IsTrue(index >= 0);
        Assert.IsTrue(index < _viewList.Length);
        Assert.IsNotNull<Transform>(_viewList[index]);
        return _viewList[index];      
    }

    private void tweenCameraTo(Transform target)
    {
        if(_camera && target)
        {
            Tween moveTween = _camera.transform.DOLocalMove(target.localPosition, 1.0f);
            Tween rotTween = _camera.transform.DOLocalRotate(target.localEulerAngles, 1.0f);
            _cameraTween = DOTween.Sequence();
            _cameraTween.Insert(0, moveTween);
            _cameraTween.Insert(0, rotTween);
            _cameraTween.Play();
        }
    }
}
