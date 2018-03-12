using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GhostGen;

public class GameHudView : UIView 
{
    public Button _undoButton;
    public Button _redoButton;
    public Button _finishTurnButton;

	// Use this for initialization
	void Awake () 
	{
        _undoButton.onClick.AddListener(onUndoClicked);
        _redoButton.onClick.AddListener(onRedoClicked);
        _finishTurnButton.onClick.AddListener(onFinishClicked);
    }

    public override void OnViewDispose()
    {

        _undoButton.onClick.RemoveListener(onUndoClicked);
        _redoButton.onClick.RemoveListener(onRedoClicked);
        _finishTurnButton.onClick.RemoveListener(onFinishClicked);
    }

    private void onUndoClicked()
    {
        DispatchEvent(GameEventType.UNDO);
    }

    private void onRedoClicked()
    {
        DispatchEvent(GameEventType.REDO);
    }

    private void onFinishClicked()
    {
        DispatchEvent(GameEventType.FINISH_TURN);
    }

}
