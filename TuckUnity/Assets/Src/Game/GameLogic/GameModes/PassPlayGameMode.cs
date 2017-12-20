using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostGen;

public class PassPlayGameMode : NotificationDispatcher, IGameModeController
{
    //private PassAndPlayFieldController  _playFieldController        = new PassAndPlayFieldController();
    //private GameOverPopupController     _gameOverPopupController    = new GameOverPopupController();
    //private GameMatchCore   _gameMatchCore;

    //private List<PlayerState>           _playerList                 = new List<PlayerState>(4);



    public void Start()
    {
        //_onGameOverCallback = gameOverCallback;

        GameContext context = Singleton.instance.sessionFlags.gameContext;
        //_playerList.Clear();
        //_playerList.AddRange(context.playerList);

        //_gameMatchCore = GameMatchCore.Create(
        //    _playerList,
        //    context.customerDeck, 
        //    context.ingredientDeck);

        //_playFieldController.Start(_gameMatchCore.matchState);
        _setupCallbacks();

        CardDeck deck = CardDeck.FromFile("Decks/StandardDeck");
        Debug.Log(deck.isEmpty);
    }

    public void Step(double deltaTime)
    {

    }

    public void CleanUp()
    {
        //_playFieldController.RemoveView();
        RemoveAllListeners(GameEventType.GAME_OVER);
    }

    private void onGameOver(bool gameOverPopup = true)
    {
        if (!gameOverPopup)
        {
            Singleton.instance.gui.screenFader.FadeOut(0.5f, () =>
            {
                DispatchEvent(GameEventType.GAME_OVER);
            });
        }
        else
        {
            //MatchOverEvent matchOver = MatchOverEvent.Create(_playerList);
            //_gameOverPopupController.Start(matchOver.playerRanking, () =>
            //{
                DispatchEvent(GameEventType.GAME_OVER);
            //});
        }
    }
    
    private void _setupCallbacks()
    {
        //_playFieldController.onPlayOnCustomer   = onPlayCard;
        //_playFieldController.onResolveScore     = onResolveScore;
        //_playFieldController.onEndTurn          = onEndTurn;
        //_playFieldController.onUndoTurn         = onUndoTurn;
        //_playFieldController.onGameOver         = onGameOver;
    }
    
}
