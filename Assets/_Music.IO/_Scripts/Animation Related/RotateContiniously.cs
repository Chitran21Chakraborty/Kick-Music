using System.Collections;
using System.Collections.Generic;
using Sourav.Engine.Core.GameElementRelated;
using UnityEngine;

public class RotateContiniously : GameElement
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,-6.0f * 2.5f * Time.deltaTime);
    }
}
