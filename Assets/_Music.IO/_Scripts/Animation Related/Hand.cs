using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sourav.Engine.Core.GameElementRelated;
using UnityEngine;

public class Hand : GameElement
{
    public Transform target;

    void OnEnable()
    {
        this.transform.DOMove(new Vector3(target.position.x, target.position.y, target.position.z), 0.8f)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
