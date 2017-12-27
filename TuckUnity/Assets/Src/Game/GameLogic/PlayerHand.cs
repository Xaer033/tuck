using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class PlayerHand
{
    public const int kDefaultHandSize = 5;

    private CardData[] _cards;
    private int _handSize;

    public static PlayerHand Create(int handSize)
    {
        int kHandSize = (handSize <= 0) ? kDefaultHandSize : handSize;
        PlayerHand hand = new PlayerHand();
        hand._handSize = kHandSize;
        hand._cards = new CardData[kHandSize];
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
