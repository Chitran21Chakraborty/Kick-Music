using System;
using System.Collections;
using System.Collections.Generic;
using Sourav.Engine.Core.GameElementRelated;
using Sourav.Engine.Editable.NotificationRelated;
using Sourav.Utilities.Extensions;
using UnityEngine;

public class Shooter : GameElement
{
   public BulletMove bullet;
   public bool isButtonPressed;
   public Vector2Int bulletDirection;
   private bool once;
   public Animator animator;
   public bool canShoot = true;
   public GameObject dummyFootball;
   public Transform parentObj;
   public SenderComponent senderComp;
   public GameObject[] particleObject;
   
   private void OnMouseDown()
   {
      if (canShoot)
      {
         if (!isButtonPressed)
         {
            isButtonPressed = true;
            if (App.GetUiData().shootTutorialText.gameObject.activeInHierarchy)
            {
               App.GetUiData().shootTutorialText.gameObject.Hide();
               App.GetUiData().shootText.Hide();
               App.GetLevelData().IsTutorialOver = true;
               App.GetUiData().tutorialParent.Hide();
            }
            StartCoroutine("SpawnBullet");
            for (int i = 0; i < particleObject.Length; i++)
            {
               particleObject[i].Show();
            }
            senderComp.activeArrow.DisableArrow();
         }
         else
         {
            isButtonPressed = false;
            StopCoroutine("SpawnBullet");
            senderComp.activeArrow.EnableArrow();
            for (int i = 0; i < particleObject.Length; i++)
            {
               particleObject[i].Hide();
            }
         }
         App.Notify(Notification.TapPlayerSound);
         App.Notify(Notification.HapticMild);
      }
      else
      {
         return;
      }
   }
   

   IEnumerator SpawnBullet()
   {
      while (isButtonPressed)
      {
         if (!App.GetLevelData().isSpawning)
         {
            yield return null;
         }
         else
         {
            dummyFootball.Show();
            animator.Play("PlayerKick");
            yield return new WaitForSeconds(0.35f);
            GameObject obj = Instantiate(bullet.gameObject, parentObj);
            obj.GetComponent<BulletMove>().xDegreeMultiplier = bulletDirection.x;
            obj.GetComponent<BulletMove>().yDegreeMultiplier = bulletDirection.y;
            obj.Show();
            dummyFootball.Hide();
            yield return new WaitForSeconds(0.7f);
         }
      }
   }
}
