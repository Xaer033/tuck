using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GhostGen;

public class PlayFieldController : BaseController
{
    public enum PlayFieldState
    {
        NONE,
        INITIAL,
        SHUFFLE_AND_REDISTRIBUTE,
        PARTNER_TRADE,
        PLAYER_TURN,
        REDISTRIBUTE,
        GAME_OVER
    }

    const int kLocalPlayerIndex = 0; // TODO: Temporary until we have real multiplayer

    private TuckMatchState _matchState;

    private BoardView _boardView;
    private PlayerHandView _playerHandView;
    private CardResourceBank _gameplayResources;
    private GameHudView _gameHudView;
    private PlayFieldState _playfieldState;
    
    public PlayFieldController()
    {
        _gameplayResources = Singleton.instance.cardResourceBank;
    }

    public void Start(TuckMatchState state)
    {
        _matchState = state;

        viewFactory.CreateAsync<BoardView>("GUI/GamePlay/BoardView", (view) =>
        {
            _boardView = view as BoardView;
            _boardView.SetBoard(_matchState.board);
        }, Singleton.instance.sceneRoot);

        viewFactory.CreateAsync<PlayerHandView>("GUI/GamePlay/PlayerHandView", (view) =>
        {
            _playerHandView = view as PlayerHandView;
            _playerHandView.AddListener(GameEventType.TRADE_CARD, onTradeCardDrop);
            _playerHandView.AddListener(GameEventType.PLAY_CARD, onPlayCardDrop);

            _setupPlayerHand(activePlayer.index);
        });

        viewFactory.CreateAsync<GameHudView>("GUI/GamePlay/GameHudView", (view) =>
        {
            _gameHudView = view as GameHudView;
            _gameHudView.AddListener(GameEventType.UNDO, (x) =>
            {
                DispatchEvent(GameEventType.UNDO);
                _setupPlayerHand(activePlayer.index);
            });
        });

        _changeState(PlayFieldState.PARTNER_TRADE);
    }


    private PlayerState activePlayer
    {
        get
        {
            return _matchState.playerGroup.activePlayer;
        }
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
        
        for (int i = 0; i < PlayerState.kFirstHandSize; ++i)
        {
            _playerHandView.RemoveCardByIndex(i);
            CardData cardData = player.hand.GetCard(i);
            CardView view = _gameplayResources.CreateCardView(cardData, _playerHandView.transform);
            _playerHandView.SetCardAtIndex(i, view);
            view.Validate();
        }
    }

    private bool _changeState(PlayFieldState newState, object changeStateData = null)
    {
        PlayFieldState oldState = _playfieldState;
        _playfieldState = newState;

        switch(_playfieldState)
        {
            case PlayFieldState.INITIAL:                    return _initialState(changeStateData);
            case PlayFieldState.SHUFFLE_AND_REDISTRIBUTE:   return _shuffleAndRedist(changeStateData);
            case PlayFieldState.PARTNER_TRADE:              return _partnerTrade(changeStateData);
            case PlayFieldState.REDISTRIBUTE:               return _redistribute(changeStateData);
            case PlayFieldState.PLAYER_TURN:                return _playerTurn(changeStateData);
            case PlayFieldState.GAME_OVER:                  return _gameOver(changeStateData);
        }

        Debug.LogErrorFormat("Could not change state to: {0}", newState);
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
        return true;
    }
    private bool _redistribute(object changeStateData)
    {
        return true;
    }
    private bool _playerTurn(object changeStateData)
    {
        return true;
    }
    private bool _gameOver(object changeStateData)
    {
        return true;
    }

    private void onTradeCardDrop(GeneralEvent e)
    {
        if(_playfieldState != PlayFieldState.PARTNER_TRADE)
        {
            return;
        }

        Debug.Log("SwapCard: " + e.data);
        PointerEventData data = (e.data as PointerEventData);
        CardView droppedCard = data.pointerDrag.GetComponent<CardView>();
        if(droppedCard != null)
        {
            addTradeCard(droppedCard.ownerIndex, droppedCard.handIndex);
        }
    }

    private void onPlayCardDrop(GeneralEvent e)
    {
        Debug.Log("PlayCard : " + e.data);
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
        return result;
    }
}
