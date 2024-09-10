using System;
using UnityEngine;
using Sourav.Engine.Core.GameElementRelated;

public class Sides : GameElement
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Bullet"))
      {
         Destroy(other.gameObject);
      }
   }
}
