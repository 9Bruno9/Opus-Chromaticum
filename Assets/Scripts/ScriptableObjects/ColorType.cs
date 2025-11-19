using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableObjects
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ColorType", menuName = "Scriptable Objects/ColorType")]
    public class ColorType : ScriptableObject
    {
        [System.Serializable]
        public class DamageMultiplier
        {
            [SerializeField] private ColorType colorType;
            public ColorType ColorType => colorType;
            [SerializeField] private float multiplierValue;
            public float MultiplierValue => multiplierValue;
        }
        
        public AlchemyColor.ColorTypeEnum colorTypeEnum;
        
        [SerializeField]
        [Tooltip("If one of the ColorType below is hit with this one then apply the corresponding damage multiplier. If not set DEFAULT VALUE:1")]
        private List<DamageMultiplier> damageMultipliers;
        
        /// <summary>
        /// Recover the damage multiplier when this ColorType hits otherColorType
        /// </summary>
        /// <param name="otherColorType">The ColorType that has been hit</param>
        /// <returns>The multiplier to the damage dealt</returns>
        public float GetDamageMultiplierWhenHittingOther(ColorType otherColorType)
        {
            foreach (var multiplier in damageMultipliers.Where(multiplier => otherColorType == multiplier.ColorType))
            {
                return multiplier.MultiplierValue;
            }
            return 1;
        }
    }
}
