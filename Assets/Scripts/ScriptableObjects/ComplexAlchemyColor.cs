using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ComplexAlchemyColor", menuName = "Scriptable Objects/ComplexAlchemyColor")]
    public class ComplexAlchemyColor : AlchemyColor
    {
        [SerializeField]
        private int rank;

        [SerializeField]
        [Tooltip("Used when a higher rank ComplexAlchemyColor hits a lower rank one")]
        private float multiplierOnLowerRank = 1;

        [SerializeField] private float additiveDamage;

        [SerializeField] private List<AlchemyColor> components;
        
        public override float HitThisWith(AlchemyColor otherColor)
        {
            return otherColor.colorType.colorTypeEnum switch
            {
                ColorTypeEnum.Primary =>
                    0f,
                ColorTypeEnum.Secondary =>
                    0f,
                ColorTypeEnum.Complex =>
                    otherColor == this ? //Is this the same InteractableAlchemyColor?
                        1f :
                        otherColor.HasHit(this),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override float HasHit(AlchemyColor targetHit)
        {
            return targetHit.colorType.colorTypeEnum switch
            {
                ColorTypeEnum.Primary =>
                    additiveDamage,
                ColorTypeEnum.Secondary =>
                    additiveDamage,
                ColorTypeEnum.Complex => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override float HasHit(ComplexAlchemyColor targetHit)
        {
            return targetHit.rank > rank ? 0 : multiplierOnLowerRank * (rank - targetHit.rank);
        }
        
        public override List<AlchemyColor> GetComponenti()
        {
            return components;
        }
    }
}
