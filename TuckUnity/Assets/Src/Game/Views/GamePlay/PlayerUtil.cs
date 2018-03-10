using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtil 
{
	public static Color GetColor(int playerIndex)
    {
        switch (playerIndex)
        {
            case 0: return Color.green;
            case 1: return new Color(1.0f, 0.75f, 0.26f);
            case 2: return new Color(1.0f, 0.1f, 0.1f);
            case 3: return new Color(0.3f, 0.1f, 1.0f);
        }

        Debug.LogError("Not handled playerIndex: " + playerIndex);
        return Color.black;
    }
}