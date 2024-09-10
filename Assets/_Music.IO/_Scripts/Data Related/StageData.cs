using System.Collections;
using System.Collections.Generic;
using Sourav.Engine.Editable.DataRelated;
using UnityEngine;

public class StageData : CommonData
{
    public List<LevelInfo> levelInfo;
}

[System.Serializable]
public class LevelInfo
{
    public List<GridInfo> gridInfo;
    public int coin;
    public List<BulletClass> shooter;
}

[System.Serializable] 
public class GridInfo
{
    public Vector3Int gridStatus;
}

[System.Serializable]
public class BulletClass
{
    public Vector2Int bulletAxis;
    public Vector2Int ballRotationAxis;
    public Vector2Int shooterLerpedPosition;
    public Vector2 targetAngle;
    public Vector2Int receiverLerpedPosition;
}