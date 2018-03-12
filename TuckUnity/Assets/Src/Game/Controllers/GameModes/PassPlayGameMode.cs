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


    public void Start()
    {
        _addCallbacks();

        GameContext context = Singleton.instance.sessionFlags.gameContext;
        //_playerList.Clear();
        //_playerList.AddRange(context.playerList);

      
        CardDeck deck = CardDeck.FromFile("Decks/StandardDeck");
        deck.Shuffle();

        /// TODO: TEMP, make player state
        for (int i = 0; i < PlayerGroup.kMaxPlayerCount; ++i)
        {
            string playerName = string.Format("P{0}", i);
            _playerList.Add(PlayerState.Create(i, playerName, i % 2));
        }

        _tuckMatchCore = TuckMatchCore.Create(_playerList, deck);
   
        _playFieldController.Start(_tuckMatchCore.matchState);

        _changeGameMatchMode(GameMatchMode.INITIAL);
        _changeGameMatchMode(GameMatchMode.SHUFFLE_AND_REDISTRIBUTE);
        _changeGameMatchMode(GameMatchMode.PARTNER_TRADE);

        _tuckMatchCore.ClearCommands();
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

        if(matchState.escrow.hasAllAssets)
        {
            _tuckMatchCore.ApplyTrade();
            _changeGameMatchMode(GameMatchMode.PLAYER_TURN);
        }

        _tuckMatchCore.ApplyNextPlayerTurn();
    }

    private void onPushTradeRequest(GeneralEvent e)
    {
        TradeRequest request = e.data as TradeRequest;
        _tuckMatchCore.AddTradeRequest(request);

    }

    private void onUndoTurn(GeneralEvent e)
    {
        _tuckMatchCore.Undo();
    }

    private void onRedoTurn(GeneralEvent e)
    {
        _tuckMatchCore.Redo();
    }

    private void _changeGameMatchMode(GameMatchMode mode)
    {
        _tuckMatchCore.ApplyChangeMatchMode(mode);
        _playFieldController.ChangeMatchMode(mode);
    }
}
