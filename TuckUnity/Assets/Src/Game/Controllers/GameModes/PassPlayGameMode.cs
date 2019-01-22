using System;
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


    public void Start(GameContext context)
    {
        _addCallbacks();
        
        CardDeck deck = CardDeck.FromFile("Decks/StandardDeck");
        deck.Shuffle();

        /// TODO: TEMP, make player state
        for (int i = 0; i < PlayerGroup.kMaxPlayerCount; ++i)
        {
            string playerName = string.Format("P{0}", i);
            _playerList.Add(PlayerState.Create(i, playerName, i % 2));
        }

        _tuckMatchCore = TuckMatchCore.Create(_playerList, deck);
   
        _playFieldController.Start(_tuckMatchCore.matchState, ()=>
        {
            _changeGameMatchMode(GameMatchMode.INITIAL);
            _changeGameMatchMode(GameMatchMode.SHUFFLE_AND_REDISTRIBUTE);
            _changeGameMatchMode(GameMatchMode.PLAYER_TURN);

            _tuckMatchCore.ClearCommands();
        });

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

    private TuckMatchState matchState
    {
        get { return _tuckMatchCore.matchState; }
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
        _playFieldController.AddListener(GameEventType.UNDO, onUndoTurn);
        _playFieldController.AddListener(GameEventType.REDO, onRedoTurn);
        _playFieldController.AddListener(GameEventType.TRADE_CARD, onPushTradeRequest);
        _playFieldController.AddListener(GameEventType.MOVE_REQUEST, onMoveRequest);
        _playFieldController.AddListener(GameEventType.FINISH_TURN, onFinishTurn);

        //_playFieldController.onPlayOnCustomer   = onPlayCard;
        //_playFieldController.onResolveScore     = onResolveScore;
        //_playFieldController.onEndTurn          = onEndTurn;
        //_playFieldController.onUndoTurn         = onUndoTurn;
        //_playFieldController.onGameOver         = onGameOver;
    }

    private void _removeCallbacks()
    {
        _playFieldController.RemoveListener(GameEventType.UNDO, onUndoTurn);
        _playFieldController.RemoveListener(GameEventType.REDO, onRedoTurn);
        _playFieldController.RemoveListener(GameEventType.TRADE_CARD, onPushTradeRequest);
        _playFieldController.RemoveListener(GameEventType.FINISH_TURN, onFinishTurn);
        //_playFieldController.onPlayOnCustomer   = onPlayCard;
        //_playFieldController.onResolveScore     = onResolveScore;
        //_playFieldController.onEndTurn          = onEndTurn;
        //_playFieldController.onUndoTurn         = onUndoTurn;
        //_playFieldController.onGameOver         = onGameOver;
    }

    private void onFinishTurn(GeneralEvent e)
    {
        GameMatchMode prevMode = matchState.gameMatchMode;
        _changeGameMatchMode(GameMatchMode.CHANGE_ACTIVE_PLAYER);

        if(prevMode == GameMatchMode.PARTNER_TRADE)
        {
            if(matchState.escrow.hasAllAssets)
            {
                _tuckMatchCore.ApplyTrade();
                _changeGameMatchMode(GameMatchMode.PLAYER_TURN);
            }
            else
            {
                _changeGameMatchMode(GameMatchMode.PARTNER_TRADE);
            }
        }
        else
        {        
            _changeGameMatchMode(GameMatchMode.PLAYER_TURN);
        }

        _tuckMatchCore.ApplyNextPlayerTurn();
    }

    private void onPushTradeRequest(GeneralEvent e)
    {
        TradeRequest request = e.data as TradeRequest;
        _tuckMatchCore.AddTradeRequest(request);

    }
    private void onMoveRequest(GeneralEvent e)
    {
        MoveRequest request = e.data as MoveRequest;
        _tuckMatchCore.ApplyMoveCommand(request);
    }

    private void onUndoTurn(GeneralEvent e)
    {
        if(_tuckMatchCore.Undo())
        {
            _playFieldController.ChangeMatchMode(_tuckMatchCore.matchState.gameMatchMode);

            _playFieldController.RefreshBoard();
            _playFieldController.RefreshHand();
        }
    }

    private void onRedoTurn(GeneralEvent e)
    {
        if(_tuckMatchCore.Redo())
        {
            _playFieldController.ChangeMatchMode(_tuckMatchCore.matchState.gameMatchMode);

            _playFieldController.RefreshBoard();
            _playFieldController.RefreshHand();
        }
    }

    private void _changeGameMatchMode(GameMatchMode mode)
    {
        _tuckMatchCore.ApplyChangeMatchMode(mode);
        _playFieldController.ChangeMatchMode(mode);
    }
}
