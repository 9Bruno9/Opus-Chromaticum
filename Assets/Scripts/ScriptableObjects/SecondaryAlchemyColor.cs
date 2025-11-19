using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SecondaryAlchemyColor", menuName = "Scriptable Objects/SecondaryAlchemyColor")]
    public class SecondaryAlchemyColor : AlchemyColor
    {
        public PrimaryAlchemyColor componentA;
        public PrimaryAlchemyColor componentB;

        public override float HitThisWith(AlchemyColor otherColor)
        {
            return otherColor.colorType.colorTypeEnum switch
            {
                ColorTypeEnum.Primary =>
                    Mathf.Max(otherColor.HasHit(componentA), otherColor.HasHit(componentB)),
                ColorTypeEnum.Secondary =>
                    this == otherColor ? //Is this the same InteractableAlchemyColor?
                        otherColor.colorType.GetDamageMultiplierWhenHittingOther(colorType) :
                        Mathf.Max(otherColor.HasHit(componentA), otherColor.HasHit(componentB)),
                ColorTypeEnum.Complex =>
                    otherColor.colorType.GetDamageMultiplierWhenHittingOther(colorType) + otherColor.HasHit(this),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public override float HasHit(AlchemyColor targetHit)
        {
            return Mathf.Max(targetHit.HitThisWith(componentA), targetHit.HitThisWith(componentB));
        }
        
        public override List<AlchemyColor> GetComponenti()
        {
            return new List<AlchemyColor>{componentA, componentB};
        }
    }
}
