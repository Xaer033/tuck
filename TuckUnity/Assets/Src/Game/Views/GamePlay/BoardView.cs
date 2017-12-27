using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GhostGen;

public class BoardView : UIView
{

    public Transform _pegsGroup;

    private Board _board;
    private List<PegView> _pegList = new List<PegView>();

	// Use this for initialization
	void Start ()
    {
		
	}


    public Board board
    {
        set
        {
            if(_board != value)
            {
                _board = value;
                invalidateFlag = InvalidationFlag.STATIC_DATA;
            }
        }
    }

    protected override void OnViewUpdate()
    {
        base.OnViewUpdate();

        if(IsInvalid(InvalidationFlag.STATIC_DATA) && _board != null)
        {
            // Setup board
            List<BoardPosition> boardPosList = _board.GetBoardPositionList();
            foreach(BoardPosition position in boardPosList)
            {
                PegView v = Singleton.instance.cardResourceBank.CreatePegView(position, _pegsGroup);
                _pegList.Add(v);
            }
        }
    }

}
