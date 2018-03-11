using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GhostGen;

public class GameHudView : UIView 
{
    public Button _undoButton;
    public Button _redoButton;

	// Use this for initialization
	void Awake () 
	{
        _undoButton.onClick.AddListener(onUndoClicked);
        _redoButton.onClick.AddListener(onRedoClicked);
	}

    public override void OnViewDispose()
    {

        _undoButton.onClick.RemoveListener(onUndoClicked);
        _redoButton.onClick.RemoveListener(onRedoClicked);
    }

    private void onUndoClicked()
    {
        DispatchEvent(GameEventType.UNDO);
    }

    private void onRedoClicked()
    {
        DispatchEvent(GameEventType.REDO);
    }

}
