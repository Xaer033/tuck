using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GhostGen;

[CreateAssetMenu(menuName = "Mr.Tuck/Card Resource Bank")]
public class CardResourceBank : ScriptableObject, IPostInit
{
    [System.Serializable]
    public class IngredientCardSprites
    {
        public Sprite background;
        public Sprite icon;
    }
    

    public CardView cardPrefab;

    public Texture2D iconAtlas;


    private Dictionary<string, Sprite> _iconMap = new Dictionary<string, Sprite>();

    public void PostInit()
    { 
        Sprite[] sprites = Resources.LoadAll<Sprite>("Atlases/" + iconAtlas.name);
        foreach(Sprite s in sprites)
        {
            _iconMap.Add(s.name, s);
        }
    }

    public Sprite GetMainIcon(string iconName)
    {
        Sprite icon = null;
        if(!_iconMap.TryGetValue(iconName, out icon))
        {
            Debug.LogError(string.Format("Could not find icon named: {0}", iconName));
            return null;
        }
        return icon;
    }

    public CardView CreateCardView(CardData cardData, Transform cParent)
    {
        CardView view = Instantiate<CardView>(cardPrefab, cParent, false);
        view.cardData = cardData;

        return view;
    }
}
