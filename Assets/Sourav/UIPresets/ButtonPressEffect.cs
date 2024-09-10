﻿// using Sirenix.OdinInspector;
using Sourav.Engine.Core.GameElementRelated;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sourav.UIPresets
{
    public class ButtonPressEffect : GameElement, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private bool isChangeImage;
        [SerializeField] private bool isMoveDownPosition;
        [SerializeField] private GameObject pushDownObject;
        [SerializeField] private Vector3 onPointerDownPosition;
        [SerializeField] private Vector3 onPointerUpPosition;
        [SerializeField] private bool isLocal;

        [SerializeField] private Image image;
        [SerializeField] private Sprite pressUpImage;
        [SerializeField] private Sprite pressDownImage;

        // [ShowIf("isMoveDownPosition", true)]
        // [Button()]
        private void SetPositionDown()
        {
            if (isLocal)
            {
                onPointerDownPosition = pushDownObject.transform.localPosition;
            }
            else
            {
                onPointerDownPosition = pushDownObject.transform.position;
            }
        }
        
        // [ShowIf("isMoveDownPosition", true)]
        // [Button()]
        private void SetPositionUp()
        {
            if (isLocal)
            {
                onPointerUpPosition = pushDownObject.transform.localPosition;
            }
            else
            {
                onPointerUpPosition = pushDownObject.transform.position;
            }
        }

        // [ShowIf("isMoveDownPosition", true)]
        // [Button()]
        private void SetDefault()
        {
            if (isLocal)
            {
                pushDownObject.transform.localPosition = onPointerUpPosition;
            }
            else
            {
                pushDownObject.transform.position = onPointerUpPosition;
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (isChangeImage)
            {
                if (image != null)
                {
                    if (pressDownImage != null)
                    {
                        image.sprite = pressDownImage;
                    }
                }
            }
            else
            {
                if (isLocal)
                {
                    pushDownObject.transform.localPosition = onPointerDownPosition;
                }
                else
                {
                    pushDownObject.transform.position = onPointerDownPosition;
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isChangeImage)
            {
                if (image != null)
                {
                    if (pressUpImage != null)
                    {
                        image.sprite = pressUpImage;
                    }
                }
            }
            else
            {
                if (isLocal)
                {
                    pushDownObject.transform.localPosition = onPointerUpPosition;
                }
                else
                {
                    pushDownObject.transform.position = onPointerUpPosition;
                }
            }
        }
    }
}
