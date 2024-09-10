using System;
using System.Collections;
using System.Collections.Generic;
using Sourav.Engine.Core.GameElementRelated;
using UnityEngine;

public class MoveTest : GameElement
{
   private bool isMoving = false;
   private float smoothTime = 1f;
   private int xDegMult, yDegMult;

   private void Start()
   {
      //Set xDegMult and yDegMult here
      isMoving = true;
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.tag == "Director")
      {
         isMoving = false;
         //xDegMult = other.gameObject.GetComponent<>()
         //yDegMult = other.gameObject.GetComponent<>()
         isMoving = true;
         StartCoroutine(MoveTowardsDirection(xDegMult, yDegMult, isMoving));
      }
   }

   IEnumerator MoveTowardsDirection(int x, int y, bool _isMoving)
   {
      while (_isMoving)
      {
         transform.Translate(x * smoothTime * Time.deltaTime, y * smoothTime * Time.deltaTime, 0);
         yield return new WaitForFixedUpdate();
      }
   }
}
