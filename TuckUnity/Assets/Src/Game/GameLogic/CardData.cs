using UnityEngine;
using UnityEngine.Assertions;


using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public enum CardColor
{
    RED,
    BLACK
}

public class SuiteType
{
    public const string SPADES = "spades";
    public const string CLUBS = "clubs";
    public const string DIAMONDS = "diamonds";
    public const string HEARTS = "hearts";
}

public class MoveType
{
    public const string FORWARDS = "forwards"; // Normal Movement
    public const string BACKWARDS = "backwards"; // Can move backwards
    public const string SPLIT_STOMP = "split_stomp"; // Can split move between multiple pieces & destroy any piece along the way
    public const string SWAP = "swap"; // Swaps any two pieces not in a safe zone
    public const string LEAVE_BASE = "leave_base"; // Can escape home base
    //public const string MULTI_VALUE = "multi_value";
}

public interface IMoveModifier
{
    string moveType { get; }

    void GetMoveablePieces();
    
}

[System.Serializable]
public class PieceMovementData
{
    public string type;
    public int value;
}

public class CardUtility
{
    
    //public static CardType TypeFromString(string cardType)
    //{
    //    switch(cardType.ToLower())
    //    {
    //        case "meat": return CardType.Meat;
    //        case "veggie": return CardType.Veggie;
    //        case "topping": return CardType.Topping;
    //        case "customer": return CardType.Customer;
    //    }

    //    Debug.LogError(string.Format("We don't handle card type: {0}!", cardType));
    //    return CardType.None;
    //}

    //public static int ApplyModifier(string modifier, int originalScore)
    //{
    //    if(string.IsNullOrEmpty(modifier))
    //    {
    //        return originalScore;
    //    }

    //    switch(modifier)
    //    {
    //        case PointModifier.X2: return originalScore * 2;
    //    }
        
    //    return originalScore;
    //}
}

[System.Serializable]
public class CardData : System.Object
{
    public string       id;
    public string       titleKey;
    public string       iconName;
    public string       suite;

    //Normally I only want data types here but this is an exception because this doesn't change and doesn't need to be mutable from json
    public CardColor    color
    {
        get
        {
            return suite == SuiteType.SPADES || suite == SuiteType.CLUBS ? 
                CardColor.BLACK : CardColor.RED;
        }
    }

    public PieceMovementData[]    pieceMovementList;
}

public class CardDataFactory
{
    public static CardData CreateFromJToken(JToken cardToken)
    {
        CardData result = new CardData();

        JsonUtility.FromJsonOverwrite(cardToken.ToString(), result);
        
        JArray movementArray = JArray.Parse(cardToken.SelectToken("moveType").ToString());
        PieceMovementData[] pieceMovementList = new PieceMovementData[movementArray.Count];

        for (int i = 0; i < movementArray.Count; ++i)
        {
            pieceMovementList[i] = JsonUtility.FromJson<PieceMovementData>(movementArray[i].ToString());
        }
        result.pieceMovementList = pieceMovementList;

        return result;
    }

    public static CardData CreateFromJson(string cardJson)
    {
        JToken token = JToken.Parse(cardJson);
        return CreateFromJToken(token);
    }

    public static string ToJson(CardData cData, bool prettyPrint)
    {
        return JsonUtility.ToJson(cData, prettyPrint);
    }    
}