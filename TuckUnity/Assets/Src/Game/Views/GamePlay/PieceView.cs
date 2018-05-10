using GhostGen;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PieceView : UIView, IPointerClickHandler
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

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        DispatchEvent(GameEventType.PIECE_TAP, true, this);
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
