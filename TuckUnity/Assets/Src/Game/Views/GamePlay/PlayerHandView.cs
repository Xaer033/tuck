using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System;
using System.Collections;
using GhostGen;
using DG.Tweening;

public class PlayerHandView : UIView
{
    public const float kHandTweenDuration = 0.15f;

    public Transform[] cardSlotList;

    public RectTransform handTransform;

    public CanvasGroup canvasGroup;
    public Transform dragCardLayer;
    public Image dragBlocker;
    public Button toggleButton;

    private CardView[] _cardViewList = new CardView[PlayerHand.kFirstHandSize];
    private Tween _handTween;
    private Vector3 _shownPosition;
    private Vector3 _hiddenPosition;

    private bool _handHidden;

    void Awake()
    {
        blockCardDrag = false;

        _shownPosition = handTransform.anchoredPosition;
        _hiddenPosition = new Vector3(0, -100, 0);

        toggleButton.onClick.AddListener(onToggleButton);

        //AddListener(GameEventType.CARD_DROPPED, (e) =>
        //{
        //    Debug.Log("BubbleInfo: " + e.currentTarget + ", " + e.target);
        //    Debug.Log("Bubble Working? : " +  e.data["success"]);
        //});
    }

    public int playerIndex { get; set; }

    public bool blockCardDrag
    {
        set
        {
            dragBlocker.gameObject.SetActive(value);
        }
    }
    


    public void SetCardAtIndex(int index, CardView card)
    {
        _boundsCheck(index);
        if(card != _cardViewList[index])
        {
            _cardViewList[index] = _processCardView(index, card);
            invalidateFlag |= InvalidationFlag.STATIC_DATA;
        }
    }

    public CardView GetCardAtIndex(int index)
    {
        _boundsCheck(index);
        return _cardViewList[index];
    }

    public void RemoveCardByIndex(int index)
    {
        _boundsCheck(index);
        if(_cardViewList[index])
        {
            //_cardViewList[index].gameObject.SetActive(false);
            Singleton.instance.gui.viewFactory.RemoveView(_cardViewList[index]);
            _cardViewList[index] = null;
            invalidateFlag = InvalidationFlag.ALL;
        }
    }

    public void Show(Action onComplete)
    {
        _killHandTween();

        _handHidden = false;

        _handTween = handTransform.DOAnchorPos3D(_shownPosition, kHandTweenDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (onComplete != null)
                {
                    onComplete();
                }
            });
    }

    public void Hide(Action onComplete)
    {
        _killHandTween();

        _handHidden = true;

        _handTween = handTransform.DOAnchorPos3D(_hiddenPosition, kHandTweenDuration * 2.0f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (onComplete != null)
                {
                    onComplete();
                }
            });
    }

    private void _killHandTween()
    {
        if(_handTween != null)
        {
            _handTween.Kill(true);
            _handTween = null;
        }
    }

    protected override void OnViewUpdate()
    {
        if(IsInvalid(InvalidationFlag.STATIC_DATA))
        {
            for(int i = 0; i < _cardViewList.Length; ++i)
            {
                CardView cardView = _cardViewList[i];
                if (cardView)
                {
                    cardView.invalidateFlag = InvalidationFlag.STATIC_DATA;
                }
            }
        }
    }

    private CardView _processCardView(
        int handIndex,
        CardView cardView)
    {
        if (cardView == null) { return null; }
        _boundsCheck(handIndex);

        Transform slot = cardSlotList[handIndex];
        cardView.transform.SetParent(slot);
        cardView.transform.localPosition = Vector3.zero;
        cardView.transform.rotation = slot.rotation;
        cardView.handView = this;
        cardView.handSlot = slot;
        cardView.dragLayer = dragCardLayer;
        cardView.handIndex = handIndex;
        cardView.ownerIndex = playerIndex;

        return cardView;
    }

    private void _boundsCheck(int index)
    {
        Debug.Assert(index >= 0, "Index is less than 0!");
        Debug.Assert(index < PlayerHand.kFirstHandSize, "Index is greater than slot container size");
    }

    private void onToggleButton()
    {
        if(_handHidden)
        {
            Show(null);
        }
        else
        {
            Hide(null);
        }
    }
}

