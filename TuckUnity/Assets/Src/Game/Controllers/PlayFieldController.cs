using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFieldController : BaseController
{
    private TuckMatchState _matchState;

    private BoardView _boardView;
    private PlayerHandView _playerHandView;
    private CardResourceBank _gameplayResources;

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
            _setupPlayerHand(activePlayer.index);
        });
    }

    private PlayerState activePlayer
    {
        get
        {
            return _matchState.playerGroup.activePlayer;
        }
    }
    
    private void _setupPlayerHand(int playerIndex)
    {
        PlayerState player = _matchState.playerGroup.GetPlayerByIndex(playerIndex);
        
        for (int i = 0; i < PlayerState.kFirstHandSize; ++i)
        {
            CardData cardData = player.hand.GetCard(i);
            Debug.Log("CardType: " + cardData.id);

            CardView view = _gameplayResources.CreateCardView(cardData, _playerHandView.transform);
            _playerHandView.SetCardAtIndex(i, view);
        }
    }
    

}
