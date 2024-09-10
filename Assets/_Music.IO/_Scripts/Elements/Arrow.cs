using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sourav.Engine.Core.GameElementRelated;
using Sourav.Utilities.Extensions;
using UnityEngine;

public class Arrow : GameElement
{
   public Transform target;
   public GameObject OriginalGameObject;
   private Sequence _sequence;

   public void OnAnimate()
   {
      OriginalGameObject.Show();
      _sequence.Append(OriginalGameObject.transform
         .DOMove(new Vector3(target.position.x, target.position.y, target.position.z), 0.7f).SetLoops(-1, LoopType.Yoyo)
         .SetEase(Ease.OutSine));
   }

   public void DisableAnimation()
   {
      _sequence.Kill();
      OriginalGameObject.Hide();
   }

   public void DisableArrow()
   {
      OriginalGameObject.Hide();
   }

   public void EnableArrow()
   {
      OriginalGameObject.Show();
   }
}
