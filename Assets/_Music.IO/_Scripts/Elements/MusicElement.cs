using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sourav.Engine.Core.GameElementRelated;
using Sourav.Engine.Editable.NotificationRelated;
using Sourav.Utilities.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class MusicElement : GameElement
{
    public int index;
    public GameObject lockObj;
    public GameObject upgradeButtonActive, upgradeButtonInactive;
    public bool isClicked;
    public int coinNeededToPurchase;
    private int valueToDeduct;
    [SerializeField]private float speed;
    public CanvasGroup popUpImg;
    public Hand hand;
    public CanvasGroup cg;
    public ParticleSystem particle;

    public void OnPressedButton()
    {
        if (!isClicked)
        {
            App.GetLevelData().UnlockedMusicalElementIndex[index] = new Vector3Int(App.GetLevelData().UnlockedMusicalElementIndex[index].x, 1, App.GetLevelData().UnlockedMusicalElementIndex[index].z);
            App.GetLevelData().UnlockedElement[index + 1] = 1;
            particle.Play();
            cg.alpha = 0.5f;
            valueToDeduct = App.GetLevelData().Coins - coinNeededToPurchase;
            isClicked = true;
            StartCoroutine(CoinDeduction());
            HideHand();
            App.GetUiData().newItemUnlockedPopUp.gameObject.Hide();
            App.Notify(Notification.HapticSuccess);
            App.Notify(Notification.PurchaseSuccessful);
        }
        else
        {
            //some pop up will be shown on ui
            Sequence sequence = DOTween.Sequence();
            sequence.Append(App.GetUiData().enoughCoinImg.DOFade(1, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                sequence.Append(App.GetUiData().enoughCoinImg.DOFade(0, 0.3f).SetDelay(1.5f).SetEase(Ease.OutBack)
                    .OnComplete(
                        () =>
                        {
                            sequence.Kill();
                        }));
            }));
        }
    }

    IEnumerator CoinDeduction()
    {
        float t = (float)App.GetLevelData().Coins;
        while (t >= valueToDeduct)
        {
            t -= Time.deltaTime * speed;
            App.GetLevelData().Coins = (int) (t + 1);
            App.GetUiData().currentCoinText.text = App.GetLevelData().Coins.ToString();
            if (t <= valueToDeduct)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(popUpImg.DOFade(1f, 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    sequence.Append(popUpImg.DOFade(0, 0.5f).SetDelay(2f).SetEase(Ease.OutSine).OnComplete(() =>
                    {
                        sequence.Kill();
                    }));
                }));
            }
            yield return null;
        }
    }

    public void ShowHand()
    {
        StartCoroutine(OnShowHand());
    }

    public void HideHand()
    {
        hand.gameObject.Hide();
    }

    IEnumerator OnShowHand()
    {
        yield return new WaitForSeconds(2.5f);
        hand.gameObject.Show();
    }
}