using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GhostGen;
using DG.Tweening;

public class GameplayState : IGameState
{
    private IGameModeController _gameModeController;
    private NotificationDispatcher _gameModeDispatcher;

    private GameStateMachine    _stateMachine;

    public void Init( GameStateMachine stateMachine, Hashtable changeStateData )
	{       
        Debug.Log ("Entering In GamePlay State");

		_stateMachine = stateMachine;

        Tween introTween = Singleton.instance.gui.screenFader.FadeIn(1.0f);
        introTween.SetDelay(0.25f);

        _gameModeController = createGameModeController();
        _gameModeController.AddListener(GameEventType.GAME_OVER, onGameOver);
        _gameModeController.Start();
    }

    
    public void Step( float p_deltaTime )
	{
        _gameModeController.Step(p_deltaTime);
    }

    public void Exit()
	{
		Debug.Log ("Exiting In Intro State");

        _gameModeController.RemoveListener(GameEventType.GAME_OVER, onGameOver);
        _gameModeController.CleanUp();   
    }

    
    private void onGameOver(GeneralEvent e)
    {
        _stateMachine.ChangeState(TuckState.MAIN_MENU); ;
    }

    private IGameModeController createGameModeController()
    {
        GameContext context = Singleton.instance.sessionFlags.gameContext;
        if(context == null)
        {
            context = GameContext.Create(GameMode.PASS_AND_PLAY);
        }

        switch(context.gameMode)
        {
            case GameMode.SINGLE_PLAYER:    return null;
            case GameMode.PASS_AND_PLAY:    return new PassPlayGameMode();
            //case GameMode.ONLINE:           return new OnlineGameMode();
        }
        Debug.LogErrorFormat("Not supported gametype {0}", context.gameMode);
        return null;
    }
}
