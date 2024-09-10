using System;
using UnityEngine;
using Sourav.Engine.Core.GameElementRelated;
using Sourav.Utilities.Extensions;
using UnityEngine.EventSystems;

public class HandDisable : GameElement, IBeginDragHandler
{
   public GameObject[] handObjects;
   
   public void OnBeginDrag(PointerEventData eventData)
   {
      for (int i = 0; i < handObjects.Length; i++)
      {
         if (handObjects[i].activeInHierarchy)
         {
            handObjects[i].Hide();
         }
      }
   }
}
