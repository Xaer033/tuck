﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostGen;

public class PassPlayGameMode : NotificationDispatcher, IGameModeController
{
    //private PassAndPlayFieldController  _playFieldController        = new PassAndPlayFieldController();
    //private GameOverPopupController     _gameOverPopupController    = new GameOverPopupController();
    private TuckMatchCore _tuckMatchCore;
    private PlayFieldController _playFieldController = new PlayFieldController();
    private List<PlayerState> _playerList = new List<PlayerState>(PlayerGroup.kMaxPlayerCount);


    public void Start()
    {
        //_onGameOverCallback = gameOverCallback;

        GameContext context = Singleton.instance.sessionFlags.gameContext;
        //_playerList.Clear();
        //_playerList.AddRange(context.playerList);

      
        CardDeck deck = CardDeck.FromFile("Decks/StandardDeck");
        deck.Shuffle();

        /// TODO: TEMP, make player state
        for (int i = 0; i < PlayerGroup.kMaxPlayerCount; ++i)
        {
            _playerList.Add(PlayerState.Create(i, "", i % 2));
        }

        _tuckMatchCore = TuckMatchCore.Create(_playerList, deck);

        _addCallbacks();
        _playFieldController.Start(_tuckMatchCore.matchState);        
    }

    public void Step(double deltaTime)
    {

    }

    public void CleanUp()
    {
        _removeCallbacks();

        _playFieldController.RemoveView();
        RemoveAllListeners();
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
    
    private void _addCallbacks()
    {
        _playFieldController.AddListener(GameEventType.APPLY_TRADE, onApplyTrade);
        //_playFieldController.onPlayOnCustomer   = onPlayCard;
        //_playFieldController.onResolveScore     = onResolveScore;
        //_playFieldController.onEndTurn          = onEndTurn;
        //_playFieldController.onUndoTurn         = onUndoTurn;
        //_playFieldController.onGameOver         = onGameOver;
    }

    private void _removeCallbacks()
    {
        _playFieldController.RemoveListener(GameEventType.APPLY_TRADE, onApplyTrade);
        //_playFieldController.onPlayOnCustomer   = onPlayCard;
        //_playFieldController.onResolveScore     = onResolveScore;
        //_playFieldController.onEndTurn          = onEndTurn;
        //_playFieldController.onUndoTurn         = onUndoTurn;
        //_playFieldController.onGameOver         = onGameOver;
    }
    private void onApplyTrade(GeneralEvent e)
    {
        List<TradeRequest> requestList = e.data as List<TradeRequest>;
        _tuckMatchCore.ApplyTrade(requestList);
    }
    
}
