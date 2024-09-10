using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sourav.Engine.Core.GameElementRelated;
using Sourav.Engine.Editable.NotificationRelated;
using UnityEngine;

public class ArrowPump : GameElement
{
   public bool isPlaying;
   public bool isStartedFading;
   public CanvasGroup cg;
   
   public void OnAnimate()
   {
      if (!isPlaying)
      {
         isPlaying = true;
         /*Sequence sequence = DOTween.Sequence();
         sequence.Append(App.GetUiData().nextArrowImage.transform.DOPunchScale(new Vector3(0.3f,0.3f,0.3f), 0.2f).OnComplete(
            () =>
            {
               sequence.Kill();
               isPlaying = false;
            }));*/
         Sequence sequence = DOTween.Sequence();
         sequence.Append(App.GetUiData().nextArrowImage.transform.DOMove(new Vector3(App.GetUiData().nextTarget.position.x, App.GetUiData().nextTarget.position.y, App.GetUiData().nextTarget.position.z),0.4f).SetEase(Ease.OutSine).OnComplete(
            () =>
            {
               sequence.Kill();
            }));
      }
   }
   
   public void OnFade()
   {
      if (isStartedFading)
      {
         isStartedFading = false;
         App.Notify(Notification.PartialAchievementSoundPlay);
         Sequence sequence = DOTween.Sequence();
         sequence.Append(cg.DOFade(1, 0.4f).SetEase(Ease.OutSine).OnComplete(() =>
         {
            sequence.Kill();
         }));
      }
   }
}
