using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class PlayerHand
{
    public const int kFirstHandSize = 5;
    public const int kNonFirstHandSize = 4;

    private CardData[] _cards;
    private int _handSize;

    public static PlayerHand Create()
    {
        PlayerHand hand = new PlayerHand();
        hand._handSize = kFirstHandSize;
        hand._cards = new CardData[kFirstHandSize];
        return hand;

    }

    public int emptyCards
    {
        get
        {
            int empty = 0;
            for (int i = 0; i < _handSize; ++i)
            {
                if (_cards[i] == null)
                    empty++;
            }

            return empty;
        }
    }

    public CardData PopCard(int index)
    {
        _boundsCheck(index);
        CardData tmpCard = _cards[index];
        _cards[index] = null;
        return tmpCard;
    }

    public CardData ReplaceCard(int index, CardData newCard)
    {
        _boundsCheck(index);

        CardData oldCard = _cards[index];
        _cards[index] = newCard;
        return oldCard;
    }

    public CardData GetCard(int index)
    {
        _boundsCheck(index);
        return _cards[index];
    }

    public void SetCard(int index, CardData card)
    {
        _boundsCheck(index);
        _cards[index] = card;
    }



    private void _boundsCheck(int index)
    {
        Debug.Assert(index >= 0);
        Debug.Assert(index < _handSize);
    }
}
