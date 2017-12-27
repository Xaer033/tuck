using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;


public class PlayFieldViewUtils 
{
	public static Sequence ZoomSlamTween(
        CardView ingredient,
        CardView customer,
        bool shouldMoveTween,
        TweenCallback slamCallback,
        TweenCallback finishCallback)
    {
        Vector3 originalScale = ingredient.transform.localScale;

        Vector3 cardEyeVec = (Camera.main.transform.position - customer.transform.position).normalized;
        Sequence sequence = DOTween.Sequence();
        Tween moveToTween = null;
        if (shouldMoveTween)
        {
            moveToTween = ingredient.transform.DOMove(customer.transform.position + cardEyeVec, 0.27f);
            moveToTween.SetEase(Ease.OutCubic);
        }

        Tween growTween = ingredient.transform.DOScale(originalScale * 1.3f, 0.31f);
        growTween.SetEase(Ease.OutCubic);

        Tween slamTween = ingredient.transform.DOScale(originalScale * 0.1f, 0.2f);
        slamTween.SetEase(Ease.InCubic);

        Sequence shakeSeq = DOTween.Sequence();
        Tween shakePosTween = customer.transform.DOShakePosition(0.4f, 10.0f, 22);
        shakePosTween.SetEase(Ease.OutCubic);
        Tween shakeRotTween = customer.transform.DOShakeRotation(0.4f, 6.0f, 16);
        shakeRotTween.SetEase(Ease.OutCubic);
        shakeSeq.Insert(0.0f, shakePosTween);
        shakeSeq.Insert(0.0f, shakeRotTween);
        if(shouldMoveTween)
        {
            sequence.Insert(0.0f, moveToTween);
        }
        sequence.Insert(0.0f, growTween);
        sequence.Append(slamTween);

        if(slamCallback != null)
        {
            sequence.AppendCallback(slamCallback);
        }

        sequence.Append(shakeSeq);

        if(finishCallback != null)
        {
            sequence.OnComplete(finishCallback);
        }
        return sequence;
    }

    public static void SetupCardView(
        PlayerHandView handView, 
        int handSlot, 
        CardData cardData, 
        Action<CardView> onBeginDrag, 
        Action onEndDrag)
    {
        if (cardData == null)
        {
            Debug.LogWarning("Card Data is null!");
            handView.RemoveCardByIndex(handSlot); // TODO: Do this On card slam instead of after the fact    
            return;
        }

        CardView view = handView.GetCardAtIndex(handSlot);
        if (view == null)
        {
            view = Singleton.instance.cardResourceBank.CreateCardView(
                cardData,
                handView.cardSlotList[handSlot]);
        }

        view.cardData = cardData;
        
        handView.SetCardAtIndex(handSlot, view);
    }
}
