using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

[System.Serializable]
public class CardDeck : object
{
    private List<CardData> _cardList = new List<CardData>();

    public List<CardData> cardList { get { return _cardList; } }

    [System.Serializable]
    public class JsonDeck : object
    {
        public CardData[] cardList;
    }

    public static CardDeck FromFile(string path)
    {
        TextAsset ingredientDeckJson = Resources.Load<TextAsset>(path);
        return CardDeck.FromJson(ingredientDeckJson.text);
    }

    public static CardDeck FromJson(string jsonStr)
    {
        CardDeck deck = new CardDeck();
        JArray jsonDeck = JArray.Parse(jsonStr);

        for (int i = 0; i < jsonDeck.Count; ++i)
        {
            JToken card = jsonDeck[i];
            deck._cardList.Add(CardDataFactory.CreateFromJToken(card));
            
        }
        return deck;
    }

    public static string ToJson(CardDeck deck, bool prettyPrint)
    {
        return JsonUtility.ToJson(deck, prettyPrint);
    }


    public CardData Pop()
    {
        CardData card = top;
        _cardList.Remove(card);

        return card;
    }

    public void Push(CardData card)
    {
        _cardList.Add(card);
    }

    public CardData top
    {
        get
        {
            if (_cardList.Count == 0)
            {
                return null;
            }

            return _cardList[_cardList.Count - 1];
        }
    }

    public CardData bottom
    {
        get
        {
            if (_cardList.Count == 0)
            {
                return null;
            }

            return _cardList[0];
        }
    }

    public void Shuffle(int randomSeed = -1)
    {
        System.Random random = new System.Random(randomSeed);
        
        _cardList.Sort((a, b) =>
        {
            return random.Next(0, 100).CompareTo(random.Next(0, 100));
        });
    }

    public bool isEmpty
    {
        get
        {
            return _cardList.Count == 0;
        }
    }
    
}
