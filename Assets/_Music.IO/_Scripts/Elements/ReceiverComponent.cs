using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Sourav.Engine.Core.GameElementRelated;
using Sourav.Utilities.Extensions;

public class ReceiverComponent : GameElement
{
   public Animator receiverAnimator;
   public GameObject collidedOnceObj;
   public ParticleSystem[] receiverParticle;
   private int a = 0;
   bool once = true;

   void Start()
   {
      collidedOnceObj.Show();
   }
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Bullet"))
      {
         a++;
         collidedOnceObj.Hide();
         receiverAnimator.Play("FootBall Net RigAction");
         App.GetLevelData().arrowPump.OnAnimate();
         App.GetLevelData().arrowPump.OnFade();
         for (int i = 0; i < receiverParticle.Length; i++)
         {
            receiverParticle[i].Play();
         }
         Destroy(other.gameObject);
      }
   }

   public bool Received()
   {
      if (a > 0)
      {
         a = 0;
         return true;
      }
      else
      {
         return false;
      }
   }
}