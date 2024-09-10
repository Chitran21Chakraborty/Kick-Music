using System;
using System.Collections;
using System.Collections.Generic;
using Sourav.Engine.Core.GameElementRelated;
using Sourav.Utilities.Extensions;
using UnityEngine;

public class SenderComponent : GameElement
{
   public Shooter shooter;
   public Transform playerTransform;
   public Arrow[] arrows;
   public Arrow activeArrow;
   private Coroutine routine;

   public void OnEnable()
   {
       if (App.GetLevelData().IsTutorialOver)
       {
           routine = StartCoroutine(ArrowAnimation());
       }
   }

   IEnumerator ArrowAnimation()
   {
      yield return new WaitForSeconds(1.5f);
      activeArrow.OnAnimate();
   }

   private void OnDisable()
   {
       if (routine != null)
       {
           StopCoroutine(routine);
       }
   }

   private void OnDestroy()
   {
       if (routine != null)
       {
           StopCoroutine(routine);
       }
   }
}
