using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GhostGen;

public class GameHudView : UIView 
{
    public Button _undoButton;

	// Use this for initialization
	void Awake () 
	{
        _undoButton.onClick.AddListener(onUndoClicked);
	}

    public override void OnViewDispose()
    {

        _undoButton.onClick.RemoveListener(onUndoClicked);
    }

    private void onUndoClicked()
    {
        DispatchEvent(GameEventType.UNDO);
    }

}
