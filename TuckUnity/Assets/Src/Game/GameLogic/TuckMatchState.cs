﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuckMatchState 
{
    public PlayerGroup      playerGroup     { get; private set; }
    public Board            board           { get; private set; }
    public CardDeck         cardDeck        { get; private set; }
    public TeamCollection   teams           { get; private set; }
    public TradeEscrow      escrow          { get; private set; }
    public MoveValidator    validator       { get; private set; }

    public GameMatchMode    gameMatchMode   { get; set; }

    public static TuckMatchState Create(
        List<PlayerState> playerList, 
        CardDeck cardDeck)
    {
        TuckMatchState state = new TuckMatchState();

        state.cardDeck = cardDeck;
        state.playerGroup = PlayerGroup.Create(playerList, cardDeck);
        state.board = Board.Create(playerList);
        state.teams = TeamCollection.Create(playerList);
        state.escrow = TradeEscrow.Create(state.teams, state.playerGroup);
        state.validator = MoveValidator.Create(state.board);
        state.gameMatchMode = GameMatchMode.INITIAL;
        
        return state;
    }
    
}
