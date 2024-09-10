using System.Collections;
using System.Collections.Generic;
using Sourav.Engine.Core.GameElementRelated;
using UnityEngine;

public class FootBallComponent : GameElement
{
    public float angle;
    public float rotationSpeed;

    void Start()
    {
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate (0,0,rotationSpeed * Time.deltaTime);
    }
}
