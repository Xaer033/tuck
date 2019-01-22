using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMatchMode
{
    NONE,
    INITIAL,
    SHUFFLE_AND_REDISTRIBUTE,
    PARTNER_TRADE,
    PLAYER_TURN,
    CHANGE_ACTIVE_PLAYER,
    REDISTRIBUTE,
    GAME_OVER
}