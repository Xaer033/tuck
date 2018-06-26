using GhostGen;
using System.Collections;
using UnityEngine;

public class MainMenuState : IGameState
{
    private MainMenuController _mainMenuController;

	public void Init( GameStateMachine stateMachine, Hashtable changeStateData )
	{
		Debug.Log ("Entering In MainMenu State");
        _mainMenuController = new MainMenuController();
        Singleton.instance.gui.viewFactory.CreateAsync<MainMenuView>("GUI/MainMenu/MainMenuView", (view) =>
        {
            Singleton.instance.gui.screenFader.FadeIn(0.35f, () =>
            {
                _mainMenuController.Start(view as MainMenuView);
            });
        });
        
    }
    
    public void Step( float p_deltaTime )
	{
		
    }

    public void Exit( )
	{
	//	_controller.getUI().rem
		Debug.Log ("Exiting In MainMenu State");

	}
    
}
