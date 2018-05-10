using GhostGen;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class PlayFieldController : BaseController
{
    const int kLocalPlayerIndex = 0; // TODO: Temporary until we have real multiplayer

    enum PlayMoveState
    {
        PLAY_CARD,
        SELECT_PAWNS,
        SELECT_PATHS,
    }
    
    private TuckMatchState _matchState;

    private BoardView _boardView;
    private PlayerHandView _playerHandView;
    private CardResourceBank _gameplayResources;
    private GameHudView _gameHudView;
    private GameMatchMode _oldMatchMode;

    private PlayMoveState _playMoveState;

    private MoveRequest _currentMoveRequest;
    private int _sevenCardSpacesLeft;

    public PlayFieldController()
    {
        _gameplayResources = Singleton.instance.cardResourceBank;
    }

    public void Start(TuckMatchState state, Action onViewsLoaded)
    {
        _matchState = state;

        int viewsToLoadCount = 3;

        viewFactory.CreateAsync<BoardView>("GUI/GamePlay/BoardView", (view) =>
        {
            _boardView = view;
            _boardView.board = _matchState.board;
            _boardView.AddListener(GameEventType.PEG_TAP, onPegTapped);
            _boardView.AddListener(GameEventType.PIECE_TAP, onPieceTapped);

            checkViewsLoaded(--viewsToLoadCount, onViewsLoaded);
        }, Singleton.instance.sceneRoot);

        viewFactory.CreateAsync<PlayerHandView>("GUI/GamePlay/PlayerHandView", (view) =>
        {
            _playerHandView = view;
            _playerHandView.AddListener(GameEventType.TRADE_CARD, onTradeCardDrop);
            _playerHandView.AddListener(GameEventType.PLAY_CARD, onPlayCardDrop);
            
            _setupPlayerHand(activePlayer.index);

            checkViewsLoaded(--viewsToLoadCount, onViewsLoaded);
        });

        viewFactory.CreateAsync<GameHudView>("GUI/GamePlay/GameHudView", (view) =>
        {
            _gameHudView = view;
            _gameHudView.AddListener(GameEventType.UNDO, onForwardEventAndRefreshHand);
            _gameHudView.AddListener(GameEventType.REDO, onForwardEventAndRefreshHand);        
            _gameHudView.AddListener(GameEventType.FINISH_TURN, onForwardEventAndRefreshHand);

            checkViewsLoaded(--viewsToLoadCount, onViewsLoaded);
        });
    }

    private void checkViewsLoaded(int count, Action callback)
    {
        if(count <= 0 && callback != null)
        {
            callback();
        }
    }

    public bool ChangeMatchMode(GameMatchMode m)
    {
        return _gameModeChanged(m);
    }

    private PlayerState activePlayer
    {
        get
        {
            return _matchState.playerGroup.activePlayer;
        }
    }

    private void onForwardEventAndRefreshHand(GeneralEvent e)
    {
        DispatchEvent(e.type);
        _setupPlayerHand(activePlayer.index);
    }

    private PlayerState localPlayer
    {
        get
        {
            return _matchState.playerGroup.GetPlayerByIndex(kLocalPlayerIndex);
        }
    }

    private void _setupPlayerHand(int playerIndex)
    {
        _playerHandView.playerIndex = playerIndex;
        PlayerState player = _matchState.playerGroup.GetPlayerByIndex(playerIndex);
        
        for (int i = 0; i < PlayerHand.kFirstHandSize; ++i)
        {
            _playerHandView.RemoveCardByIndex(i);
            CardData cardData = player.hand.GetCard(i);
            if(cardData != null)
            {
                CardView view = _gameplayResources.CreateCardView(cardData, _playerHandView.transform);
                _playerHandView.SetCardAtIndex(i, view);
                view.Validate();
            }
        }

        if(_boardView)
        {
            _boardView.viewIndex = playerIndex;
        }
    }

    private bool _gameModeChanged(GameMatchMode newMode, object changeStateData = null)
    {
        Debug.LogFormat("Update PlayField game mode From: {0} to {1}", _oldMatchMode, newMode);
        Assert.IsFalse(newMode == _oldMatchMode, "Should not be updating mode to the same state!");
        _oldMatchMode = newMode;

        switch (newMode)
        {
            case GameMatchMode.INITIAL:                    return _initialState(changeStateData);
            case GameMatchMode.SHUFFLE_AND_REDISTRIBUTE:   return _shuffleAndRedist(changeStateData);
            case GameMatchMode.PARTNER_TRADE:              return _partnerTrade(changeStateData);
            case GameMatchMode.REDISTRIBUTE:               return _redistribute(changeStateData);
            case GameMatchMode.PLAYER_TURN:                return _playerTurn(changeStateData);
            case GameMatchMode.GAME_OVER:                  return _gameOver(changeStateData);
        }

        Debug.LogErrorFormat("Could not change state to: {0}", newMode);
        return false;
    }

    private bool _initialState(object changeStateData)
    {
        
        return true;
    }
    private bool _shuffleAndRedist(object changeStateData)
    {
        return true;
    }
    private bool _partnerTrade(object changeStateData)
    {
        _playerHandView.tradeMatEnabled = true;
        return true;
    }
    private bool _redistribute(object changeStateData)
    {
        return true;
    }
    private bool _playerTurn(object changeStateData)
    {
        Debug.Log("Player Turn!");

        _currentMoveRequest = new MoveRequest();
        _currentMoveRequest.playerIndex = activePlayer.index;

        _playMoveState = PlayMoveState.PLAY_CARD;
        _playerHandView.playCardMatEnabled = true;
        _playerHandView.tradeMatEnabled = false;
        return true;
    }
    private bool _gameOver(object changeStateData)
    {
        return true;
    }

    private void onTradeCardDrop(GeneralEvent e)
    {
        PointerEventData data = (e.data as PointerEventData);
        CardView droppedCard = data.pointerDrag.GetComponent<CardView>();

        if( _matchState.gameMatchMode != GameMatchMode.PARTNER_TRADE 
         || droppedCard == null
         || _matchState.escrow.HasAssetFromPlayer(droppedCard.ownerIndex))
        {
            return;
        }

        Debug.Log("SwapCard: " + droppedCard);
        addTradeCard(droppedCard.ownerIndex, droppedCard.handIndex);
    }

    private void onPlayCardDrop(GeneralEvent e)
    {
        Debug.Log("PlayCard : " + e.data);

        PointerEventData data = (e.data as PointerEventData);
        CardView droppedCard = data.pointerDrag.GetComponent<CardView>();

        if(_playMoveState == PlayMoveState.PLAY_CARD)
        {
            _playMoveState = PlayMoveState.SELECT_PAWNS;
            _playerHandView.playCardMatEnabled = false;
            _currentMoveRequest.handIndex = droppedCard.handIndex;


            DispatchEvent(e); // Pass on
        }
    }

    private bool isSplitStomp(CardData card)
    {
        foreach(var movement in card.pieceMovementList)
        {
            if(movement.type == MoveType.SPLIT_STOMP)
            {
                return true;
            }
        }
        return false;
    }

    private void onPegTapped(GeneralEvent e)
    {
        PegView peg = e.data as PegView;
        Debug.Log("Peg: " + peg.ToString());
        
    }

    private void onPieceTapped(GeneralEvent e)
    {
        PieceView pieceView = e.data as PieceView;
        Debug.Log("Piece: " + pieceView.ToString());

        if(_playMoveState == PlayMoveState.SELECT_PAWNS)
        {
            CardData currentCard = activePlayer.hand.GetCard(_currentMoveRequest.handIndex);
            
            var piecePath = new MoveRequest.PiecePathData();
            piecePath.pieceIndex = pieceView.piece.index;

            var pathList = new List<MovePath>();
            if(_matchState.validator.GetValidPaths(pieceView.piece, currentCard, ref pathList))
            {

            }
        }
    }
    

    private bool addTradeCard(int playerIndex, int handSlotIndex)
    {
        bool result = false;

        PlayerState player = _matchState.playerGroup.GetPlayerByIndex(playerIndex);
        TradeRequest trade = TradeRequest.Create(player.index, player.teamIndex, handSlotIndex);

        if(_matchState.escrow.HasAsset(trade))
        {
            result = false;
        }
        else
        {
            DispatchEvent(GameEventType.TRADE_CARD, false, trade);
            result = true;
        }
          
        _setupPlayerHand(activePlayer.index);
        _playerHandView.Show(null);
        return result;
    }
}
