using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PrimaryAlchemyColor", menuName = "Scriptable Objects/PrimaryAlchemyColor")]
    public class PrimaryAlchemyColor : AlchemyColor
    {
        public override float HitThisWith(AlchemyColor otherColor)
        {
            return otherColor.colorType.colorTypeEnum switch
            {
                ColorTypeEnum.Primary =>
                    this == otherColor ? otherColor.colorType.GetDamageMultiplierWhenHittingOther(colorType) : 0f,
                ColorTypeEnum.Secondary =>
                    otherColor.colorType.GetDamageMultiplierWhenHittingOther(colorType) * otherColor.HasHit(this),
                ColorTypeEnum.Complex =>
                    otherColor.colorType.GetDamageMultiplierWhenHittingOther(colorType) + otherColor.HasHit(this),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public override float HasHit(AlchemyColor targetHit)
        {
            return targetHit.HitThisWith(this);
        }
        
        public override List<AlchemyColor> GetComponenti()
        {
            return new List<AlchemyColor>{this};
        }
    }
}