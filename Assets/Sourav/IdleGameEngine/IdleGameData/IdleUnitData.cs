using UnityEngine;

namespace Sourav.IdleGameEngine.IdleGameData
{
    [CreateAssetMenu()]
    public class IdleUnitData : ScriptableObject
    {
        public IdleCurrency.IdleCurrency.IdleCurrency baseCost;
        public float percentageOnBaseCost;
        public float percentageOnNewCost;
        public int totalCount;
        public bool isLockedByDefault;
        public bool canBeUnlockedWithCost;
        public IdleCurrency.IdleCurrency.IdleCurrency unlockCost;
        public bool hasTimerBeforeNextClick;
        public float defaultTimer;
        
        [Space(10)][Header("Growth Related")]
        public float separatorPercentage;
        public float valueSeparatorPercentage;
    }
}
