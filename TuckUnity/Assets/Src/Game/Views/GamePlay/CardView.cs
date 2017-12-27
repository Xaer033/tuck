using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GhostGen;
using UnityEngine.EventSystems;

using TMPro;
using DG.Tweening;

[System.Serializable]
[RequireComponent(typeof(EventTrigger))]
public sealed class CardView : 
    UIView,
    IBeginDragHandler, 
    IEndDragHandler, 
    IDragHandler
{
    private CardDragDropController _dragDropController;

    public EventTrigger eventTrigger;

    public int handIndex { get; set; }
    public PlayerHandView handView { get; set; }

    public Transform handSlot { get; set; }
    public Transform dragLayer { get; set; }

    public bool isDropSuccessfull { get; set; } 
   
    public bool isDragging
    {
        get { return _dragDropController.isDragging; }
    }

    void Awake()
    {
        eventTrigger = GetComponent<EventTrigger>();
        isDropSuccessfull = false;
    }

    void Start()
    {
        _setup();
    }
    
    public override void Update()
    {
        base.Update();
        _dragDropController.Step(Time.deltaTime);
    }
    
    public Image _cardIcon;
    public TextMeshProUGUI _valueTopLabel;
    public TextMeshProUGUI _valueBottomLabel;

    private CardData _cardData = null;

    public CardData cardData
    {
        set
        {
            if (_cardData != value)
            {
                _cardData = value;
                invalidateFlag |= InvalidationFlag.STATIC_DATA;
            }
        }

        get
        {
            return _cardData;
        }
    }
    
    protected override void OnViewUpdate()
    {
        base.OnViewUpdate();
        
        if ( _cardData != null && IsInvalid(InvalidationFlag.STATIC_DATA) )
        {

            _valueTopLabel.text = _cardData.titleKey; // TODO: Localize!
            _valueBottomLabel.text = _cardData.titleKey;
            _cardIcon.name = _cardData.iconName;

            //CardResourceBank cardBank = Singleton.instance.cardResourceBank;
            //_backgroundImg.sprite = cardBank.GetIngredientBG(ingredientData.cardType);
            //_cardTypeIcon.sprite = cardBank.GetIngredientTypeIcon(ingredientData.cardType);
            //_foodValueLbl.text = string.Format("{0}", ingredientData.foodValue);
            //_foodValueLbl.color = (ingredientData.foodValue > 0) ? Color.white : Color.red;
            //_cardIcon.sprite = cardBank.GetMainIcon(ingredientData.iconName);
        }        
    }
    
    private void _setup()
    {
        _dragDropController = new CardDragDropController(transform, handSlot, dragLayer);
    }

    public void OnBeginDrag(PointerEventData e)
    {
        _dragDropController.OnDragBegin(e);
    }

    public void OnDrag(PointerEventData e)
    {
        _dragDropController.OnDrag(e);
    }
    
    public void OnEndDrag(PointerEventData e)
    {
        handView.canvasGroup.blocksRaycasts = true;
        _dragDropController.OnDragEnd(e, isDropSuccessfull);
    }
}
